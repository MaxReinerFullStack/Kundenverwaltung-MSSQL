using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CustomerApplication
{
    /// <summary>
    /// Interaktionslogik für CustomerDetailWindow.xaml
    /// </summary>
    public partial class CustomerDetailWindow : MetroWindow
    {
        CustomerViewModel vm = new CustomerViewModel(DialogCoordinator.Instance);
        public CustomerDetailWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

       

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
  

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) CustomerViewModel.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) CustomerViewModel.Errors -= 1;
        }

    }
}
