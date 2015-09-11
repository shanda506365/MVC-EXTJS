namespace USO.Infrastructure
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;

    public static class DbContextExtensions
    {
        public static void AttachAsModified<T>(this DbSet<T> dbSet, T current, T original, DbContext dbContext) where T : class
        {
            if (dbSet == null)
            {
                throw new ArgumentNullException("dbSet");
            }
            if (current == null)
            {
                throw new ArgumentNullException("current");
            }
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            DbEntityEntry<T> dbEntityEntry = dbContext.Entry<T>(current);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                dbSet.Attach(current);
            }
            else
            {
                dbEntityEntry.State = EntityState.Modified;
            }
            ObjectContext objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            ObjectStateEntry objectStateEntry = ObjectContextUtilities.AttachAsModifiedInternal<T>(current, original, objectContext);
            if (objectStateEntry.State != EntityState.Modified)
            {
                dbEntityEntry.State = EntityState.Modified;
            }
        }
        public static void AttachAsModified<T>(this DbSet<T> dbSet, T entity, DbContext dbContext) where T : class
        {
            if (dbSet == null)
            {
                throw new ArgumentNullException("dbSet");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            DbEntityEntry<T> dbEntityEntry = dbContext.Entry<T>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }
    }
}
