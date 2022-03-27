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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        public ProductPage()
        {
            InitializeComponent();
            var allManuf = Manager.GetContext().Manufacturer.ToList();
            allManuf.Insert(0, new Manufacturer
            {
                Name = "Все типы"
            });
            ComboManuf.ItemsSource = allManuf;
            ComboManuf.SelectedIndex = 0;
            Update();
        }
        public void Update()
        {
            var allProd = Manager.GetContext().Product.Count();
            var currentProd = Manager.GetContext().Product.ToList();
            if (ComboManuf.SelectedIndex > 0)
                currentProd = currentProd.Where(p => p.Manufacturer.Name.Contains((ComboManuf.SelectedItem as Manufacturer).Name)).ToList();

            currentProd = currentProd.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            LViewProd.ItemsSource = currentProd;

            switch (ComboSort.SelectedIndex)
            {
                case 0:
                    LViewProd.ItemsSource = currentProd.OrderBy(p => p.Title).ToList();
                    break;
                case 1:
                    LViewProd.ItemsSource = currentProd.OrderByDescending(p => p.Title).ToList();
                    break;
            }
            LabCount.Content = currentProd.Count() + " из " + allProd.ToString();
            Manager.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ComboManuf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void LViewProd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(LViewProd.SelectedItem as Product));
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible) 
            {
                Manager.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LViewProd.ItemsSource = Manager.GetContext().Product.ToList();
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void BtnSale_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new SalesProducts(LViewProd.SelectedItem as Product));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var removeProd = LViewProd.SelectedItems.Cast<Product>().ToList();
            if(MessageBox.Show("Вы точно хотите удалить?","Внимание!", MessageBoxButton.YesNo)==MessageBoxResult.Yes)
            {
                try
                {
                    Manager.GetContext().Product.RemoveRange(removeProd);
                    Manager.GetContext().SaveChanges();
                    MessageBox.Show("Удалено!");
                    Update();
                }
                catch
                {
                    MessageBox.Show("Ошибка при удалении!");
                }
            }
        }
    }
}
