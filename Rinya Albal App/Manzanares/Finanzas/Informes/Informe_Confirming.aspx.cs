using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;

namespace Rinya_Albal_App.Manzanares.Finanzas.Informes
{
    public partial class Informe_Confirming : System.Web.UI.Page
    {
        public string strSQL;
        public string cadenaConexion =System.Web.Configuration.WebConfigurationManager.ConnectionStrings["QC600ConnectionString"].ConnectionString;
        public  SqlConnection conexion;
        public SqlCommand comando;
        public SqlDataReader objDataReader;
        public int operacion;
        public int totalItemSeleccionados;
        private string _sql2;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                string command = SqlDataSource1.SelectCommand; // added just for debug purpose
                _sql2 = Convert.ToString(Session["_sql_"]);
                if (_Sql == null || _Sql == "")
                {
                    _Sql = command;

                }
                if (_sql2 != null || _sql2 != "")
                {
                    SqlDataSource1.SelectCommand = _sql2;
                    SqlDataSource1.DataBind();
                    GridView_Confirming.DataBind();
                }
            }
            else if (!this.IsPostBack) {
                ShowData();
            }
           /* if (!this.IsPostBack)
            {
                //gvUsuarios.DataSource = con.seleccion("select * from usuarios");
                string command = SqlDataSource1.SelectCommand; // added just for debug purpose
                if (_Sql == null || _Sql == "")
                {
                    _Sql = command;

                }
                SqlDataSource1.DataBind();
                GridView_Confirming.DataBind();
            }*/
            }

        protected void GridView_Confirming_DataBound(object sender, EventArgs e)
        {
            //Recupera la el PagerRow..
            GridViewRow pagerRow = GridView_Confirming.BottomPagerRow;
            //Recupera los controles DropDownList y label...
            try
            {
                DropDownList pageList = pagerRow.Cells[0].FindControl("PageDropDownList") as DropDownList;
                Label pageLabel = pagerRow.Cells[0].FindControl("CurrentPageLabel") as Label;
                if (pageList != null)
                {
                    for (int i = 0; i < GridView_Confirming.PageCount; i++)
                    {
                        int pageNumber = i + 1;
                        //Se crea un objeto ListItem para representar la página...
                        ListItem item = new ListItem(pageNumber.ToString());
                        if (i == GridView_Confirming.PageIndex)
                        {
                            item.Selected = true;
                        }
                        //Se añade el ListItem a la colección de Items del DropDownList...
                        pageList.Items.Add(item);
                    }
                }
                if (pageLabel != null)
                {
                    // Calcula el nº de página actual...
                    int currentPage = GridView_Confirming.PageIndex + 1;
                    //Actualiza el Label control con la página actual.
                    pageLabel.Text = "Página " + currentPage.ToString() + " de " + GridView_Confirming.PageCount.ToString();

                }
            }
            catch { }
        }

        protected void GridView_Confirming_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            
            if (e.Exception == null)
            {
                lblInfo.Text = " ¡Linea eliminada/s OK! ";
                lblInfo.CssClass = "label label-success";
            }else {
                lblInfo.Text = " ¡Se ha producido un error al intentar elimnar la linea/s! ";
                lblInfo.CssClass = "label label-danger";
                e.ExceptionHandled = true;
            }
        }

        protected void ShowData()
        {
            SqlDataSource1.SelectCommand = _sql2;
            SqlDataSource1.DataBind();
            GridView_Confirming.DataBind();
            /*  dt = new DataTable();
              con = new SqlConnection(cs);
              con.Open();
              adapt = new SqlDataAdapter("Select ID,Name,City from tbl_Employee", con);
              adapt.Fill(dt);
              if (dt.Rows.Count > 0)
              {
                  GridView1.DataSource = dt;
                  GridView1.DataBind();
              }
              con.Close();*/
        }
        protected void GridView_Confirming_RowEditing(object sender, GridViewEditEventArgs e)
        {
            lblInfo.Text = "";
            GridView_Confirming.EditIndex = e.NewEditIndex;
            ShowData();
        }

        protected void GridView_Confirming_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                lblInfo.Text = " ¡Modificación realizada OK! ";
                lblInfo.CssClass = "label label-success";
            }
            else
            {
                lblInfo.Text = " ¡Se ha producido un error al intentar modificar la linea/s! ";
                lblInfo.CssClass = "label label-danger";
                e.ExceptionHandled = true;
            }
        }

        protected void GridView_Confirming_PreRender(object sender, EventArgs e)
        {
            if (totalItemSeleccionados > 0)
            {
                btnQuitarSeleccionados.CssClass = "btn btn-lg btn-danger";
            }
            else
            {
                btnQuitarSeleccionados.CssClass = "btn btn-lg btn-danger disabled";
            }
        }

        protected void btnQuitarSeleccionados_Click(object sender, EventArgs e)
        {
            //Recorrer las filas del GridView...
            foreach (GridViewRow row in GridView_Confirming.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBoxElim = row.FindControl("chkEliminar") as CheckBox;
                    if (CheckBoxElim.Checked)
                    {
                        GridView_Confirming.DeleteRow(row.RowIndex);
                    }
                }

            }
            GridView_Confirming.DataBind();
        }
        protected void chk_OnCheckedChanged(object sender, EventArgs e)
        {
            //Recorrer las filas del GridView...
            //Quita el mensaje de información si lo hubiera...
            foreach (GridViewRow row in GridView_Confirming.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox CheckBoxElim = row.FindControl("chkEliminar") as CheckBox;
                    if (CheckBoxElim.Checked)
                    {
                        totalItemSeleccionados += 1;
                    }
                }
                  
            }
        }
        protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Recupera la fila.
            GridViewRow pagerRow = GridView_Confirming.BottomPagerRow;
            //Recupera el control DropDownList...
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
            //Se Establece la propiedad PageIndex para visualizar la página seleccionada...
            GridView_Confirming.PageIndex = pageList.SelectedIndex;
            //Quita el mensaje de información si lo hubiera...
            lblInfo.Text = "";

        }
        protected void SqlDataSource1_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            lblTotalClientes.Text = e.AffectedRows.ToString();
        }

        protected void SqlDataSource1_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
           
        }
        public string _Sql = "";
        private void datos_2_grid(int tipo)
        {
            string sql = @"SELECT [ID]
      ,Convert(varchar,[FECHA_EMISION],103) as [FECHA_EMISION]
      ,[ENTIDAD]
      , Convert(varchar,[FECHA_PAGO],103) as [FECHA_PAGO]
      ,[PROVEEDOR]
      ,[N_FACTURA]
      ,[EMPRESA]
      ,[IMPORTE]
      ,Convert(varchar,[FECHA_VENCIMIENTO],103) as [FECHA_VENCIMIENTO]
      ,[MES_VENCIMIENTO]
      ,[NOMINAL]
      ,[PAGADO] FROM [CONFIRMING]";
            switch (tipo) { 
                case 1:
                    sql= sql+ " WHERE [FECHA_EMISION] between convert(datetime,'" + datepicker_1.Text + "',103) and  convert(datetime,'" + datepicker_2.Text + "',103)"; 
                break;
                case 2:
                    sql = sql + " WHERE [FECHA_PAGO] between convert(datetime,'" + datepicker_1b.Text + "',103) and  convert(datetime,'" + datepicker_2b.Text + "',103)";
                    break;
                case 3:
                    sql = sql + " WHERE [FECHA_VENCIMIENTO] between convert(datetime,'" + datepicker_1c.Text + "',103) and  convert(datetime,'" + datepicker_2c.Text + "',103)";
                    break;
                case 4:
                    string a = Request.Form["ctl00$MainContent$txtEntidad_"].ToString().Trim().Replace("\r","").Replace("\n","");
                    sql = sql + " Where ENTIDAD like '%"+ Request.Form["ctl00$MainContent$txtEntidad_"].ToString().Trim().Replace("\r", "").Replace("\n", "") + @"%'";
                    break;
            }
            _Sql = sql;
            _sql2= sql;
            Session["_sql_"] = sql;
            SqlDataSource1.SelectCommand = sql;
            GridView_Confirming.PageIndex = 0;
            GridView_Confirming.EditIndex = -1;
            GridView_Confirming.SelectedIndex = -1;
            GridView_Confirming.DataBind();
            Panel_grid.Visible = true;
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Confirming.PageIndex = e.NewPageIndex;
            GridView_Confirming.DataBind();
        }
        private void get_excel() {
            
           
            DataTable datos = new DataTable(); //(DataTable)GridView_Confirming.DataSource;
            string pr = _Sql;
            string pr2= _sql2;

            for (int i = 3; i < GridView_Confirming.Columns.Count; i++)
            {
                datos.Columns.Add(GridView_Confirming.Columns[i].ToString());
            }
            foreach (GridViewRow row in GridView_Confirming.Rows)
            {
                DataRow dr = datos.NewRow();
                for (int j = 3; j < GridView_Confirming.Columns.Count; j++)
                {
                    if (String.IsNullOrEmpty(row.Cells[j].Text)) { 
                    if (row.Cells[j].Controls[0].GetType().ToString().Contains("DataBoundLiteralControl")) {
                           
                            DataBoundLiteralControl dat= (DataBoundLiteralControl) row.Cells[j].Controls[0];
                            
                            dr[GridView_Confirming.Columns[j].ToString()] = dat.Text.Replace("\r\n", "").Trim();
                            // row.Cells[j].Text;
                                                                                                                // string a=LB.Text ;
                        }
                    }
                    else if (!row.Cells[j].Text.Equals("&nbsp;"))
                        dr[GridView_Confirming.Columns[j].ToString()] = row.Cells[j].Text;
                }

                datos.Rows.Add(dr);
            }

            if (datos.Rows.Count > 0)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                    List<string> dateColumns = new List<string>() { "F. Emision", "F. Pago" , "F. Vencimiento" };

                    ws.Cells["A1"].LoadFromDataTable(datos, true, OfficeOpenXml.Table.TableStyles.Medium14);
                    FormatWorksheetData(dateColumns, datos, ws);
                    // make sure it is sent as a XLSX file
                    Response.ContentType = "application/vnd.ms-excel";
                    // make sure it is downloaded rather than viewed in the browser window
                    Response.AddHeader("Content-disposition", "attachment; filename=Confirming.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
            }
          }
        private static void FormatWorksheetData(List<string> dateColumns, DataTable table, ExcelWorksheet ws)
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


        }
        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            datos_2_grid(1);
            //SqlDataSource1.SelectCommand = sql; // 'sql' is the new query
         
            //updatePanel.Update();
        }
        protected void BtBuscar_3_Click(object sender, EventArgs e)
        {
            datos_2_grid(3);
        }
        protected void BtBuscar_2_Click(object sender, EventArgs e)
        {
            datos_2_grid(2);
        }

        protected void BtBuscar_4_Click(object sender, EventArgs e)
        {
            get_excel();
        }

        protected void GridView_Confirming_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            datos_2_grid(4);
        }
        //
    }

}