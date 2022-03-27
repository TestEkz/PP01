using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PP01v2
{
    /// <summary>
    /// Логика взаимодействия для SalesProducts.xaml
    /// </summary>
    public partial class SalesProducts : Page
    {
        public SalesProducts(Product selProd)
        {
            InitializeComponent();
            var currentSale = Manager.GetContext().ProductSale.ToList();
            var comboSales = Manager.GetContext().Product.ToList();
            comboSales.Insert(0, new Product
            {
                Title = "Все товары"
            });
            ComboSale.ItemsSource = comboSales;
            if (selProd != null)
            {
                currentSale = currentSale.Where(p => p.ProductID == selProd.ID).ToList();
                ComboSale.SelectedIndex = selProd.ID;
            }
            else
            {
                ComboSale.SelectedIndex = 0;
            }
            DGridSales.ItemsSource = currentSale.OrderByDescending(p => p.SaleDate).ToList();
        }

        private void ComboSale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSale = Manager.GetContext().ProductSale.ToList();
            if (ComboSale.SelectedIndex == 0)
            {
                currentSale = currentSale.ToList();
            }
            else
            {
                currentSale = currentSale.Where(p => p.Product.Title == (ComboSale.SelectedItem as Product).Title).ToList();
            }
            DGridSales.ItemsSource = currentSale;

        }
    }
}
