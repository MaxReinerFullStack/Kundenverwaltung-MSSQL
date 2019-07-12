using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataModel
{
  /**  public class FE6CodeConfig : DbConfiguration
    {
        public FE6CodeConfig()
        {
            this.SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
        }
    }

    [DbConfigurationType(typeof(FE6CodeConfig))]**/
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("name=CustomerConn")
        {
        }
        public IDbSet<Customer> Customers { get; set; }



        public IDbSet<User> Users { get; set; }

        // Fügen Sie ein 'DbSet' für jeden Entitätstyp hinzu, den Sie in das Modell einschließen möchten. Weitere Informationen 
        // zum Konfigurieren und Verwenden eines Code First-Modells finden Sie unter 'http://go.microsoft.com/fwlink/?LinkId=390109'.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>();
            modelBuilder.Entity<User>();
        
      
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


    }
    public class FakeApplicationDbContext :  IApplicationDbContext
    {
        public IDbSet<Customer> Customers { get; set; }

     

        public IDbSet<User> Users { get; set; }
        public FakeApplicationDbContext()
        {
        }

        public int SaveChanges()
        {
            return 0;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

}