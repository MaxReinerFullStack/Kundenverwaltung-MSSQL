
using DataModel;

using MahApps.Metro.Controls.Dialogs;
using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ViewModel
{
    public class CustomerViewModel : ViewModelBase, IObservable<Customer>, ICloneable
    {

        private Customer customerInfo;
        private IDialogCoordinator dialogCoordinator;

        public CustomerViewModel(IDialogCoordinator instance) 
        {
            dialogCoordinator = instance;
            this.CustomerInfo = new Customer();
            SaveDataCommand = new RelayCommand(SaveData);

        }
       

        public static CustomerViewModel getSampleCustomerView()
        {
            Customer sample = Customer.getSampleCustomer();
            
            return new CustomerViewModel(DialogCoordinator.Instance) { CustomerInfo = sample };
        }
        public static CustomerViewModel getCustomerView(Customer customer)
        {
            return new CustomerViewModel(DialogCoordinator.Instance) { CustomerInfo = customer };
        }

        public static CustomerViewModel getCancelObject(Customer customer)
        {
            CustomerViewModel cancelCustomerView = new CustomerViewModel(DialogCoordinator.Instance) { CustomerInfo = customer };
            return cancelCustomerView;
        }

        public static CustomerViewModel getNewCustomerView()
        {
            CustomerViewModel newCustomerView = new CustomerViewModel(DialogCoordinator.Instance) { CustomerInfo = new Customer() };

            using (var ctx = new ApplicationDbContext())
            {


                Customer customer = ctx.Customers.Where(f => f.Id == ctx.Customers.Max(f2 => f2.Id)).FirstOrDefault();
                if (customer == null)
                {
                    customer = new Customer();
                    customer.Id = 1;
                }

                newCustomerView.CustomerInfo.Id = customer.Id+1;
                newCustomerView.CustomerInfo.Änderungsdatum = DateTime.Now;


                // TODO: Hardcodierte Objekte durch Liste ersetzen und Liste in CustomerViewModel speichern
                newCustomerView.User = new User() { Id = 1, Firstname = "R.", Lastname = "Klauss" };
               
                return newCustomerView;
            }
        }

       

        public int ID
        {
            get { return customerInfo.Id; }
            set { customerInfo.Id = value; }
        }
        [Required(ErrorMessage = "Kundenbezeichnung wird benötigt")]
        public string Kundenbezeichnung
        {
            get { return customerInfo.Kundenbezeichnung; }
            set { customerInfo.Kundenbezeichnung = value;
                SetValue(() => Kundenbezeichnung, value); }
        }

        public string Ort
        {
            get { return customerInfo.Ort; }
            set { customerInfo.Ort = value; }
        }

        public string Bundesland
        {
            get { return customerInfo.Bundesland; }
            set { customerInfo.Bundesland = value; }
        }

        public string Adresse
        {
            get { return customerInfo.Adresse; }
            set { customerInfo.Adresse = value; }
        }
        [Required(ErrorMessage = "Telefonnummer wird benötigt")]
        [Phone(ErrorMessage = "Keine gültige Telephonnummer")]
        public string Telefonnummer_1
        {
            get { return customerInfo.Telefonnummer1; }
            set { customerInfo.Telefonnummer1 = value;
                SetValue(() => Telefonnummer_1, value);
            }
        }

        public string Telefonnummer_2
        {
            get { return customerInfo.Telefonnummer2; }
            set { customerInfo.Telefonnummer2 = value; }
        }
        [Required(ErrorMessage = "E-Mail Adresse wird benötigt")]
        [EmailAddress(ErrorMessage = "Keine gültige E-Mail Adresse")]
      
        public string Email
        {
            get { return customerInfo.Email; }
            set { customerInfo.Email = value;
                SetValue(() => Email, value); 
            }
        }

        public string AnsprechpersonVorname
        {
            get { return customerInfo.Contactperson_Firstname; }
            set { customerInfo.Contactperson_Firstname = value; }
        }
        public string AnsprechpersonNachname
        {
            get { return customerInfo.Contactperson_Lastname; }
            set { customerInfo.Contactperson_Lastname = value; }
        }
        public string AnsprechpersonTitle
        {
            get { return customerInfo.Contactperson_Title; }
            set { customerInfo.Contactperson_Title = value; }
        }
        public DateTime Änderungsdatum
        {
            get { return customerInfo.Änderungsdatum; }
            set { customerInfo.Änderungsdatum = value; }
        }
        public Status In_Bearbeitung
        {
            get { return customerInfo.In_bearbeitung; }
            set { customerInfo.In_bearbeitung = value; }
        }
        public Status Unterlagen_gesendet
        {
            get { return customerInfo.Unterlagen_gesendet; }
            set { customerInfo.Unterlagen_gesendet = value; }
        }

        public string UserVorname
        {
            get { return customerInfo.User.Firstname; }
            set { customerInfo.User.Firstname = value; }
        }

        public string UserNachname
        {
            get { return customerInfo.User.Lastname; }
            set { customerInfo.Contactperson_Lastname = value; }
        }

        public Status Angebot_geschickt
        {
            get { return customerInfo.Angebot_geschickt; }
            set { customerInfo.Angebot_geschickt = value; }

        }

        public Status Interesse_Kooperationsvertrag
        {
            get { return customerInfo.Interesse_kooperationsvertrag; }
            set { customerInfo.Interesse_kooperationsvertrag = value; }
        }

        public Status Abgeschlossen
        {
            get { return customerInfo.Abgeschlossen; }
            set { customerInfo.Abgeschlossen = value; }
        }

        public string Notizen
        {
            get { return customerInfo.Notizen; }
            set { customerInfo.Notizen = value; }
        }

        public int Angebotsnummer
        {
            get { return CustomerInfo.Angebotsnummer; }
            set { CustomerInfo.Angebotsnummer = value; }
        }

        public string Abklärung
        {
            get { return CustomerInfo.Abklärung; }
            set { CustomerInfo.Abklärung = value; }
        }

       
        public Customer CustomerInfo { get => customerInfo; set => customerInfo = value; }

        public User User { get => customerInfo.User; set => customerInfo.User = value; }
       

        public RelayCommand SaveDataCommand { get; set; }

        public int InsertOrUpdateCustomerFromView()
        {
            int result = 0;
            using (var ctx = new ApplicationDbContext())
            {
                CustomerViewModel customerViewModel = this;
                Customer customerFromView = customerViewModel.CustomerInfo;
                Customer customer = ctx.Customers.Where(x => x.Id == customerFromView.Id).FirstOrDefault();
                if (customer == null)
                {
                    customer = customerFromView;

                }
                else foreach (PropertyInfo property in typeof(Customer).GetProperties())
                {
                    property.SetValue(customer, property.GetValue(customerFromView, null), null);
                }
                User user = ctx.Users.Where(x => x.Id == customerFromView.User.Id).FirstOrDefault();
                if (user == null)
                {
                    // Mark them as  added
                    customer.UserId = 1;
                    ctx.Users.AddOrUpdate(customer.User);
                }
                else
                {
                    // Update
                    customer.User = user;
                   
                    foreach (PropertyInfo property in typeof(User).GetProperties())
                    {
                        property.SetValue(customer.User, property.GetValue(customerFromView.User, null), null);
                    }
                }
                ctx.Customers.AddOrUpdate(customer);

                result = ctx.SaveChanges();
                return result;
            }
        }
        private async void ShowCustomerSavedDialog(object parameter)
        {
            await this.dialogCoordinator.ShowMessageAsync(this.DetailWindowDatacontext, "Kunde wurde gespeichert", "", MessageDialogStyle.Affirmative);
            var editWindow = parameter as Window;
            editWindow?.Close();
        }

        private async void ShowCustomerNotSavedDialog(string message)
        {
            await this.dialogCoordinator.ShowMessageAsync(this.DetailWindowDatacontext, "Kunde wurde nicht gespeichert", message, MessageDialogStyle.Affirmative);
        }


        public void SaveData(object parameter)
        {
            // Extra validation code not necessary here, because of existing form validation
            // string error = OnValidate("Email"); 
            // if (!string.IsNullOrEmpty(error)) Errors += 1;

            if (Errors == 0)
            {

                try
                {
                    int result = InsertOrUpdateCustomerFromView();
                    if (result > 0)
                    {

                        ShowCustomerSavedDialog(parameter);
                        ParentGrid.AddOrUpdateCustomer(this);


                    }
                    else ShowCustomerNotSavedDialog("Unbekannter Fehler");
                }
                catch (DbEntityValidationException e)
                {
                    string errorMessage = string.Empty;
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        errorMessage += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            errorMessage += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    ShowCustomerNotSavedDialog("\n\nGrund:" + errorMessage);
                }

            }
        }


        public static int Errors { get; set; }
        public DataGridViewModel ParentGrid { get; set; }
        public object DetailWindowDatacontext { get; set; }

        public IDisposable Subscribe(IObserver<Customer> observer)
        {
            return this.Subscribe(observer);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
