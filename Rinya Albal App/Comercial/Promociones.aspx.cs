using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using OfficeOpenXml;

namespace Rinya_Albal_App.Comercial
{
    public partial class Promociones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
       
        Expert con = new Expert();
        private bool datos_promocion(string _Articulo, string _Tarifa, string _Cliente)
        {
            Respuesta_1_lbl.Text = "Procesando...";
            Respuesta_1_lbl.Visible = true;
            Respuesta_2_lbl.Visible = true;
            string sql = "select * from V_CLIENTES_ARTICULOS_PROMO_ where ARTICULO=175 and TARIFA=10";
            if (_Articulo.Length == 0 && _Tarifa.Length == 0 && _Cliente.Length == 0)
            {
                sql = "select * from V_CLIENTES_ARTICULOS_PROMO_ ";
            }
            else
            {
                sql = "select * from V_CLIENTES_ARTICULOS_PROMO_  where ";
            }

            if (_Articulo.Length > 0 && _Tarifa.Length == 0 && _Cliente.Length == 0)
            {
                sql += @"ARTICULO='" + _Articulo + @"'";
            }
            else if (_Articulo.Length > 0 && (_Tarifa.Length > 0 || _Cliente.Length == 0))
            {
                sql += @"ARTICULO='" + _Articulo + @"' AND ";
            }
            if (_Tarifa.Length > 0 && _Cliente.Length == 0)
            {
                sql += " TARIFA=" + _Tarifa;
            }
            else if (_Tarifa.Length > 0 && _Cliente.Length > 0)
            {
                sql += " TARIFA=" + _Tarifa + " AND ";
            }

            if (_Cliente.Length > 0)
            {
                sql += " CLIENTE=" + _Cliente;
            }


            // datos = Session["datos"] as DataTable;
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Promociones");
                ws.Cells["A1"].LoadFromDataTable(con.Sql_Datatable(sql), true, OfficeOpenXml.Table.TableStyles.Medium14);
                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Tarifas.xlsx");

                byte[] a = pck.GetAsByteArray();
                Response.OutputStream.Write(a, 0, a.Length);

                Response.OutputStream.Flush();
                Response.OutputStream.Close();

            }
            Respuesta_1_lbl.Text = "Excel exportado correctamente";
            Respuesta_2_lbl.Visible = false;

            return true;
        }
        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            datos_promocion(ART_textBox.Text, tarifa_textBox.Text, CL_textBox.Text);
        }
    }
}