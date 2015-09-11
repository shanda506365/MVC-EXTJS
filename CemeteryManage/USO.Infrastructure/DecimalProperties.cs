
namespace USO.Infrastructure
{
    using System.Data.Entity;
    using USO.Domain;

    public static class DecimalProperties
    {
        public static void Apply(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SalesOrder>().Property(a => a.Price).HasPrecision(19, 5); 
        }
    }
}
