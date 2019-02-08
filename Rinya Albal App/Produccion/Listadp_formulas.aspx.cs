using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;
using OfficeOpenXml;

namespace Rinya_Albal_App.Produccion
{
    public partial class Listadp_formulas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private static void FormatWorksheetData(List<string> dateColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            ExcelRange r;
            /*try { 
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
            }
            catch { }*/
            // get all data and autofit
            r = ws.Cells[1, 1, rowCount + 1, columnCount];
            r.AutoFitColumns();


        }

        DataTable datos_formulas()
        {
            string sql = @"SELECT distinct  [DESPIECE_CABE].[Despiece] as [Despiece Formula]
     
      ,[DESPIECE_CABE].[Artículo] as Formula
    
     ,REPLACE(REPLACE(art3.Descripción,CHAR(10),''),CHAR(13),'')  as [Descrip. Formula]
	 ,dpc.Despiece as [Despiece Pre]

	 ,dpc.Artículo  as [Pre]
	 ,REPLACE(REPLACE(art2.Descripción,CHAR(10),''),CHAR(13),'')  as [Descrip. Pre]
	 ,dpl.Producto as [Articulo Vta]
	 
	 ,REPLACE(REPLACE(art3.Descripción,CHAR(10),''),CHAR(13),'')  as [Descrip. Vta]
  FROM [QC_PRUEBAS].[dbo].[DESPIECE_CABE] inner join ARTICULO art1 on art1.Artículo=[DESPIECE_CABE].Artículo 
  inner join DESPIECE_LIN on DESPIECE_LIN.Despiece=[DESPIECE_CABE].Despiece
  inner join ARTICULO art2 on art2.Artículo=DESPIECE_LIN.Producto
  inner join [DESPIECE_CABE] dpc on dpc.Artículo=DESPIECE_LIN.Producto
  inner join DESPIECE_LIN dpl on dpc.Despiece=dpl.Despiece
  inner join Articulo art3 on dpl.Producto=art3.Artículo

  where 
   art3.Discrim_4 like '%V%'
  and art3.Activo <>0";
            return con.Sql_Datatable(sql);

        }
        Quality con = new Quality();
        protected void Btexport_Click(object sender, EventArgs e)
        {
            DataTable datos = datos_formulas();
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet LT = pck.Workbook.Worksheets.Add("Formulas");
                List<string> dateColumns = new List<string>() { "FECHA" };
                LT.Cells["A1"].LoadFromDataTable(datos, true, OfficeOpenXml.Table.TableStyles.Medium6);
                FormatWorksheetData(dateColumns, datos, LT);
                Response.ContentType = "application/vnd.ms-excel";

                Response.AddHeader("Content-disposition", "attachment; filename=Despiece_articulo.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
        }
    }
}