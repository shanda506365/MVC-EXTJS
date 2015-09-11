using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using USO.Domain;


namespace USO.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using USO.Domain;

    public class USOEntities : DbContext, IDatabaseContext, IDisposable
    {
        #region 基础数据
        public IDbSet<CustomerType> CustomerTypes { get; set; }
        public IDbSet<CemeteryAreas> CemeteryAreas { get; set; }
        public IDbSet<CemeteryColumns> CemeteryColumns { get; set; }
        public IDbSet<CemeteryRows> CemeteryRows { get; set; }
        public IDbSet<CustomerStatus> CustomerStatus { get; set; }
        public IDbSet<Nationality> Nationalitys { get; set; }
        public IDbSet<SecurityLevel> SecurityLevels { get; set; }
        public IDbSet<ServiceLevel> ServiceLevels { get; set; }
        public IDbSet<TombstoneType> TombstoneTypes { get; set; }
        public IDbSet<PaymentStatus> PaymentStatus { get; set; }
        public IDbSet<Department> Departments { get; set; }
        #endregion
        #region 日志
        public IDbSet<SysLog> SysLogs { get; set; }
        #endregion
        #region 客户
        public IDbSet<Customer> Customers { get; set; }
        #endregion
        #region 墓碑
        public IDbSet<Tombstone> Tombstones { get; set; }
        public IDbSet<TombstoneBuriedPeopleMap> TombstoneBuriedPeopleMaps { get; set; }
        #endregion
        #region 用户角色
        public IDbSet<User> Users { get; set; }
        public IDbSet<Function> Functions { get; set; }
        public IDbSet<RoleFunctionMap> RoleFunctionMaps { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<UserRoleMap> UserRoleMaps { get; set; }
        #endregion
        public USOEntities()
            : base()
        {
            Configuration.ValidateOnSaveEnabled = false;

            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 120; //seconds
            ((IObjectContextAdapter)this).ObjectContext.Connection.Open();
            //使用数据库配置的默认隔离级别。
            this.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");//防止sql死锁
        }

        public IDbSet<TEntity> GetSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //运用所有decimal属性的精度设置
            DecimalProperties.Apply(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TEntity>(sql, parameters).ToList();
        }

        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            Database.ExecuteSqlCommand(sql, parameters);
        }

        public void AttachAsModified<T>(T current, T original) where T : class
        {
            this.Set<T>().AttachAsModified(current, original, this);
        }

        //DTO-->Entity
        public void AttachAsModified<T>(object dto, T original) where T : class
        {
            this.Entry<T>(original).CurrentValues.SetValues(dto);
        }

        public void AttachAsAdded<T>(T entity) where T : class
        {
            //这是另一种添加新实体的方法：类似于this.Set<T>().Add(entity); 
            this.Entry<T>(entity).State = EntityState.Added;
        }

        public void AttachAsDeleted<T>(T entity) where T : class
        {
            this.Entry<T>(entity).State = EntityState.Deleted;
        }

        public void AttachAsUnchanged<T>(T entity) where T : class
        {
            this.Entry<T>(entity).State = EntityState.Unchanged;
        }

        public object GetObject(Type type, int id)
        {
            return Set(type).Find(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (((IObjectContextAdapter)this).ObjectContext.Connection.State == ConnectionState.Open)
                ((IObjectContextAdapter)this).ObjectContext.Connection.Close();

            base.Dispose(disposing);
        }


        public void OpenAutoDetectChangesEnabled()
        {
            Configuration.AutoDetectChangesEnabled = true;
        }

        public void CloseAutoDetectChangesEnabled()
        {
            Configuration.AutoDetectChangesEnabled = false;
        }
    }
}