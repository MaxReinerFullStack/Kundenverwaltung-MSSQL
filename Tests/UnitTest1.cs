using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModel;
using System.Linq;


namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CustomerIsCorrectAfterDBCreation()
        {
            var fakeDb = new ApplicationDbContext();
            fakeDb.Customers = new FakeDbSet<Customer>();

            Customer customerInfo = Customer.getSampleCustomer();

            fakeDb.Customers.Add(customerInfo);
            fakeDb.Users = new FakeDbSet<User>();
            int result1 = fakeDb.SaveChanges();
            var realDb = new ApplicationDbContext();
            realDb.Customers.Add(customerInfo);
            int result2 = realDb.SaveChanges();
            var customer2 = fakeDb.Customers.FirstOrDefault<Customer>();
            var customerSaved = realDb.Customers.Where(o => o.Kundenbezeichnung == customer2.Kundenbezeichnung).FirstOrDefault<Customer>();
        
            Assert.AreEqual(customerSaved.Kundenbezeichnung, customer2.Kundenbezeichnung);


        }
    }
}
