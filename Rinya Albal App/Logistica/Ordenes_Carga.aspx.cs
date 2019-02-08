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

namespace Rinya_Albal_App.Logistica
{
    public partial class Ordenes_Carga : System.Web.UI.Page
    {

        string _OC { get; set; }
        string _Almacen { get; set; }
        string _Estado { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            _Almacen = "6";
            _Estado = "2";
            if (!IsPostBack)
            {
                GridView1.ClearSearchFilters();
                _OC = OC_textBoxs.Text;

                // FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, (string)GridView1.CurrentSortDirection, 1);
            }
        }
        private void Buscar()
        {

            GridView1.Visible = false;

            DataTable dat = new DataTable();
            if (OC_textBoxs.Text.Length > 0)
            {

                _OC = OC_textBoxs.Text;
                GridView1.ClearSearchFilters();
                FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, (string)GridView1.CurrentSortDirection, 1);
                //FilterToNthPage(dat,)
                /*  GridView1.DataSource = Fabricacion_stock (DropDownDesde.SelectedValue, DropDownHasta.SelectedValue);
                  GridView1.DataBind();*/
                GridView1.Visible = true;

            }

        }

        private string consulta_sql(string oc)
        {
            _Almacen = "6";
            _Estado = "2";

            string sql = @"SELECT  distinct   OF_CABE.[Nº OF] as N_OF, OF_CABE.[Fecha Entrega] as F_ENTREGA, PEDIDO_LIN.Artículo as ARTICULO, ARTICULO.Descripción AS DESCRIPCION, AREA.Descripción as SECCION, 
                       ARTICULO.[Uds Venta] AS UDS_VENTA, Coalesce (SUM(PEDIDO_LIN.[Cant Pedida en Kg]),0) AS Kg_Pedido, 
                      SUM(PEDIDO_LIN.[Cant Pedida en Uds]) AS Ud_Pedido, Coalesce (sum(PEDIDO_LIN.[Cant Recibida en Uds]),0)  AS Ud_Servidas, coalesce (STOCK_ACTUAL.[Kg Actuales],0) as KG_Actuales,coalesce (STOCK_ACTUAL.[Unidades Actuales],0) as Ud_Actuales 
                      ,Coalesce(SUM(PEDIDO_LIN.[Cant Pedida en Kg]),0) - coalesce (STOCK_ACTUAL.[Kg Actuales],0) as KG_Pendiente, SUM(PEDIDO_LIN.[Cant Pedida en Uds])- Coalesce (sum(PEDIDO_LIN.[Cant Recibida en Uds]),0) - coalesce (STOCK_ACTUAL.[Unidades Actuales],0) as Ud_Pendiente
FROM         
                     ARTICULO inner join 
                     PEDIDO_LIN ON ARTICULO.Artículo = PEDIDO_LIN.Artículo AND PEDIDO_LIN.[C/P] = 'C'  inner join
                      OF_CABE ON OF_CABE.[Nº OF] = PEDIDO_LIN.[Nº OF]  INNER JOIN
                      PEDIDO_CABE ON PEDIDO_LIN.Año = PEDIDO_CABE.Año AND PEDIDO_LIN.Empresa = PEDIDO_CABE.Empresa AND 
                      PEDIDO_LIN.[C/P] = PEDIDO_CABE.[C/P] AND PEDIDO_LIN.[Nº Pedido] = PEDIDO_CABE.[Nº Pedido] 
                      inner join AREA on AREA.[Area Preparación]=ARTICULO.[Area Preparación]
                      left outer join STOCK_ACTUAL on STOCK_ACTUAL.Artículo =ARTICULO.Artículo and STOCK_ACTUAL.[Almacén]=" + _Almacen + @" and STOCK_ACTUAL.Estado=" + _Estado + @"

WHERE     (OF_CABE.[Nº OF] = " + oc + @")  
GROUP BY OF_CABE.[Nº OF], OF_CABE.[Fecha OF], OF_CABE.[Fecha Entrega], PEDIDO_LIN.Artículo, ARTICULO.Descripción, ARTICULO.[Area Preparación], 
                      ARTICULO.[Tipo Artículo], ARTICULO.[Uds Venta],STOCK_ACTUAL.[Kg Actuales],STOCK_ACTUAL.[Unidades Actuales],AREA.Descripción";

            return sql;
        }
        private bool Get_Excel()
        {
            Quality con = new Quality();

            //string sql = consulta_sql(string agencia);
            List<string> hideColumns = new List<string>() {

                "orden"
            };
            using (ExcelPackage pck = new ExcelPackage())
            {


                DataTable table = con.Sql_Datatable(consulta_sql(OC_textBoxs.Text));
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                FormatWorksheetData(hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Ordenes_carga.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            return true;
        }
        private static void FormatWorksheetData(List<string> hideColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

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
        private void FilterToNthPage(DataTable SearchFilterValues, string SortExpression, string SortDirection, int PageIndex)
        {
          /*  int PageSize = GridView1.PageSize;

            try
            {
                DataTable dt = GetSearchFilteredData("SearchForPagedData", "@SearchFilter", SearchFilterValues, consulta_sql(), SortExpression, SortDirection, PageIndex, PageSize);
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
            }*/
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

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Get_Excel();
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
/*
            string filename = "Export.xlsx";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            Quality data = new Quality();


            DataTable dt = new DataTable();
            dt = data.Sql_Datatable(consulta_sql());

            System.IO.MemoryStream ms = DataTableToExcelXlsx(dt, "Sheet1");
            ms.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.End();*/
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
    }

}
