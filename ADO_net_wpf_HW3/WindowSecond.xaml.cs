using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml;

namespace ADO_net_wpf_HW3
{
    /// <summary>
    /// Interaction logic for WindowSecond.xaml
    /// </summary>
    public partial class WindowSecond : Window
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        DataTable table;
        string connectionString = "Data Source=KOSMOSLT\\SQLEXPRESS;Initial Catalog=BagShopDB;Integrated Security=True;Connect Timeout=10";
        public WindowSecond()
        {
            InitializeComponent();
            AsyncGetHistory(); 
        }
        private void AsyncGetHistory()
        {
            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand("select Operation, CreateAt as [Date of Creation], Product.Name\r\nfrom History\r\njoin Product on Product.id=History.ProductId\r\norder by [Date of Creation] desc", conn);
            try
            {
                conn.Open();
                AsyncCallback callback = new AsyncCallback(GetDataCallback);
                cmd.BeginExecuteReader(callback, cmd);
                //MessageBox.Show("Executing in the new thread..."); 
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }
        private void GetDataCallback(IAsyncResult result)
        {
            SqlDataReader reader = null;
            try
            {
            SqlCommand command = result.AsyncState as SqlCommand;
                reader = command.EndExecuteReader(result); 
                table = new DataTable();
                table.Load(reader);
                showDataTable(); 
            }
            finally
            {
                if(reader.IsClosed) reader.Close();
            }
        }
        private void showDataTable()
        {
            App.Current.Dispatcher.BeginInvoke(new Action (() => XamlDataGridHistory.ItemsSource = table.DefaultView));
        }
    }
}
