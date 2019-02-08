using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data.SqlClient;

namespace Rinya_Albal_App.Manzanares.Finanzas
{
    public partial class Confirming : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           /* if (this.IsPostBack)
            {
                if (Session["Sql_v"]!= null)
                Sql_.Text = Session["Sql_v"].ToString();
            }else { Sql_.Text = "prueba"; }*/

        }

        private bool sql_insert(string sql)
        {

          // try
          //  {
                //string Con_Q = @"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso";
                // Properties.Settings.Default.Conex_Quality;
                string Con_Q= System.Web.Configuration.WebConfigurationManager.ConnectionStrings["QC600ConnectionString"].ConnectionString;
            using (SqlConnection cnn = new SqlConnection(Con_Q))
                {
                    SqlCommand command = new SqlCommand(sql, cnn);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();


                }
          /*  }
            catch (Exception)
            {
                return false;
            }*/

            return true;
        }
        private void insertar_confirming(string fecha_emision,string entidad, string fecha_pago, string Proveedor, string n_factura, string Empresa, string nominal,string pagado, string importe)
        {

            string sql = @"INSERT INTO  QC_PRUEBAS.dbo.CONFIRMING ([FECHA_EMISION],[ENTIDAD],[FECHA_PAGO],[PROVEEDOR],[N_FACTURA],[EMPRESA],[IMPORTE],[FECHA_VENCIMIENTO],[MES_VENCIMIENTO],[NOMINAL],[PAGADO]) VALUES (CONVERT (DATETIME,'" + fecha_emision.Trim().Replace((char)13, ' ').Replace((char)10, ' ') + "',103), '"+ entidad.Trim().Replace((char)13, ' ').Replace((char)10, ' ') + @"',CONVERT (DATETIME,'" + fecha_pago.Trim().Replace((char)13, ' ').Replace((char)10, ' ') + @"',103),'"
+ Proveedor.Trim() + @"', '"+ n_factura.Trim() + @"','"+ Empresa.Trim() + @"',"+ importe
+@",dateadd(day,(180),CONVERT (DATETIME,'" + fecha_pago.Trim().Replace((char)13, ' ').Replace((char)10, ' ') + @"',103)),datename(month,CONVERT (DATETIME,'" + fecha_pago.Trim().Replace((char)13, ' ').Replace((char)10, ' ') + @"',103)),"+ nominal.Trim().Replace((char)13, ' ').Replace((char)10, ' ') + ","+pagado.Trim().Replace((char)13, ' ').Replace((char)10, ' ') + @")" ;
         //   Session["Sql_v"] = sql;
           // Sql_.Text = sql;
            
            /*      sql = @"INSERT INTO CONFIRMING ([FECHA_EMISION]
            ,[ENTIDAD]
            ,[FECHA_PAGO]
            ,[PROVEEDOR]
            ,[N_FACTURA]
            ,[EMPRESA]
            ,[IMPORTE]
            ,[FECHA_VENCIMIENTO]
            ,[MES_VENCIMIENTO]
            ,[NOMINAL]
            ,[PAGADO]) VALUES (CONVERT (DATETIME,'01/12/2017',103), 'Sabadell',CONVERT (DATETIME,'02/12/2017',103),'123', 'prueba_di','AQM',2000,dateadd(day,(180),CONVERT (DATETIME,'02/12/2017',103)),datename(month,CONVERT (DATETIME,'02/12/2017',103)),0,3)";*/
             sql_insert(sql);


        }
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string fecha_emision = txtfecha_emision.Text.ToString();
            string entidad = Request.Form["ctl00$MainContent$txtEntidad_"].ToString();
          //  ; // txtEntidad_.Value;
            string fecha_pago = txtfecha_pago.Text.ToString();
            string Proveedor = txtProveedor.Value.ToString();
            string n_factura = txtFactura.Value.ToString();
            string Empresa = Request.Form["ctl00$MainContent$txtEmpresa"].ToString();// txtEmpresa.SelectedIndex.ToString();
            string Nominal = "0";// txtNominal.Value.ToString();
           string pagado= txtPagado.Value.ToString();
            string importe = txtImporte.Value.ToString();
            if (Nominal.Length == 0)
                Nominal = "0";
            if (pagado.Length == 0)
                pagado = "0";
            importe = importe.Replace(',', '.');
            Nominal= Nominal.Replace(',', '.');
            pagado= pagado.Replace(',', '.');
            insertar_confirming(fecha_emision, entidad, fecha_pago, Proveedor, n_factura, Empresa, Nominal, pagado, importe);
            Response.Redirect(Request.Url.PathAndQuery, true);
           
        }
    }
}