using System.Data.Entity;

namespace DataModel
{
    public interface IApplicationDbContext
    {
        IDbSet<Customer> Customers { get; set; }

        IDbSet<User> Users { get; set; }

        int SaveChanges();
        
    }
}