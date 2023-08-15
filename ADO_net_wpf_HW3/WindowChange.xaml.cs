using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace ADO_net_wpf_HW3
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WindowChange : Window
    {
        public static RadioButton allProducts; 
        public WindowChange()
        {
            InitializeComponent();
        }

        private void newQuantity_textChanged(object sender, TextChangedEventArgs e)
        {
            CheckAllFormFilled(); 
        }

        private void newPrice_textChanged(object sender, TextChangedEventArgs e)
        {
            CheckAllFormFilled(); 
        }
        private void CheckAllFormFilled()
        {
            if (newQuantity.Text !="" && newPrice.Text!= "")
            OkChangeProduct.IsEnabled = true;
            else OkChangeProduct.IsEnabled = false;
        }
         private void OkChangeProduct_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source = KOSMOSLT\\SQLEXPRESS; Initial Catalog = BagShopDB; Integrated Security = True; Connect Timeout = 10"))
            {
                try
                {
                    conn.Open();
                    int quantity = Convert.ToInt32(newQuantity.Text); 
                    decimal price = Convert.ToDecimal(newPrice.Text);
                    string selectedInMainWindow = MainWindow.productGlobal; 

                    SqlCommand cmd = new SqlCommand($"update Product\r\nset quantity={quantity}, \r\n\tprice={price}\r\nwhere Product.name='{selectedInMainWindow}'", conn); 
                    cmd.ExecuteNonQuery();
                    allProducts.IsChecked = false;
                    allProducts.IsChecked=true;
                    Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message); 
                }
                finally
                {
                    if(conn.State==System.Data.ConnectionState.Open) conn.Close();
                }
            }
        }

        private void CancelChangeProduct_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

    }
}
