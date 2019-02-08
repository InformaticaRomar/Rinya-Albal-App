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
    public partial class Ventas_Articulo_Pedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string datos_ventas(string F_desde, string F_hasta)
        {
            string sql = @"select t.EMPRESA,t.ARTICULO,t.NOMBRE_ARTICULO,sum(t.IMPORTE) IMPORTE,sum(t.Margen) Margen,t.ESTADO,t.MES, case when coalesce(t2.WCPC_REPRESENTANTE,0) <> 0 then t2.REP_NOMBRE else t.USUARIO_CREACION end Usuario, t.Ruta from (
select CIF_EMPRESA EMPRESA, DIF_ARTICULO ARTICULO,DIF_NOMBRE_ARTICULO NOMBRE_ARTICULO,SUM (dif_importe) IMPORTE,sum (dif_cantidad*(dif_precio -dif_precio_limite)) as Margen,CASE WHEN CIF_ESTADO=2 THEN 'COBRADO' WHEN CIF_ESTADO=1 THEN 'NO COBRADO' ELSE 'OTROS' END AS ESTADO,CPM_USUARIO_CREACION USUARIO_CREACION 
, to_char(to_date(TO_CHAR(CIF_FECHA_FACTURA,'mm'),'mm'),'Month','NLS_DATE_LANGUAGE = SPANISH') as MES ,
 CPM_NUMERO_PEDIDO, DETALLE_IMPRESO_FRA_CLTE.DIF_AG_TRANSPORTE as Ruta
FROM DETALLE_IMPRESO_FRA_CLTE,CABECERA_IMPRESO_FRA_CLTE, CABECERA_PEDIDO_CLIENTE 
WHERE 
CIF_EMPRESA in (1,2) and 
CIF_EMPRESA = DIF_EMPRESA AND 
CIF_CONTABILIDAD = DIF_CONTABILIDAD AND 
CIF_EJERCICIO = DIF_EJERCICIO AND 
CIF_NUMERO_FACTURA = DIF_NUMERO_FACTURA  
AND  CABECERA_PEDIDO_CLIENTE.CPM_EMPRESA=DIF_EMPRESA 
AND CPM_CONTABILIDAD=DIF_CONTABILIDAD 
AND CPM_NUMERO_PEDIDO = DIF_NUMERO_PEDIDO 
and CABECERA_PEDIDO_CLIENTE.CPM_USUARIO_CREACION<>'GRINYA_EXPERT'  
and CIF_FECHA_FACTURA BETWEEN to_date('" + F_desde + @"') AND to_date('" + F_hasta + @"') 
GROUP BY DIF_ARTICULO,DIF_NOMBRE_ARTICULO,CIF_ESTADO,CPM_USUARIO_CREACION,CIF_EMPRESA, to_char(to_date(TO_CHAR(CIF_FECHA_FACTURA,'mm'),'mm'),'Month','NLS_DATE_LANGUAGE = SPANISH'),CPM_NUMERO_PEDIDO, DIF_AG_TRANSPORTE order by CIF_FECHA_FACTURA)  t
left join (select REP_NOMBRE, WCPC_NUMERO_PEDIDO_ERP,WCPC_REPRESENTANTE,WCPC_EMPRESA_ERP
from PEDIDOS_PROCESADOS_TPV INNER JOIN REPRESENTANTE ON rep_empresa=WCPC_EMPRESA_ERP AND REP_REPRESENTANTE=WCPC_REPRESENTANTE) t2 on t.EMPRESA=t2.WCPC_EMPRESA_ERP and t2.WCPC_NUMERO_PEDIDO_ERP=CPM_NUMERO_PEDIDO
group by t.EMPRESA,t.ARTICULO,t.NOMBRE_ARTICULO,t.ESTADO,t.USUARIO_CREACION,t.MES,t2.REP_NOMBRE, t2.WCPC_REPRESENTANTE, t.Ruta";
            return sql;
        }
        private bool Get_Excel()
        {
            Expert con = new Expert();
            string F_desde = datepicker1.Value;
            string F_hasta = datepicker2.Value;
            string sql = datos_ventas(F_desde, F_hasta);
            List<string> hideColumns = new List<string>() {

                "orden"
            };
            
            using (ExcelPackage pck = new ExcelPackage())
            {


                DataTable table = con.Sql_Datatable(sql);
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                FormatWorksheetData(hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Pedidos_Ventas.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
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
        protected void Btexport_Click(object sender, EventArgs e)
        {
            Get_Excel();
        }
    }
}