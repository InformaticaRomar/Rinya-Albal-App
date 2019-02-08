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
    public partial class Control_stock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //  GridView1.ClearSearchFilters();
                //  FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, (string)GridView1.CurrentSortDirection, 1);
            }

        }
        private bool export_2()
        {
            Quality con = new Quality();
            string sql = Fabricacion_stock(DropDownDesde.SelectedValue, DropDownHasta.SelectedValue);

            using (ExcelPackage pck = new ExcelPackage())
            {
                /*ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Stock");
                ws.Cells["A1"].LoadFromDataTable(con.Sql_Datatable(sql), true, OfficeOpenXml.Table.TableStyles.Medium14);
                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=stock.xlsx");

                byte[] a = pck.GetAsByteArray();
                Response.OutputStream.Write(a, 0, a.Length);

                Response.OutputStream.Flush();
                Response.OutputStream.Close();*/

                DataTable table = con.Sql_Datatable(sql);
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Resumen");

                var dataRange = ws.Cells[ws.Dimension.Address.ToString()];
                dataRange.AutoFitColumns();
                var pivotTable = ws2.PivotTables.Add(ws2.Cells["A3"], dataRange, "Pivotname");
                pivotTable.MultipleFieldFilters = true;
                pivotTable.RowGrandTotals = true;
                pivotTable.ColumGrandTotals = true;
                pivotTable.Compact = true;
                pivotTable.CompactData = true;
                pivotTable.GridDropZones = false;
                pivotTable.Outline = false;
                pivotTable.OutlineData = false;
                pivotTable.ShowError = true;
                pivotTable.ErrorCaption = "[error]";
                pivotTable.ShowHeaders = true;
                pivotTable.UseAutoFormatting = true;
                pivotTable.ApplyWidthHeightFormats = true;
                pivotTable.ShowDrill = true;
                pivotTable.FirstDataCol = 3;
                pivotTable.RowHeaderCaption = "Articulos";

                var AlmacenField = pivotTable.Fields["Almacen"];
                pivotTable.PageFields.Add(AlmacenField);
                AlmacenField.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                var FamiliaField = pivotTable.Fields["Familia"];
                pivotTable.PageFields.Add(FamiliaField);
                FamiliaField.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                //Sumatorio
                var Kg_Field = pivotTable.Fields["Kg Actuales"];
                pivotTable.DataFields.Add(Kg_Field);
                //pivotTable.ColumnFields.Add(Kg_Field);
                

               var UD_Field = pivotTable.Fields["Unidades Actuales"];
                pivotTable.DataFields.Add(UD_Field);
                //pivotTable.ColumnFields.Add(pivotTable.DataFields.Add(UD_Field););

                /*
                var countryField = pivotTable.Fields["Country"];
                pivotTable.RowFields.Add(countryField);
                var gspField = pivotTable.Fields["GSP / DRSL"];
                pivotTable.RowFields.Add(gspField);*/

                /*                var oldStatusField = pivotTable.Fields["Kg Actuales"];
                                pivotTable.ColumnFields.Add(oldStatusField);
                                var newStatusField = pivotTable.Fields["Unidades Actuales"];
                                pivotTable.ColumnFields.Add(newStatusField);*/

                var ArticuloDateField = pivotTable.Fields["Articulo"];
                pivotTable.RowFields.Add(ArticuloDateField);
                var DescripcionDateField = pivotTable.Fields["Descripcion"];
                pivotTable.RowFields.Add(DescripcionDateField);
                var FechaCaducidadDateField = pivotTable.Fields["FechaCaducidad"];
                pivotTable.RowFields.Add(FechaCaducidadDateField);
                /* submittedDateField.AddDateGrouping(OfficeOpenXml.Table.PivotTable.eDateGroupBy.Months | OfficeOpenXml.Table.PivotTable.eDateGroupBy.Days);
                 var monthGroupField = pivotTable.Fields.GetDateGroupField(OfficeOpenXml.Table.PivotTable.eDateGroupBy.Months);
                 monthGroupField.ShowAll = false;
                 var dayGroupField = pivotTable.Fields.GetDateGroupField(OfficeOpenXml.Table.PivotTable.eDateGroupBy.Days);
                 dayGroupField.ShowAll = false;*/


                //  FormatWorksheetData(dateColumns, hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Stock.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            return true;
        }
        private void export()
        {
            string oFileName = @"stock.xlsx";
            if (DropDownDesde.SelectedValue.Length > 0 && DropDownHasta.SelectedValue.Length > 0)
            {
                try
                {
                    Quality con = new Quality();
                    string sql = Fabricacion_stock(DropDownDesde.SelectedValue, DropDownHasta.SelectedValue);
                    DataTable datos = con.Sql_Datatable(sql);
                    if (datos != null)
                    {
                        ExportToExcel(datos, oFileName);

                    }

                }
                catch (Exception )
                { }
            }
        }
        private void ExportToExcel(DataTable dt, string filename)
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    // string filename = "traza.xls";
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
        private void Buscar()
        {

            GridView1.Visible = false;

            DataTable dat = new DataTable();
            if (DropDownDesde.SelectedValue.Length > 0 && DropDownHasta.SelectedValue.Length > 0)
            {
                GridView1.ClearSearchFilters();
                FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, (string)GridView1.CurrentSortDirection, 1);
                //FilterToNthPage(dat,)
                /*  GridView1.DataSource = Fabricacion_stock (DropDownDesde.SelectedValue, DropDownHasta.SelectedValue);
                  GridView1.DataBind();*/
                GridView1.Visible = true;

            }

        }

        private string Fabricacion_stock(string almacen1, string almacen2)
        {

            string sql = @"SELECT DISTINCT 
                         [STOCK PARTIDAS].Artículo as Articulo , REPLACE(REPLACE(REPLACE(ARTICULO.Descripción,CHAR(9),''),CHAR(10),''),CHAR(13),'') as Descripcion, SUM(SSCC_CON.KgActual) AS [Kg Actuales], SUM(SSCC_CON.UdActual) AS [Unidades Actuales], SSCC_CON.Estado, Estado.Estado AS [Descripcion Estado], 
                         [STOCK PARTIDAS].Almacén as Almacen, [STOCK PARTIDAS].LoteInterno,'' as N_Palet, CONVERT(varchar, [STOCK PARTIDAS].FechaCaducidad, 103) AS FechaCaducidad,CONVERT(varchar, [STOCK PARTIDAS].[Fecha Creación], 103) AS FechaFabricacion,SSCC.SSCC, CAST([STOCK PARTIDAS].Año AS varchar) 
                         + '/' + CAST([STOCK PARTIDAS].Empresa AS varchar) + '/' + [STOCK PARTIDAS].Serie + '/' + CAST([STOCK PARTIDAS].[Nº Partida] AS varchar) AS Partida, ARTICULO.Discrim_2 AS Discriminador, ARTICULO.Familia, 
                         '' as OBS_ESTADO
FROM            SSCC_CON INNER JOIN
                         ARTICULO INNER JOIN
                         [STOCK PARTIDAS] ON ARTICULO.Artículo = [STOCK PARTIDAS].Artículo ON SSCC_CON.IdLote = [STOCK PARTIDAS].IDSSCC INNER JOIN
                         Estado ON SSCC_CON.Estado = Estado.ID_Estado INNER JOIN
						 sscc on sscc.id=SSCC_CON.IdPadre

WHERE        (ARTICULO.[Tipo Lote] <> '-') AND ([STOCK PARTIDAS].Almacén >=" + almacen1 + @" OR [STOCK PARTIDAS].Almacén<=" + almacen2 + @") 
GROUP BY [STOCK PARTIDAS].Artículo, [STOCK PARTIDAS].LoteInterno, SSCC_CON.Estado, Estado.Estado, [STOCK PARTIDAS].Almacén, [STOCK PARTIDAS].FechaCaducidad, ARTICULO.Descripción, ARTICULO.Discrim_2, 
                        ARTICULO.Familia, [STOCK PARTIDAS].Año, [STOCK PARTIDAS].Empresa, [STOCK PARTIDAS].Serie, [STOCK PARTIDAS].[Nº Partida]--, DATOS_ORGANOLEPTICO.SSCC, 
                         ,SSCC.SSCC
						 ,[STOCK PARTIDAS].[Fecha Creación]
						 HAVING        (SUM(SSCC_CON.KgActual) > 0.001) OR
                         (SUM(SSCC_CON.UdActual) > 0)
ORDER BY [STOCK PARTIDAS].Artículo";

            return sql;
        }

        private void FilterToNthPage(DataTable SearchFilterValues, string SortExpression, string SortDirection, int PageIndex)
        {
            int PageSize = GridView1.PageSize;

            try
            {
                DataTable dt = GetSearchFilteredData("SearchForPagedData", "@SearchFilter", SearchFilterValues, Fabricacion_stock(DropDownDesde.SelectedValue, DropDownHasta.SelectedValue), SortExpression, SortDirection, PageIndex, PageSize);
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
            //Buscar();
            export_2();
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
            dt = data.Sql_Datatable(Fabricacion_stock(DropDownDesde.SelectedValue, DropDownHasta.SelectedValue));

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