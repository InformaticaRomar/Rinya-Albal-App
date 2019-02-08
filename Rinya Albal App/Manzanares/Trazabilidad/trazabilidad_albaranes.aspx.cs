using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using Utiles;
using System.Data;
using System.Diagnostics;

namespace Rinya_Albal_App.Manzanares
{
    public partial class trazabilidad_albaranes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //   GridView1.Visible = true;
            if (this.IsPostBack == true)
            {
                datos = Session["datos"] as DataTable;
            }
            if (!this.IsPostBack)
            {


                DataView source = new DataView(datos);
                GridView1.DataSource = source;
                GridView1.DataBind();
            }
        }
        public DataTable Sql_Datatable(string sql)
        {
            string Con_E = @"Data Source=(DESCRIPTION =    (ADDRESS_LIST =      (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.0.14)(PORT = 1521))    )    (CONNECT_DATA = (SID = dbgrinya))  );User Id=grinya_expert;Password=datadec;";
            DataTable dt = new DataTable("Lineas");

            using (OracleConnection cnn2 = new OracleConnection(Con_E))
            {
                try
                {
                    cnn2.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cnn2;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    dt.Load(dr);

                    cnn2.Dispose();
                    return dt;
                }
                catch (Exception)
                {
                    throw;

                }
            }

        }
        Expert con = new Expert();
        private DataTable datos { get; set; }
        private bool datos_albaran(string nalbaran)
        {
            bool resultado = false;
            string sql = "  SELECT DPC_NUMERO_ALBARAN as \"Albaran\", DPC_FECHA_ALBARAN as \"Fecha Albaran\",DPC_ARTICULO as \"Articulo\",PRO_DESCRIPCION_CORTA as \"Descripcion\",LOA_NUMERO_LOTE as \"Lote\", LOA_FECHA_CADUCIDAD as \"Fecha Caducidad\",LOA_CANTIDAD as \"Cantidad\",LOA_UNIDAD_MEDIDA as \"Unidad de Medida\"" + " , DPC_CANTIDAD as  \"Cantidad Pedido\",DPC_UMP as \"Unidad de Pedido\"" + " , LOA_CANTIDAD_UMP as  \"Cantidad albaran Pedido\",DPC_UMP as \"Unidad Pedido\"" + @"
FROM PR_PRODUCTO ,
LOTE_ARTICULO ,
CUERPO_PEDIDO_CLIENTE
WHERE DPC_EMPRESA = 3
AND LOA_EMPRESA = DPC_EMPRESA
AND LOA_CONTABILIDAD = DPC_CONTABILIDAD
AND LOA_CLIENTE = DPC_CLIENTE
AND LOA_NUMERO_PEDIDO = DPC_NUMERO_PEDIDO
AND LOA_NUMERO_BIS = DPC_NUMERO_BIS
AND LOA_NUMERO_ITEM = DPC_NUMERO_ITEM
AND DPC_NUMERO_ALBARAN like '" + nalbaran + @"'
AND PRO_EMPRESA = DPC_EMPRESA
AND PRO_CODIGO_PRODUCTO = DPC_ARTICULO
ORDER BY DPC_NUMERO_ALBARAN";

            datos = con.Sql_Datatable(sql);
            Session.Add("datos", datos);
            DataView source = new DataView(datos);
            if (datos != null)
                if (datos.Rows.Count > 0)
                { resultado = true; }
            GridView1.DataSource = source;
            GridView1.DataBind();
            GridView1.Visible = true;
            return resultado;
        }
        private bool datos_fechas(string desde, string hasta)
        {
            bool resultado = false;
            string sql = "  SELECT DPC_NUMERO_ALBARAN as \"Albaran\", DPC_FECHA_ALBARAN as \"Fecha Albaran\",DPC_ARTICULO as \"Articulo\",PRO_DESCRIPCION_CORTA as \"Descripcion\",LOA_NUMERO_LOTE as \"Lote\", LOA_FECHA_CADUCIDAD as \"Fecha Caducidad\",LOA_CANTIDAD as \"Cantidad\",LOA_UNIDAD_MEDIDA as \"Unidad de Medida\"" + " , DPC_CANTIDAD as  \"Cantidad Pedido\",DPC_UMP as \"Unidad de Pedido\"" + " , LOA_CANTIDAD_UMP as  \"Cantidad albaran Pedido\",DPC_UMP as \"Unidad Pedido\"" + @"
FROM PR_PRODUCTO ,
LOTE_ARTICULO ,
CUERPO_PEDIDO_CLIENTE
WHERE DPC_EMPRESA = 3
AND LOA_EMPRESA = DPC_EMPRESA
AND LOA_CONTABILIDAD = DPC_CONTABILIDAD
AND LOA_CLIENTE = DPC_CLIENTE
AND LOA_NUMERO_PEDIDO = DPC_NUMERO_PEDIDO
AND LOA_NUMERO_BIS = DPC_NUMERO_BIS
AND LOA_NUMERO_ITEM = DPC_NUMERO_ITEM
AND DPC_FECHA_ALBARAN BETWEEN TO_DATE('" + desde.Replace("-", "/") + @"') AND TO_DATE('" + hasta.Replace("-", "/") + @"')
AND PRO_EMPRESA = DPC_EMPRESA
AND PRO_CODIGO_PRODUCTO = DPC_ARTICULO
ORDER BY DPC_NUMERO_ALBARAN";

            datos = con.Sql_Datatable(sql);
            Session.Add("datos", datos);
            DataView source = new DataView(datos);
            if (datos != null)
                if (datos.Rows.Count > 0)
                { resultado = true; }
            GridView1.DataSource = source;
            GridView1.DataBind();
            GridView1.Visible = true;
            return resultado;
        }
        private void Buscar()
        {
            // this.Btexport.Enabled = false;
            GridView1.Visible = false;
            // ClientScriptManager CSM = Page.ClientScript;
            char[] charsToTrim = { '*', ' ' };
            string albaran = this.Nalbaran.Text.Trim(charsToTrim);
            if (albaran.Length > 0)
            {
                if (datos_albaran(albaran)) { this.Btexport.Enabled = true; this.Nalbaran.Text = ""; }
            }
            else
            {
                string desde = this.datepicker_1.Text.Trim(charsToTrim);
                string hasta = this.datepicker_2.Text.Trim(charsToTrim);
                if (datos_fechas(desde, hasta)) { this.Btexport.Enabled = true; }
            }
            //   if (datos_albaran(Lote)) { this.Btexport.Enabled = true; }
        }
        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        static void OpenMicrosoftExcel(string f)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "EXCEL.EXE";
            startInfo.Arguments = f;
            Process.Start(startInfo);
        }
        private void ExportToExcel2(DataTable dt)
        {
            string attachment = "attachment; filename=traza.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }
        private void ExportToExcel(DataTable dt)
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    string filename = "traza.xls";
                    System.IO.StringWriter tw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                    DataGrid dgGrid = new DataGrid();
                    dgGrid.DataSource = dt;
                    dgGrid.DataBind();

                    //Get the HTML for the control.
                    dgGrid.RenderControl(hw);
                    //Write the HTML back to the browser.
                    //Response.ContentType = application/vnd.ms-excel;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                    this.EnableViewState = false;
                    Response.Write(tw.ToString());
                    Response.End();
                }
        }
        protected void Btexport_Click(object sender, EventArgs e)
        {
            //string oFileName = @"C:\temp\traza.xls";

            try
            {

                if (datos != null)
                {
                    ExportToExcel(datos);

                }

            }
            catch (Exception )
            { }
        }
    }
}