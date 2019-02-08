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
    public partial class Albaran_usuario_Kilos : System.Web.UI.Page
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

        private string consulta_sql(string fecha1, string fecha2)
        {

            string sql = @"SELECT DISTINCT  ALBARAN_CABE.[Nº Albarán] as N_albaran, ALBARAN_CABE.[Fec_hora emisión], ALBARAN_CABE.Usuario, ALBARAN_CABE.Empresa, CLIENTE.Cliente, 
                      CLIENTE.[Nombre envio] AS [Nombre Cliente], ALBARAN_LIN.Artículo, 
                      CASE ARTICULO.[Uds Venta] WHEN 'Ud' THEN ALBARAN_LIN.Cantidad * ARTICULO.[Factor(Kg/Ud)] ELSE ALBARAN_LIN.Cantidad END AS [Cantidad en Kg], 
                      CASE ARTICULO.[Uds Venta] WHEN 'Kg' THEN ALBARAN_LIN.Cantidad * ARTICULO.[Factor(Kg/Ud)] ELSE ALBARAN_LIN.Cantidad END AS [Cantidad en UDs], 
                      CASE WHEN ALBARAN_LIN.Cantidad > 0 AND ARTICULO.[Factor(Kg/Ud)] > 0 AND 
                      ARTICULO.[Factor(Kg/Cj)] > 0 THEN (CASE ARTICULO.[Uds Venta] WHEN 'Ud' THEN (ALBARAN_LIN.Cantidad * ARTICULO.[Factor(Kg/Ud)]) 
                      / ARTICULO.[Factor(Kg/Cj)] ELSE ALBARAN_LIN.Cantidad / ARTICULO.[Factor(Kg/Cj)] END) ELSE '0' END AS [Cantidad de cajas], ARTICULO.[Factor(Kg/Cj)], 
                      ARTICULO.[Uds Venta], ALBARAN_LIN.[Nº linea Albarán], ALBARAN_LIN.Cantidad, ARTICULO.[Factor(Kg/Ud)], ALBARAN_LIN.Cantidad AS Expr1, ALBARAN_LIN.Cajas, 
                      CLIENTE.[Población envio] AS Poblacion, CLIENTE.[Cod Postal envio], ALBARAN_CABE.Ruta,RUTA.Descripción, ALBARAN_CABE.Cerrado, ALBARAN_CABE.[Albarán Impreso], 
                      ALBARAN_CABE.Observaciones, ALBARAN_LIN.[Nº Pedido]
FROM         ARTICULO RIGHT OUTER JOIN
                      ALBARAN_LIN RIGHT OUTER JOIN
                      ALBARAN_CABE INNER JOIN
                      RUTA ON ALBARAN_CABE.Ruta = RUTA.Ruta ON ALBARAN_LIN.Serie = ALBARAN_CABE.Serie AND ALBARAN_LIN.Empresa = ALBARAN_CABE.Empresa AND 
                      ALBARAN_LIN.Año = ALBARAN_CABE.Año AND ALBARAN_LIN.[Nº Albarán] = ALBARAN_CABE.[Nº Albarán] ON 
                      ARTICULO.Artículo = ALBARAN_LIN.Artículo LEFT OUTER JOIN
                      CLIENTE ON ALBARAN_CABE.[Código Cliente] = CLIENTE.Cliente
WHERE albaran_cabe.[Nº Albarán]>0 and ALBARAN_LIN.Cantidad>0 and ((dbo.ALBARAN_CABE.Cerrado)=-1) AND ((dbo.ALBARAN_CABE.[Albarán Impreso])=-1) AND dbo.ALBARAN_CABE.[Fecha Emisión] between CONVERT (datetime,'" + fecha1 + @"',103) and  CONVERT (datetime,'" + fecha2 + @"',103)";

            return sql;
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
            catch (System.Exception )
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

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
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
            Quality data = new Quality();


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
    }

}