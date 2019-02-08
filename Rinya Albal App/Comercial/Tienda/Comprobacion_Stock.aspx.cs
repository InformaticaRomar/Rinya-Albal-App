using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using OfficeOpenXml;

namespace Rinya_Albal_App.Comercial.Tienda
{
    public partial class Comprobacion_Stock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        DataTable Stock_Tienda()
        {
            string sql = @"select distinct   Articulo,Nombre,Cantidad,Importe,Valoracion
FROM OPENQUERY(UNICENTA, 'SELECT `stockcurrent`.`PRODUCT` Articulo, `products`.`NAME` Nombre,`stockcurrent`.`UNITS`Cantidad, round(`products`.`PRICEBUY`,2) Importe,round(`stockcurrent`.`UNITS` * `products`.`PRICEBUY`,2) as Valoracion  FROM `stockcurrent`,`products` where `stockcurrent`.`PRODUCT`=`products`.`ID`')
order by Articulo";
            Quality con = new Quality();

            return con.Sql_Datatable(sql);
        }

        DataTable Stock_Quality()
        {
            string sql = @"select [ALMACEN ARTICULOS].artículo as Articulo,ARTICULO.Descripción as Nombre, case when [ALMACEN ARTICULOS].[Existencias Kgs] =0 then round ([ALMACEN ARTICULOS].Existencias,3) else round ([ALMACEN ARTICULOS].[Existencias Kgs],3) end as Cantidad  
,Round([Precio Ult Compra],2) Importe
, Round (case when [ALMACEN ARTICULOS].[Existencias Kgs] =0 then round ([ALMACEN ARTICULOS].Existencias,3) else round ([ALMACEN ARTICULOS].[Existencias Kgs],3) end * [Precio Ult Compra],2) as Valoracion
from [ALMACEN ARTICULOS] inner join ARTICULO on ARTICULO.artículo=[ALMACEN ARTICULOS].artículo  where Almacén=5";
            Quality con = new Quality();

            return con.Sql_Datatable(sql);
        }
        class lista_datos
        {
            public string Articulo { get; set; }
            public string Nombre { get; set; }
            public string Cantidad { get; set; }
            public string Importe { get; set; }
            public string Valoracion { get; set; }
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (System.Reflection.PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            System.Reflection.PropertyInfo[] Props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (System.Reflection.PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        DataTable comparar_datos(DataTable tienda, DataTable quality)
        {
            List<lista_datos> tenda = ConvertDataTable<lista_datos>(tienda);
            List<lista_datos> quali = ConvertDataTable<lista_datos>(quality);
            List<lista_datos> listaPedidosClientes = (from c in tenda   // Lista de clientes
                                                      from p in quali     // Lista de pedidos
                                                      where p.Articulo == c.Articulo            // Filtro: ID del pedido == ID del cliente
                                                      && p.Cantidad != c.Cantidad
                                                      && Math.Round(Convert.ToDouble(c.Cantidad) - Convert.ToDouble(p.Cantidad.Replace('.', ',')), 3) != 0
                                                      select new lista_datos() { Articulo = p.Articulo, Nombre = p.Nombre, Cantidad = (Math.Round(Convert.ToDouble(c.Cantidad) - Convert.ToDouble(p.Cantidad.Replace('.', ',')), 2)).ToString(),Importe=p.Importe ,Valoracion=p.Valoracion}).ToList();


            DataTable datos = ToDataTable<lista_datos>(listaPedidosClientes);

            return datos;
        }
        protected void BtExportar_Click(object sender, EventArgs e)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("Tienda");
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Quality");
                
                DataTable Tienda = Stock_Tienda();
                DataTable Quality = Stock_Quality();
                Tienda.TableName = "Tienda";
                Quality.TableName = "Quality";
               
                ws1.Cells["A1"].LoadFromDataTable(Tienda, true, OfficeOpenXml.Table.TableStyles.Light16);
                ws2.Cells["A1"].LoadFromDataTable(Quality, true, OfficeOpenXml.Table.TableStyles.Light17);
                if (Tienda.Rows.Count > 0 && Quality.Rows.Count > 0)
                {
                    ExcelWorksheet ws3 = pck.Workbook.Worksheets.Add("Descuadre");
                    DataTable Comparar = comparar_datos(Tienda, Quality);
                    Comparar.TableName = "Descuadre";
                    ws3.Cells["A1"].LoadFromDataTable(Comparar, true, OfficeOpenXml.Table.TableStyles.Light17);


                }
                    // make sure it is sent as a XLSX file
                    Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Control_Stock_Tienda.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            }
    }
}