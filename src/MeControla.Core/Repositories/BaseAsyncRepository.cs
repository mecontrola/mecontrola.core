﻿using MeControla.Core.Data.Entities;
using MeControla.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MeControla.Core.Repositories
{
    public abstract class BaseAsyncRepository<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TEntity>(IDbContext context, DbSet<TEntity> dbSet)
        : ContextRepository<TEntity>(context, dbSet), IAsyncRepository<TEntity>
         where TEntity : class, IEntity
    {
        public virtual async Task<long> CountAsync(CancellationToken cancellationToken)
            => await dbSet.LongCountAsync(cancellationToken);

        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => await dbSet.LongCountAsync(predicate, cancellationToken);

        public virtual async Task<TEntity> CreateAsync(TEntity obj, CancellationToken cancellationToken)
        {
            Detach(obj, EntityState.Added);

            await ApplyAlterContextAsync(dbSet => dbSet.Add(obj), cancellationToken);

            return obj;
        }

        public virtual async Task<bool> UpdateAsync(TEntity obj, CancellationToken cancellationToken)
        {
            Detach(obj, EntityState.Modified);

            return await ApplyAlterContextAsync(dbSet => dbSet.Update(obj), cancellationToken);
        }

        public virtual async Task<bool> RemoveAsync(TEntity obj, CancellationToken cancellationToken)
        {
            Detach(obj, EntityState.Deleted);

            return await ApplyAlterContextAsync(dbSet => dbSet.Remove(obj), cancellationToken);
        }

        public virtual async Task<IList<TEntity>> FindAllPagedAsync(IPaginationFilter paginationFilter, CancellationToken cancellationToken)
            => await FindAllPagedAsync(paginationFilter, null, cancellationToken);

        public virtual async Task<IList<TEntity>> FindAllPagedAsync(IPaginationFilter paginationFilter, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => await dbSet.SetPagination(paginationFilter).SetPredicate(predicate).ToListAsync(cancellationToken);

        public virtual async Task<IList<TEntity>> FindAllAsync(CancellationToken cancellationToken)
            => await FindAllAsync(null, cancellationToken);

        public virtual async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => await dbSet.SetPredicate(predicate).ToListAsync(cancellationToken);

        public virtual async Task<TEntity> FindAsync(long id, CancellationToken cancellationToken)
            => await FindAsync(itm => itm.Id.Equals(id), cancellationToken);

        public virtual async Task<TEntity> FindAsync(Guid uuid, CancellationToken cancellationToken)
            => await FindAsync(itm => itm.Uuid.Equals(uuid), cancellationToken);

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => await dbSet.AsNoTracking().Where(predicate).FirstOrDefaultAsync(cancellationToken);

        public async Task<bool> ExistsAsync(long id, CancellationToken cancellationToken)
            => await ExistsAsync(itm => itm.Id.Equals(id), cancellationToken);

        public async Task<bool> ExistsAsync(Guid uuid, CancellationToken cancellationToken)
            => await ExistsAsync(itm => itm.Uuid.Equals(uuid), cancellationToken);

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
            => await dbSet.AnyAsync(predicate, cancellationToken);

        protected virtual void Detach(TEntity entity, EntityState entityState)
        {
            var local = dbSet.Local.FirstOrDefault(itm => itm.Id.Equals(entity.Id));
            var id = local?.Id ?? 0;

            if (id != 0)
                context.Entry(local).State = EntityState.Detached;

            context.Entry(entity).State = entityState;
        }

        private async Task<bool> ApplyAlterContextAsync(Action<DbSet<TEntity>> action, CancellationToken cancellationToken)
        {
            action(dbSet);

            return await context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}