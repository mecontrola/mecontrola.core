﻿using MeControla.Core.Repositories;
using MeControla.Core.Tests.Mocks.Datas.Entities;

namespace MeControla.Core.Tests.Mocks.Datas.Repositories
{
    public class UserPermissionRepository(IDbAppContext context)
        : BaseManyAsyncRepository<UserPermission>(context, context.UserPermissions), IUserPermissionRepository
    { }
}