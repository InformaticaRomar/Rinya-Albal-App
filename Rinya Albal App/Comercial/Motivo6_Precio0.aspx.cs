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


namespace Rinya_Albal_App.Comercial
{
    public partial class Motivo6_Precio0 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.ClearSearchFilters();
                FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, (string)GridView1.CurrentSortDirection, 1);
            }
        }
        private void Buscar()
        {

            GridView1.Visible = false;

            DataTable dat = new DataTable();
            if (datepicker1.Text.Length > 0 && datepicker1.Text.Length > 0)
            {
                GridView1.ClearSearchFilters();
                FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, (string)GridView1.CurrentSortDirection, 1);
                //FilterToNthPage(dat,)
                /*  GridView1.DataSource = Fabricacion_stock (DropDownDesde.SelectedValue, DropDownHasta.SelectedValue);
                  GridView1.DataBind();*/
                GridView1.Visible = true;

            }

        }
        private void FilterToNthPage(DataTable SearchFilterValues, string SortExpression, string SortDirection, int PageIndex)
        {
            int PageSize = GridView1.PageSize;

            try
            {
                DataTable dt = GetSearchFilteredData("SearchForPagedData", "@SearchFilter", SearchFilterValues, consulta_sql(datepicker1.Text, datepicker2.Text), SortExpression, SortDirection, PageIndex, PageSize);
                GridView1.CurrentSearchPageNo = PageIndex;
                if (dt.Rows.Count > 0)
                {
                    GridView1.TotalSearchRecords = (int)dt.Rows[0]["TotalRows"];
                }
                else
                {
                    GridView1.TotalSearchRecords = 0;
                }
                GridView1.DataSource = dt;

                GridView1.DataBind();
            }
            catch (System.Exception)
            {
                //  strMsg = ex.Message;
            }
            finally
            {
                // lblMsg.Text = strMsg;
            }
        }
        public DataTable GetSearchFilteredData(string StoreProcedureName, string SQLTableVariableName, DataTable SearchFilterValues, string Select, string SortExpression, string SortDirection, int PageIndex, int PageSize)
        {

            // Assumes connection is an open SqlConnection object.
            SqlConnection conn = new SqlConnection(@"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso");
            using (conn)
            {
                //Open the connection
                conn.Open();

                // Configure the SqlCommand and SqlParameter.
                SqlCommand cmd = new SqlCommand("SearchForPagedData", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                //creating parameter and assign values
                //Create SQL Table parameter and assign datatable to it @SqlSelect
                SqlParameter tvpParam = cmd.Parameters.AddWithValue(SQLTableVariableName, SearchFilterValues);
                tvpParam.SqlDbType = SqlDbType.Structured;

                SqlParameter stpPageIndex = cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
                stpPageIndex.SqlDbType = SqlDbType.Int;

                SqlParameter stpPageSize = cmd.Parameters.AddWithValue("@PageSize", PageSize);
                stpPageSize.SqlDbType = SqlDbType.Int;

                SqlParameter stpSortExpression = cmd.Parameters.AddWithValue("@SortExpression", SortExpression);
                stpSortExpression.SqlDbType = SqlDbType.VarChar;

                SqlParameter stpSortDirection = cmd.Parameters.AddWithValue("@SortDirection", SortDirection);
                stpSortDirection.SqlDbType = SqlDbType.VarChar;

                SqlParameter stpselect = cmd.Parameters.AddWithValue("@SqlSelect", Select);
                stpselect.SqlDbType = SqlDbType.VarChar;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                //Close connection
                conn.Close();
                return ds.Tables[0];

            }
        }
        private string consulta_sql(string fecha1, string fecha2)
        {

            string sql = @"SELECT DPC_EMPRESA,DPC_CLIENTE, CPM_SUCURSAL_ENVIO,DPC_NUMERO_ALBARAN, DPC_NUMERO_PEDIDO, CUERPO_PEDIDO_CLIENTE.DPC_NUMERO_ITEM,CUERPO_PEDIDO_CLIENTE.DPC_ARTICULO,CUERPO_PEDIDO_CLIENTE.DPC_FECHA_ALBARAN,
CABECERA_PEDIDO_CLIENTE.CPM_F_PAGO,dpc_codigo_incidencia
  FROM TEXTO_PEDIDO_CLIENTE    TC,
       TEXTO_PEDIDO_CLIENTE    TD,
       REFERENCIA_CRUZADA_CLTE,
       DATOS_RECARGO_LINEA_PED,
       CABECERA_PEDIDO_CLIENTE,
       CUERPO_PEDIDO_CLIENTE
 WHERE CPM_EMPRESA            = DPC_EMPRESA         AND
       CPM_CONTABILIDAD       = DPC_CONTABILIDAD    AND
       CPM_CLIENTE            = DPC_CLIENTE         AND
       CPM_NUMERO_PEDIDO      = DPC_NUMERO_PEDIDO   AND
       DRL_EMPRESA         (+)= DPC_EMPRESA         AND
       DRL_CONTABILIDAD    (+)= DPC_CONTABILIDAD    AND
       DRL_CLIENTE         (+)= DPC_CLIENTE         AND
       DRL_NUMERO_PEDIDO   (+)= DPC_NUMERO_PEDIDO   AND
       DRL_NUMERO_BIS      (+)= DPC_NUMERO_BIS      AND
       DRL_NUMERO_ITEM     (+)= DPC_NUMERO_ITEM     AND
       RCC_EMPRESA         (+)= DPC_EMPRESA         AND
       RCC_CLIENTE         (+)= DPC_CLIENTE         AND
       RCC_ARTICULO        (+)= DPC_ARTICULO        AND
       TD.TPC_EMPRESA      (+)= DPC_EMPRESA         AND
       TD.TPC_NUMERO_TEXTO (+)= DPC_NUMERO_TEXTO    AND
       TC.TPC_EMPRESA      (+)= CPM_EMPRESA         AND
       TC.TPC_NUMERO_TEXTO (+)= CPM_NUMERO_TEXTO and
       dpc_empresa in (1,2) 
      and( DPC_PRECIO_ARTICULO=0  Or dpc_codigo_incidencia=8 )and Dpc_fecha_albaran between to_date('" + fecha1 + "') and to_date('" + fecha2 + "')";


            return sql;
        }
        private bool Get_Excel()
        {
            Expert con = new Expert();
            List<string> dateColumns = new List<string>() {

                    "WCPC_FECHAPED",
                    "Fecha Pedido"
                };
            //string sql = consulta_sql(string agencia);
            List<string> hideColumns = new List<string>() {

                "orden"
            };
            using (ExcelPackage pck = new ExcelPackage())
            {


                DataTable table = con.Sql_Datatable(consulta_sql(datepicker1.Text, datepicker2.Text));
                if (table == null )//&& table.Rows.Count <= 0)
                {
                    System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('No hay precio 0 motivo 6')</SCRIPT>");
                }
                else
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                    ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                    FormatWorksheetData(dateColumns, hideColumns, table, ws);

                    // make sure it is sent as a XLSX file
                    Response.ContentType = "application/vnd.ms-excel";
                    // make sure it is downloaded rather than viewed in the browser window
                    Response.AddHeader("Content-disposition", "attachment; filename=integracion_agencia.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
            }

            return true;
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
        protected void GridView1_FilterButtonClick(object sender, SearchGridEventArgs e)
        {
            FilterToNthPage(e.SearchFilterValues, GridView1.CurrentSortExpression, GridView1.CurrentSortDirection, 1);
        }

        protected void GridView1_NavigationButtonClick(object sender, NavigationButtonEventArgs e)
        {
            if (e.NavigationButtonsType == NavigationButtonsTypes.GoFirst)
            {
                FilterToNthPage(GridView1.SearchFilters,
                    GridView1.CurrentSortExpression,
                    GridView1.CurrentSortDirection, 1);
            }
            else if (e.NavigationButtonsType == NavigationButtonsTypes.GoLast)
            {
                FilterToNthPage(GridView1.SearchFilters,
                    GridView1.CurrentSortExpression,
                    GridView1.CurrentSortDirection,
                    GridView1.TotalSearchPages);
            }
            else if (e.NavigationButtonsType == NavigationButtonsTypes.GoNext)
            {
                if (GridView1.CurrentSearchPageNo < GridView1.TotalSearchPages)
                {
                    FilterToNthPage(GridView1.SearchFilters,
                        GridView1.CurrentSortExpression,
                        GridView1.CurrentSortDirection,
                        (int)GridView1.CurrentSearchPageNo + 1);
                }
            }
            else if (e.NavigationButtonsType == NavigationButtonsTypes.GoPrevious)
            {
                if (GridView1.CurrentSearchPageNo > 1)
                {
                    FilterToNthPage(GridView1.SearchFilters,
                        GridView1.CurrentSortExpression,
                        GridView1.CurrentSortDirection,
                        (int)GridView1.CurrentSearchPageNo - 1);
                }
            }
            else if (e.NavigationButtonsType == NavigationButtonsTypes.GoToPage)
            {
                FilterToNthPage(GridView1.SearchFilters,
                    GridView1.CurrentSortExpression,
                    GridView1.CurrentSortDirection,
                    (int)e.PageIndex);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, GridView1.CurrentSortDirection, (int)GridView1.CurrentSearchPageNo);
        }

        protected void GridView1_CancelFilterButtonClick(object sender, SearchGridEventArgs e)
        {
            FilterToNthPage(e.SearchFilterValues, GridView1.CurrentSortExpression, GridView1.CurrentSortDirection, 1);
        }

        protected void GridView1_ExcelButtonClick(object sender, SearchGridEventArgs e)
        {

            string filename = "Export.xlsx";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            Expert data = new Expert();


            DataTable dt = new DataTable();
            dt = data.Sql_Datatable(consulta_sql(datepicker1.Text, datepicker2.Text));

            System.IO.MemoryStream ms = DataTableToExcelXlsx(dt, "Sheet1");
            ms.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.End();
        }
        public static System.IO.MemoryStream DataTableToExcelXlsx(DataTable table, string sheetName)
        {
            System.IO.MemoryStream Result = new System.IO.MemoryStream();
            ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);
            int col = 1;
            int row = 1;
            foreach (DataColumn column in table.Columns)
            {
                ws.Cells[row, col].Value = column.ColumnName.ToString();
                col++;
            }
            col = 1;
            row = 2;
            foreach (DataRow rw in table.Rows)
            {
                foreach (DataColumn cl in table.Columns)
                {
                    if (rw[cl.ColumnName] != DBNull.Value)
                        ws.Cells[row, col].Value = rw[cl.ColumnName].ToString();
                    col++;
                }
                row++;
                col = 1;
            }
            pack.SaveAs(Result);
            return Result;
        }

        protected void GridView1_PageSizeChanged(object sender, PageSizeChangeEventArgs e)
        {
            GridView1.PageSize = e.NewPageSize;
            GridView1.PageIndex = 0;
            FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, GridView1.CurrentSortDirection, (int)GridView1.CurrentSearchPageNo);


        }
        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Get_Excel();
        }
    }
}