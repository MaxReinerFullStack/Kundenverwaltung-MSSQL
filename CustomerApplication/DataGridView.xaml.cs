using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;
using MahApps.Metro.Controls.Dialogs;

namespace CustomerApplication
{
    /// <summary>
    /// Interaktionslogik für DataGridView.xaml
    /// </summary>
    public partial class DataGridView : UserControl
    {
        DataGridViewModel vm = new DataGridViewModel(DialogCoordinator.Instance);

        public DataGridView()
        {
            InitializeComponent();
            DataContext = vm;

        }

        
        private void CustomerGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            Border border = VisualTreeHelper.GetChild(dg, 0) as Border;
            ScrollViewer scrollViewer = VisualTreeHelper.GetChild(border, 0) as ScrollViewer;
            Grid grid = VisualTreeHelper.GetChild(scrollViewer, 0) as Grid;
            Button button = VisualTreeHelper.GetChild(grid, 0) as Button;

            if (button != null && button.Command != null && button.Command == DataGrid.SelectAllCommand)
            {
                button.IsEnabled = false;
                button.Opacity = 0;
            }
            
        }

        private void btn_Click_UnselectAllRows_Customer(object sender, RoutedEventArgs e)
        {
          
            CustomerGrid.UnselectAllCells();
        }
        private void btn_Click_Delete_Customers(object sender, RoutedEventArgs e)
        {
            DataGridViewModel gridViewModel = (DataGridViewModel)this.DataContext;
            if (CustomerGrid.SelectedItems.Count > 0)
            {
                ObservableCollection<CustomerViewModel> customersToDelete = new ObservableCollection<CustomerViewModel>();
                foreach (CustomerViewModel customerToDelete in CustomerGrid.SelectedItems)
                {
                    customersToDelete.Add(customerToDelete);
                }
                int customersDeleted = gridViewModel.DeleteCustomers(customersToDelete);
                ShowMessageDialog("Kunden gelöscht", customersDeleted == 1 ? customersDeleted + " Datensatz wurde gelöscht": customersDeleted + " Datensätze wurden gelöscht", MessageDialogStyle.Affirmative);
            }
            else ShowMessageDialog("Kunden löschen", "Es müssen zuvor ein oder mehr Kunden ausgewählt werden", MessageDialogStyle.Affirmative);

           
        }

        private void btn_Click_Update_Customers(object sender, RoutedEventArgs e)
        {
            CustomerGrid.CommitEdit();
            CustomerGrid.UnselectAllCells();
            DataGridViewModel gridViewModel = (DataGridViewModel)CustomerGrid.DataContext;

            try
            {
                if (gridViewModel.CustomersInfo.Count > 0)
                {

                    gridViewModel.UpdateAllCustomers();
                    CustomerGrid.DataContext = new DataGridViewModel();
                    var customersUpdated = ((DataGridViewModel)CustomerGrid.DataContext).CustomersInfo.Count();
                    ShowMessageDialog("Kunden aktualisiert", customersUpdated == 1 ? customersUpdated + " Datensatz wurde aktualisiert" : customersUpdated + " Datensätze wurden aktualisiert", MessageDialogStyle.Affirmative);
                }

            }
            catch (System.InvalidOperationException ex)
            {
                ShowMessageDialog("Sorting ist während einer AddNew- oder EditItem-Transaktion nicht zulässig.", "Aktualisierung erforderlich.",  MessageDialogStyle.Affirmative);
            }
           
        }
        private void btn_Click_Edit_Customer(object sender, RoutedEventArgs e)
        {
            if (CustomerGrid.SelectedItems.Count == 1)
            {
                CustomerViewModel rowViewModel = CustomerGrid.SelectedItem as CustomerViewModel;
                CustomerDetailWindow detailWindow = new CustomerDetailWindow();
               
                detailWindow.DataContext = rowViewModel.Clone(); ;

                DataGridViewModel gridViewModel = (DataGridViewModel)this.DataContext;
                rowViewModel.ParentGrid = gridViewModel;
                rowViewModel.DetailWindowDatacontext = detailWindow.DataContext;
                CustomerGrid.UnselectAllCells();
                bool? result = detailWindow.ShowDialog();
            }
            else ShowMessageDialog("Kunde bearbeiten - Info", "Ein Datensatz muss zuvor selektiert werden", MessageDialogStyle.Affirmative);
        }

        private void btn_Click_Insert_New_Customer(object sender, RoutedEventArgs e)
        {
           
            CustomerDetailWindow detailWindow = new CustomerDetailWindow();
            CustomerViewModel rowViewModel = CustomerViewModel.getNewCustomerView();
            detailWindow.DataContext = rowViewModel;

            DataGridViewModel gridViewModel = (DataGridViewModel)this.DataContext;
            rowViewModel.ParentGrid = gridViewModel;
            rowViewModel.DetailWindowDatacontext = detailWindow.DataContext;
            bool? result = detailWindow.ShowDialog();
          
        }
        private async void ShowMessageDialog(string title, string message, MessageDialogStyle messageDialogStyle)
        {
            await DialogCoordinator.Instance.ShowMessageAsync(this.DataContext, title, message, messageDialogStyle);
         
        }
        private void CustomerGrid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            

           
        }
    }
}
