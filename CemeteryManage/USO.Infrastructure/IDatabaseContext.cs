using System;
using USO.Domain;


namespace USO.Infrastructure
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using USO.Core;
    using USO.Domain;

    public interface IDatabaseContext : IPerRequestDependency //IDependency
    {
        #region 基础数据
        IDbSet<CustomerType> CustomerTypes { get; set; }
        IDbSet<CemeteryAreas> CemeteryAreas { get; set; }
        IDbSet<CemeteryColumns> CemeteryColumns { get; set; }
        IDbSet<CemeteryRows> CemeteryRows { get; set; }
        IDbSet<CustomerStatus> CustomerStatus { get; set; }
        IDbSet<Nationality> Nationalitys { get; set; }
        IDbSet<SecurityLevel> SecurityLevels { get; set; }
        IDbSet<ServiceLevel> ServiceLevels { get; set; }
        IDbSet<TombstoneType> TombstoneTypes { get; set; }
        IDbSet<PaymentStatus> PaymentStatus { get; set; }
        IDbSet<Department> Departments { get; set; }
        #endregion
        #region 日志
        IDbSet<SysLog> SysLogs { get; set; }
        #endregion
        #region 客户
        IDbSet<Customer> Customers { get; set; }
        #endregion
        #region 墓碑
        IDbSet<Tombstone> Tombstones { get; set; }
        IDbSet<TombstoneBuriedPeopleMap> TombstoneBuriedPeopleMaps { get; set; }
        #endregion
        #region 用户角色
        IDbSet<User> Users { get; set; }
        IDbSet<Function> Functions { get; set; }
        IDbSet<RoleFunctionMap> RoleFunctionMaps { get; set; }
        IDbSet<Role> Roles { get; set; }
        IDbSet<UserRoleMap> UserRoleMaps { get; set; }
        #endregion

        int SaveChanges();
        IDbSet<TEntity> GetSet<TEntity>() where TEntity : class;

        IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters);

        void ExecuteSqlCommand(string sql, params object[] parameters);

        void AttachAsModified<T>(T current, T original) where T : class;
        void AttachAsModified<T>(object dto, T original) where T : class;
        void AttachAsAdded<T>(T entity) where T : class;
        void AttachAsDeleted<T>(T entity) where T : class;
        void AttachAsUnchanged<T>(T entity) where T : class;
        void OpenAutoDetectChangesEnabled();
        void CloseAutoDetectChangesEnabled();

        object GetObject(Type type, int id);
    }
}