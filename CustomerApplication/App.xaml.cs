using DataModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using ViewModel;

namespace CustomerApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
           
            base.OnStartup(e);
            using (var ctx = new ApplicationDbContext())
            {
                // access the data 
                DataGridViewModel.DatabaseIsInitialized = true;
               

            }
        }
    }
}
