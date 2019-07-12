using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ViewModel;

namespace CustomerApplication
{
    public class KundeÜberfälligConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var änderungsDatum = (DateTime)value;

                DateTime today = DateTime.Now;
             

                TimeSpan duration = today - änderungsDatum;

                //wenn Datensatz älter als 30 Tage, dann zeige andere Farbe an
                if (duration.Days >30)
                {
                    return true;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class KundeIstWirklichÜberfälligConverter : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var bindingGroup = value as BindingGroup;
            var kunde = bindingGroup?.Items.OfType<CustomerViewModel>().ElementAtOrDefault(0);
            var änderungsDatum = (DateTime)kunde.Änderungsdatum;

            DateTime today = DateTime.Now;


            TimeSpan duration = today - änderungsDatum;
            if (kunde != null && kunde.Abgeschlossen >= DataModel.Status.Fällig && duration.Days > 30)
            {
                return new ValidationResult(false, $"Der Kunde  {kunde.Kundenbezeichnung} ist überfällig!");
            }
            return ValidationResult.ValidResult;
        }
    }
}
