using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;
using OfficeOpenXml;

namespace Rinya_Albal_App.Comercial
{
    public partial class Perdida_Referencias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    
    Quality con = new Quality();
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
        DataTable _datos_baja()
        {
            string Con_Q = @"Data Source=192.168.1.195\sqlserver2008;Initial Catalog=QC600;Persist Security Info=True;User ID=dso;Password=dsodsodso";
            string sql = @"select  ARTICULO, DESCRIPCION, count (*) as BAJAS
from(
SELECT DISTINCT  round (t2.[Código Cliente]/1000,0) AS CLIENTE,t1.Artículo AS ARTICULO,  REPLACE(REPLACE(ARTICULO.Descripción,CHAR(10),''),CHAR(13),'') as DESCRIPCION--, articulo.discrim_2							
FROM            ALBARAN_LIN AS t1 INNER JOIN							
                         ALBARAN_CABE AS t2 ON t1.Año = t2.Año AND t1.Empresa = t2.Empresa AND t1.Serie = t2.Serie AND t1.[Nº Albarán] = t2.[Nº Albarán] INNER JOIN							
                         ARTICULO ON t1.Artículo = ARTICULO.Artículo INNER JOIN							
                         CLIENTE ON t2.[Código Cliente] = CLIENTE.Cliente		
						 					
WHERE        (ARTICULO.activo <>0 and Articulo.discrim_2<>'8' and  ARTICULO.discrim_4 like '%V%' and (t2.Ruta between 2 and 16) and t1.FechaCreacionALin >= DATEADD(mm, - 9, GETDATE()))and ARTICULO.Activo <>0 and t1.cantidad>1 AND (NOT EXISTS							
                             (SELECT DISTINCT ALBARAN_LIN.Artículo, ALBARAN_CABE.[Código Cliente]							
                               FROM            ALBARAN_LIN INNER JOIN							
                                                         ALBARAN_CABE ON ALBARAN_LIN.Año = ALBARAN_CABE.Año AND ALBARAN_LIN.Empresa = ALBARAN_CABE.Empresa AND ALBARAN_LIN.Serie = ALBARAN_CABE.Serie AND 							
                                                         ALBARAN_LIN.[Nº Albarán] = ALBARAN_CABE.[Nº Albarán]							
                               WHERE        (ALBARAN_LIN.FechaCreacionALin >= DATEADD(mm, - 3, GETDATE())) AND (ALBARAN_CABE.[Código Cliente] = t2.[Código Cliente]))) ) as t

							   group by ARTICULO, DESCRIPCION

							   ORDER BY BAJAS desc";
            return con.Sql_Datatable(sql,Con_Q);
        }
        DataTable _datos_detalle_cliente()
        {
            string Con_Q = @"Data Source=192.168.1.195\sqlserver2008;Initial Catalog=QC600;Persist Security Info=True;User ID=dso;Password=dsodsodso";
            string sql = @"SELECT DISTINCT  round (t2.[Código Cliente]/1000,0) AS CLIENTE,t1.Artículo AS ARTICULO,  REPLACE(REPLACE(ARTICULO.Descripción,CHAR(10),''),CHAR(13),'') as DESCRIPCION--, articulo.discrim_2							
FROM ALBARAN_LIN AS t1 INNER JOIN
                         ALBARAN_CABE AS t2 ON t1.Año = t2.Año AND t1.Empresa = t2.Empresa AND t1.Serie = t2.Serie AND t1.[Nº Albarán] = t2.[Nº Albarán] INNER JOIN
ARTICULO ON t1.Artículo = ARTICULO.Artículo INNER JOIN
CLIENTE ON t2.[Código Cliente] = CLIENTE.Cliente


WHERE        (ARTICULO.activo<>0 and Articulo.discrim_2<>'8' and ARTICULO.discrim_4 like '%V%' and (t2.Ruta between 2 and 16) and t1.FechaCreacionALin >= DATEADD(mm, - 9, GETDATE()))and ARTICULO.Activo<>0 and t1.cantidad>1 AND (NOT EXISTS							
                             (SELECT DISTINCT ALBARAN_LIN.Artículo, ALBARAN_CABE.[Código Cliente]
FROM            ALBARAN_LIN INNER JOIN
ALBARAN_CABE ON ALBARAN_LIN.Año = ALBARAN_CABE.Año AND ALBARAN_LIN.Empresa = ALBARAN_CABE.Empresa AND ALBARAN_LIN.Serie = ALBARAN_CABE.Serie AND
ALBARAN_LIN.[Nº Albarán] = ALBARAN_CABE.[Nº Albarán]

WHERE        (ALBARAN_LIN.FechaCreacionALin >= DATEADD(mm, - 3, GETDATE())) AND(ALBARAN_CABE.[Código Cliente] = t2.[Código Cliente])))							
							   ORDER BY  round(t2.[Código Cliente]/1000,0)";
            return con.Sql_Datatable(sql, Con_Q);
        }
    protected void Btexport_Click(object sender, EventArgs e)
        {
            DataTable datos_baja = _datos_baja();
            DataTable datos_detalle_cliente = _datos_detalle_cliente();
            if (datos_baja.Rows.Count > 0 && datos_detalle_cliente.Rows.Count > 0)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    datos_baja.TableName = "Bajas";
                    datos_detalle_cliente.TableName = "Detalle_cliente";
                    List<string> dateColumns = new List<string>() { "FECHA" };
                    ExcelWorksheet LT = pck.Workbook.Worksheets.Add("Bajas");
                    ExcelWorksheet PR = pck.Workbook.Worksheets.Add("Detalle cliente");
                    LT.Cells["A1"].LoadFromDataTable(datos_baja, true, OfficeOpenXml.Table.TableStyles.Medium6);
                    FormatWorksheetData(dateColumns, datos_baja, LT);
                    PR.Cells["A1"].LoadFromDataTable(datos_detalle_cliente, true, OfficeOpenXml.Table.TableStyles.Medium2);
                    FormatWorksheetData(dateColumns, datos_detalle_cliente, PR);
                    Response.ContentType = "application/vnd.ms-excel";

                    Response.AddHeader("Content-disposition", "attachment; filename=Articulos_No_vendidos.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
            }

            }
    }
}