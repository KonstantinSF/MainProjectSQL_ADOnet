using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace ADO_net_wpf_HW3
{
    /// <summary>
    /// Interaction logic for WindowAddProduct.xaml
    /// </summary>

    public partial class WindowAddProduct : Window
    {

        public WindowAddProduct()
        {
            InitializeComponent();
        }

        private void OkAddProduct_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source = KOSMOSLT\\SQLEXPRESS; Initial Catalog = BagShopDB; Integrated Security = True; Connect Timeout = 10"))
            {
                try
                {
                    conn.Open();
                    string name = productName.Text;
                    decimal price = Convert.ToDecimal(productPrice.Text);
                    int quantity = Convert.ToInt32(productQuantity.Text);
                    int volume = Convert.ToInt32(productVolume.Text);
#if false //Create procedure for extract ID by Name
                    string proc1 = @"create proc GetIDbyName

    @manufacturerName nvarchar(50),
    @typeName nvarchar(50), 
    @genderName nvarchar(50),
    @materialName nvarchar(50), 
    @hangtypeName nvarchar(50),
    @manufacturerId int output,
    @typeId int output,
    @genderId int output, 
    @materialId int output, 
    @hangtypeId int output
    as
    set @manufacturerId = (select m.id from Manufacturer as m where m.name=@manufacturerName)
    set @typeId = (select t.id from Type as t where t.name = @typeName)
    set @genderId = (select g.id from Gender as g where g.name = @genderName)
    set @materialId = (select m.id from Material as m where m.name = @manufacturerName)
    set @hangtypeId = (select h.id from HangType as h where h.name = @hangtypeName)
    go"; 
#endif
                    SqlCommand cmdGetID = new SqlCommand("GetIDbyName", conn);
                    cmdGetID.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter manufacturerNameParam = new SqlParameter("@manufacturerName", manufacturerName.Text);
                    cmdGetID.Parameters.Add(manufacturerNameParam);
                    SqlParameter typeNameParam = new SqlParameter("@typeName", typeName.Text);
                    cmdGetID.Parameters.Add(typeNameParam);
                    SqlParameter genderNameParam = new SqlParameter("@genderName", genderName.Text);
                    cmdGetID.Parameters.Add(genderNameParam);
                    SqlParameter materialNameParam = new SqlParameter("@materialName", materialName.Text);
                    cmdGetID.Parameters.Add(materialNameParam);
                    SqlParameter hangTypeNameParam = new SqlParameter("@hangtypeName", hangTypeName.Text);
                    cmdGetID.Parameters.Add(hangTypeNameParam);

                    SqlParameter manufacturerIdParam = new SqlParameter
                    {
                        ParameterName = "@manufacturerId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    cmdGetID.Parameters.Add(manufacturerIdParam);
                    SqlParameter typeIdParam = new SqlParameter
                    {
                        ParameterName = "@typeId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    cmdGetID.Parameters.Add(typeIdParam);
                    SqlParameter genderIdParam = new SqlParameter
                    {
                        ParameterName = "@genderId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    cmdGetID.Parameters.Add(genderIdParam);
                    SqlParameter materialIdParam = new SqlParameter
                    {
                        ParameterName = "@materialId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    cmdGetID.Parameters.Add(materialIdParam);
                    SqlParameter hangTypeIdParam = new SqlParameter
                    {
                        ParameterName = "@hangTypeId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    cmdGetID.Parameters.Add(hangTypeIdParam);

                    cmdGetID.ExecuteNonQuery();

                    SqlCommand cmdAddProduct = new SqlCommand("AddProduct", conn);
                    cmdAddProduct.CommandType = System.Data.CommandType.StoredProcedure;
                    //*********Param from PROCEDURE AddProduct***********
#if false
//@name nvarchar(50),
//@price money,
//@quantity int,
//@volume int,
//@manufacturerId int,
//@typeID int,
//@genderID int,
//@materialID int,
//@hangTypeID int,
//@id int output

#endif
                    SqlParameter manufacturerIdParamAdd = new SqlParameter("@manufacturerId", manufacturerIdParam.Value);
                    cmdAddProduct.Parameters.Add(manufacturerIdParamAdd);
                    SqlParameter typeIdParamAdd = new SqlParameter("@typeId", typeIdParam.Value);
                    cmdAddProduct.Parameters.Add(typeIdParamAdd);
                    SqlParameter genderIdParamAdd = new SqlParameter("@genderId", genderIdParam.Value);
                    cmdAddProduct.Parameters.Add(genderIdParamAdd);
                    SqlParameter materialIdParamAdd = new SqlParameter("@materialId", materialIdParam.Value);
                    cmdAddProduct.Parameters.Add(materialIdParamAdd);
                    SqlParameter hangtypeIdParamAdd = new SqlParameter("@hangtypeId", hangTypeIdParam.Value);
                    cmdAddProduct.Parameters.Add(hangtypeIdParamAdd);
                    ////////////////////////////////////////////////////////////////
                    SqlParameter nameParamAdd = new SqlParameter("@name", name);
                    cmdAddProduct.Parameters.Add(nameParamAdd);
                    SqlParameter priceParamAdd = new SqlParameter("@price", price);
                    cmdAddProduct.Parameters.Add(priceParamAdd);
                    SqlParameter quantityParamAdd = new SqlParameter("@quantity", quantity);
                    cmdAddProduct.Parameters.Add(quantityParamAdd);
                    SqlParameter volumeParamAdd = new SqlParameter("@volume", volume);
                    cmdAddProduct.Parameters.Add(volumeParamAdd);
                    SqlParameter idParamAdd = new SqlParameter
                    {
                        ParameterName = "id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    cmdAddProduct.Parameters.Add(idParamAdd);

                    cmdAddProduct.ExecuteNonQuery();
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            //Close(); 
            }
        }

        private void CancelAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void productName_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled(); 
        }

        private void checkAllFormFilled()
        {
            if (
                productName.Text != "" &&
                manufacturerName.Text !="" &&
                productPrice.Text != "" &&
                productQuantity.Text != "" &&
                productVolume.Text != "" &&
                typeName.Text!=""&&
                genderName.Text!=""&&
                materialName.Text!=""&&
                hangTypeName.Text!=""&&
                productVolume.Text!=""
                ) OkAddProduct.IsEnabled = true;
            else OkAddProduct.IsEnabled = false;
        }
        private void manufacturerName_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled();
        }

        private void productPrice_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled();
        }

        private void productQuantity_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled();
        }

        private void typeName_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled();
        }

        private void genderName_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled();
        }

        private void materialName_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled();
        }

        private void hangTypeName_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled();
        }

        private void productVolume_textChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            checkAllFormFilled();
        }
    }
}
