using DataModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace CustomerApplication
{
    /// <summary>
    /// Interaktionslogik für CustomComboBox.xaml
    /// </summary>
    public partial class CustomComboBox : UserControl
    {
        public CustomComboBox()
        {
            
            InitializeComponent();
      
        }
        public string StatusValue
        {
            get { return (string)GetValue(StatusValueProperty); }
            set
            {
                SetValue(StatusValueProperty, value);
               
                
            }
        }
        public string StatusName
        {
            get { return (string)GetValue(StatusNameProperty); }
            set
            {
                SetValue(StatusNameProperty, value);
               

            }
        }

        public static readonly DependencyProperty StatusValueProperty
    = DependencyProperty.Register(
          "StatusValue",
          typeof(string),
          typeof(CustomComboBox),
           new UIPropertyMetadata(null));

        public static readonly DependencyProperty StatusNameProperty
  = DependencyProperty.Register(
        "StatusName",
        typeof(string),
        typeof(CustomComboBox),
         new UIPropertyMetadata(null));

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update DataContext
            ComboBox comboBox = (ComboBox)sender;

            CustomerViewModel customerViewModel = (CustomerViewModel)this.DataContext;

            Type _type = customerViewModel.GetType();

            // verify if property name exists in model
            customerViewModel.VerifyPropertyName(StatusName);

            // use reflection to update selected value in data context
            PropertyInfo _propertyInfo = _type.GetProperty(StatusName.ToString());
            EnumItem item = (EnumItem)combo.SelectedItem;
            Status MyStatus = (Status)Enum.Parse(typeof(Status), item.Value.ToString(), true);
            _propertyInfo.SetValue(customerViewModel, MyStatus);


            

        }
      

        private void combo_Loaded(object sender, RoutedEventArgs e)
        {
            
            ComboBox comboBox = (ComboBox)sender;

            CustomerViewModel customerViewModel = (CustomerViewModel)this.DataContext;

            Type _type = customerViewModel.GetType();

            PropertyInfo _propertyInfo = _type.GetProperty(StatusName.ToString());
            Status MyStatus = (Status)Enum.Parse(typeof(Status), StatusValue.ToString(), true);
            _propertyInfo.SetValue(customerViewModel, MyStatus);


            customerViewModel.VerifyPropertyName(StatusName);
            
            // Set initial Enum values to given Status Name and Value

            for (int index = 0; index < combo.Items.Count; index++)
            {
                EnumItem item = (EnumItem)combo.Items.GetItemAt(index);
                if (item.Value.ToString() == MyStatus.ToString())
                {
                    combo.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
