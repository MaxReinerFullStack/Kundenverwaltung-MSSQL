using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;


namespace ViewModel
{
    public class DataGridViewModel : ViewModelBase
    {
        ObservableCollection<CustomerViewModel> customersInfo = new ObservableCollection<CustomerViewModel>();
        private IDialogCoordinator dialogCoordinator;

        public static bool DatabaseIsInitialized = false;

        public DataGridViewModel(IDialogCoordinator instance) :this()
        {
            dialogCoordinator = instance;
        }

        public RelayCommand SaveCommand { get; set; }

        public RelayCommand ClearCommand { get; set; }

        public CustomerViewModel NewCustomer
        {
            get { return GetValue(() => NewCustomer); }
            set { SetValue(() => NewCustomer, value); }
        }
        public DataGridViewModel()
        {
            CustomerViewModel sampleCustomerViewModel = CustomerViewModel.getSampleCustomerView();

            // Check if database is initialized
            // If yes, take customers from there and create the view
            if (DatabaseIsInitialized)
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var customers = ctx.Customers.ToList<Customer>();
                    if (customers.Count() > 0)
                    {
                        // Load user and contact person and create view
                        foreach (var customer in customers)
                        {

                            var user = ctx.Users.Where(o => o.Id == 1).FirstOrDefault<User>();
                            customer.User = user;
                            CustomerViewModel customerViewModel = CustomerViewModel.getCustomerView(customer);
                            customersInfo.Add(customerViewModel);

                        }

                    } // Otherwise create a sample Customer view
                    else customersInfo.Add(sampleCustomerViewModel);

                }
                
                
            }
        }

        public int DeleteCustomers(ObservableCollection<CustomerViewModel> customerInfosToDelete)
        {
            int customersDeleted = 0;
            List<Customer> customersToDelete = new List<Customer>();
            foreach (CustomerViewModel customerViewModel in customerInfosToDelete)
            {
                Customer customer = customerViewModel.CustomerInfo;
                CustomerViewModel modelToRemove = CustomersInfo.Where(i => i.ID == customerViewModel.ID).FirstOrDefault();
                if (modelToRemove != null) CustomersInfo.Remove(modelToRemove); else return customersDeleted;
                customersToDelete.Add(customer);
            }
            using (var ctx = new ApplicationDbContext())
            {
                foreach (Customer customerToDelete in customersToDelete)
                {
                    Customer customer = ctx.Customers.Where(x => x.Id == customerToDelete.Id).FirstOrDefault();
                    if (customer != null)
                    {
                        customer.User = null;
                        ctx.Customers.Remove(customer);
                        customersDeleted++;
                        ctx.SaveChanges();
                    }
                    
                }
            }

            return customersDeleted;
        }

        public void UpdateAllCustomers()
        {
            foreach (CustomerViewModel customerInfo in CustomersInfo)
            {
                customerInfo.InsertOrUpdateCustomerFromView();
            }
            
        }

       

        public void InsertNewCustomer(CustomerViewModel customerViewModel)
        {
            this.CustomersInfo.Add(customerViewModel);
            NotifyPropertyChanged("CustomersInfo");
        }
        public void AddOrUpdateCustomer(CustomerViewModel customerViewModel)
        {
            var customerViewToUpdate = CustomersInfo.FirstOrDefault(i => i.ID == customerViewModel.ID);
            if (customerViewToUpdate == null)
            {
                // Insert
                this.InsertNewCustomer(customerViewModel);
            }
            else
            {
                // Update
                foreach (PropertyInfo property in typeof(CustomerViewModel).GetProperties())
                {
                    property.SetValue(customerViewToUpdate, property.GetValue(customerViewModel, null), null);
                }
                NotifyPropertyChanged("CustomersInfo");
            }
            
        }

        public ObservableCollection<CustomerViewModel> CustomersInfo
        {
            get
            {
                return customersInfo;
            }
            set
            {
                customersInfo = value;
                NotifyPropertyChanged("CustomersInfo");
            }
           
        }
     
    }
}
