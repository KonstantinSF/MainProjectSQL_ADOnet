using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace ADO_net_wpf_HW3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        DataTable table;
        string connectionString = "Data Source=KOSMOSLT\\SQLEXPRESS;Initial Catalog=BagShopDB;Integrated Security=True;Connect Timeout=10";
        public static string productGlobal { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void showProductBtn_Checked(object sender, RoutedEventArgs e)
        {
            WindowChange.allProducts =this.ShowProductBtn; 
            GetAllProducts();
            AddNotification();
        }
        public void GetAllProducts()
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand("Select * from AllProductView order by Product", conn);
                    table = new DataTable();
                    rdr = cmd.ExecuteReader();
                    table.Load(rdr);
                    xamlDataGridTable.ItemsSource = table.DefaultView;

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private void showSalesBtn_Checked(object sender, RoutedEventArgs e)
        {
            GetAllSales();
            AddNotification();
        }
        public void GetAllSales()
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    cmd = new SqlCommand(
                        "Select SaleDate, Product.Name as [ProductName], Emploee.fio as [Emploee Name], NumOfProduct, Discount\r\nfrom Sale as s \r\n" +
                        "join Product on ProductId=S.ProductId \r\n" +
                        "join Emploee on Emploee.id=S.EmploeeID\r\n" +
                        "order by SaleDate desc", conn);
                    rdr = cmd.ExecuteReader();
                    table = new DataTable();
                    table.Load(rdr);
                    xamlDataGridTable.ItemsSource = table.DefaultView;
                    //var selectedItem = xamlDataGridTable.SelectedItem;
                    //if (selectedItem != null)
                    //{
                    //productGlobal = (string)((DataRowView)selectedItem).Row["Product"]; ; 
                    //}
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string search = searchBox.Text;
                    string query1 = $"select * from AllProductView where Product like '%{search}%'";
                    string query2 = $"Select SaleDate, Product.Name as [ProductName], Emploee.fio as [Emploee Name], NumOfProduct, Discount " +
                        $"from Sale as s " +
                        $"join Product on ProductId = S.ProductId" +
                        $" join Emploee on Emploee.id = S.EmploeeID " +
                        $"where Emploee.fio like '{search}%' " +
                        $"order by SaleDate desc";
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    if (ShowProductBtn.IsChecked == true)
                    {

                        cmd.CommandText = query1;
                    }
                    else if (ShowSalesBtn.IsChecked == true)
                    {
                        cmd.CommandText = query2;

                    }
                    else return;
                    table = new DataTable();
                    rdr = cmd.ExecuteReader();
                    table.Load(rdr);
                    xamlDataGridTable.ItemsSource = table.DefaultView;

                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }
        private void AddNotification()
        {
            if (ShowSalesBtn.IsChecked == true)
                NotificationTextBlock.Text = "<-- Enter the Employee Name";
            else if (ShowProductBtn.IsChecked == true) NotificationTextBlock.Text = "<-- Enter the name of Product";
        }

        private void buyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ShowSalesBtn.IsChecked == true)
            {
                MessageBox.Show("You can't buy selected Sale, please press the button 'Show bags' and continue", "Attantion", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
            var selectedItem = xamlDataGridTable.SelectedItem;
            if (selectedItem == null || ShowSalesBtn.IsChecked == true) return;
            else
            {
                var value = (string)((DataRowView)selectedItem).Row["Product"] + " " + (string)((DataRowView)selectedItem).Row["Manufacturer"];
                AddNewSale(); 
                MessageBox.Show($"You've bought a {value}");
            }
        }
        private void AddNewSale()
        {
            using (conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    var selectedItem = xamlDataGridTable.SelectedItem;
                    var search = (string)((DataRowView)selectedItem).Row["Product"];
                    string query = $"insert into Sale (ProductID) values ((select id from Product where name like '{search}'))";
                    cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    GetAllProducts(); 
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private void AsyncHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowSecond secondWindow = new WindowSecond();
           secondWindow.Owner = this;
            secondWindow.Show(); //=addition Window  or new Window_Second().ShowDialog()=ModalWindow;
            
        }

        private void addProduct_Click(object sender, RoutedEventArgs e)
        {
            WindowAddProduct windowAddProduct = new WindowAddProduct();
            windowAddProduct.Owner = this;
            windowAddProduct.Show();
            
        }

        private void ChangeProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ShowSalesBtn.IsChecked == true)
            {
                MessageBox.Show("You can't buy selected Sale, please press the button 'Show bags' and continue", "Attantion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var selectedItem = xamlDataGridTable.SelectedItem;
            if (selectedItem == null || ShowSalesBtn.IsChecked == true) return;
            else
            {
                WindowChange windowChange = new WindowChange();
                windowChange.Show();
                //var value = (string)((DataRowView)selectedItem).Row["Product"] + " " + (string)((DataRowView)selectedItem).Row["Manufacturer"];
                //ChangeProductQuery();
                //MessageBox.Show($"You've changed a {value}");
            }
        }

        private void xamlDataGridTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = xamlDataGridTable.SelectedItem;
            if (selectedItem != null)
            {
                productGlobal = (string)((DataRowView)selectedItem).Row["Product"];
            }
        }
        //private void ChangeProductQuery()
        //{
        //    using (conn= new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            var selectedItem = xamlDataGridTable.SelectedItem;
        //            var nameChangableProduct = (string)((DataRowView)selectedItem).Row["Product"];
        //            string query = $"update Product set quantity={newQuantity}, set price = {newPrice} where product.name='{nameChangableProduct}'";
        //            cmd = new SqlCommand(query, conn); 
        //            cmd.ExecuteNonQuery();
        //            GetAllProducts(); 
        //        }
        //        finally
        //        {
        //            if(conn.State==ConnectionState.Open)conn.Close(); 
        //        }
        //    }
        //}
    }
}
