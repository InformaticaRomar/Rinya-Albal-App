using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using CustomControls;
using System.Data.SqlClient;
using OfficeOpenXml;


namespace Rinya_Albal_App.Pedidos
{
    public partial class Pedidos_Tablet : System.Web.UI.Page
    {
        protected void OnPagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (lvPedidos.FindControl("DataPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            this.BindListView();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                string a = datepicker1.Value;
                this.BindListView();
            }
            else
            {
                string sql = "select REP_REPRESENTANTE, REP_NOMBRE from REPRESENTANTE where rep_empresa=1 and REP_ESTADO='A' and REP_NOMBRE_ABREV<>'COMISIONIS' and Rep_representante between 100 and 200";
                Expert data = new Expert();
                DataTable dt = data.Sql_Datatable(sql);
                comercial.DataSource = dt;
                comercial.DataTextField = "REP_NOMBRE";
                comercial.DataValueField = "REP_REPRESENTANTE";
                comercial.DataBind();
            }
        }
        private string sql()
        {

            string representante = comercial.SelectedValue;
            string F_desde = datepicker1.Value;
            string F_hasta = datepicker2.Value; //this.Page.Request.Form["datepicker2"];//.Text;
            //F_desde = Request.QueryString("datepicker1");
            string sql = @"select WCPC_CLIENTE_ERP,WCPC_SUC_RECEPTOR_MERC,WCPC_FECHAPED,WCPC_NUMERO_PEDIDO_ERP,WCPC_NUMPED,WCPC_REPRESENTANTE,WCPC_OBSERVACIONES
from PEDIDOS_PROCESADOS_TPV where WCPC_REPRESENTANTE = " + representante + " and WCPC_FECHAPED between to_date('" + F_desde + "') and to_date('" + F_hasta + "')";
            return sql;
        }
        private void BindListView()
        {
            Expert data = new Expert();
            DataTable dt = data.Sql_Datatable(sql());
            lvPedidos.DataSource = dt;
            lvPedidos.DataBind();
        }

        protected void Unnamed1_Load(object sender, EventArgs e)
        {

        }
        public void Get_Excel()
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                // date columns
                List<string> dateColumns = new List<string>() {

                "WCPC_FECHAPED",
                "Fecha Pedido"
            };

                // hide columns
                List<string> hideColumns = new List<string>() {

                "RecordID",
                "CategoryID"
            };
                Expert data = new Expert();
                DataTable table = data.Sql_Datatable(sql());
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);

                FormatWorksheetData(dateColumns, hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Pedidos_tablet.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
        }
        private static void FormatWorksheetData(List<string> dateColumns, List<string> hideColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            ExcelRange r;

            // which columns have dates in
            for (int i = 1; i <= columnCount; i++)
            {
                // if cell header value matches a date column
                if (dateColumns.Contains(ws.Cells[1, i].Value.ToString()))
                {
                    r = ws.Cells[2, i, rowCount + 1, i];
                    r.AutoFitColumns();
                    r.Style.Numberformat.Format = @"dd MMM yyyy hh:mm";
                }
            }
            // get all data and autofit
            r = ws.Cells[1, 1, rowCount + 1, columnCount];
            r.AutoFitColumns();

            // which columns have columns that should be hidden
            for (int i = 1; i <= columnCount; i++)
            {
                // if cell header value matches a hidden column
                if (hideColumns.Contains(ws.Cells[1, i].Value.ToString()))
                {
                    ws.Column(i).Hidden = true;
                }
            }
        }
        protected void Btexport_Click(object sender, EventArgs e)
        {
            Get_Excel();
        }
    }
}