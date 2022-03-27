using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Product _curProd = new Product();
        public string Fill;
        public bool active;
        public AddEditPage(Product selProd)
        {
            InitializeComponent();
            _curProd.IsActive = true;
            if (selProd != null)
            {
                _curProd = selProd;
            }
            DataContext = _curProd;
            ComboManufact.ItemsSource = Manager.GetContext().Manufacturer.ToList();
        }

        private void BtnImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG)|*.BMP;*.JPG;*.JPEG*.GIF;*.PNG|All files (*.*)|*.*";
            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    Img.Source = new BitmapImage(new Uri(openDialog.FileName));
                    Fill = openDialog.FileName.ToString();
                }
                catch
                {
                    MessageBox.Show("Ошибка открытия");
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            active = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            active = false;
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_curProd.Title))
                error.AppendLine("Введите название!");
            if (_curProd.Cost <= 0)
                error.AppendLine("Введите корректную цену!");
            if (string.IsNullOrWhiteSpace(_curProd.Description))
                error.AppendLine("Введите описание!");
            if (_curProd.Manufacturer == null)
                error.AppendLine("Выберите производителя!");
            if(error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }
            if(_curProd.ID == 0)
            {
                _curProd.MainImagePath = Fill;
                Manager.GetContext().Product.Add(_curProd);
            }
            try
            {
                _curProd.IsActive = active;
                _curProd.MainImagePath = Fill;
                Manager.GetContext().SaveChanges();
                MessageBox.Show("Готово");
                Manager.MainFrame.GoBack();
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
    }
}
