﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MeControla.Core.Repositories
{
    public interface IBaseDbContextFactory<TDbContext> : IDesignTimeDbContextFactory<TDbContext>, IDisposable
        where TDbContext : DbContext
    { }

    public abstract class BaseDbContextFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TDbContext>
        : IDesignTimeDbContextFactory<TDbContext>, IDisposable
        where TDbContext : DbContext
    {
        private TDbContext context;

        public TDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();

            Configure(optionsBuilder);

            context = CreateInstanceDbContext(optionsBuilder);

            return context;
        }

#if DEBUG
        protected virtual
#else
        private static
#endif
        TDbContext CreateInstanceDbContext(DbContextOptionsBuilder<TDbContext> optionsBuilder)
           => (TDbContext)Activator.CreateInstance(typeof(TDbContext), [optionsBuilder.Options]);

        protected abstract void Configure(DbContextOptionsBuilder<TDbContext> options);

        public void Dispose()
        {
            context?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}