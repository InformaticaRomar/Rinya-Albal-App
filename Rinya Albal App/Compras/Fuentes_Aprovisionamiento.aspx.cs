using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using OfficeOpenXml;

namespace Rinya_Albal_App.Compras
{
    public partial class Fuentes_Aprovisionamiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string datos_aprovisiona()
        {
            string sql = @"select  fap_codigo_empresa, fap_codigo_proveedor, PROVEEDOR.PRO_NOMBRE,Fap_codigo_producto,PR_PRODUCTO.PRO_DESCRIPCION, FAP_DENOMINACION_PRODUCTO_PROV
,decode(PROVEEDOR.PRO_ESTADO, 'A', 'Alta', 'B','Baja') as Estado_proveedor,
decode(PR_PRODUCTO.PRO_ESTADO, 'A', 'Alta', 'B','Baja') as Estado_producto,
decode(FAP_ESTADO, '0', 'Activa','1', 'Bloqueada','2', 'Obsoleta', '3','Baja','4', 'Descatalogada') as Estado_fuente_aprovis
,decode(FAP_BLOQUEO_ORDEN_COMPRA_SN,'S','SI','N','NO') as Bloqueo_Orden_Compra
,decode(FAP_HOMOLOGADA,'S','SI','N','NO') as Homologada
,decode(PR_PRODUCTO.PRO_TIPO_PRODUCTO, '0', 'Sin Definir','I','SEMIELABORADO','M','MATERIA PRIMA','C','COMPONENTES','A','ARTICULO','E','ENVASE','U','UTILLAJE','V','VARIOS', 'S', 'SERVICIO') as TIPO_PRODUCTO
,PRO_CLAVE_ESTAD_PRINCIPAL as FAMILIA
,( select  cuf_denominacion  from PR_CU_FAMILIA where cuf_codigo_familia=PRO_CLAVE_ESTAD_PRINCIPAL and  cuf_clave=1 and  ROWNUM <= 1) as DESCRIP_FAM
,PRO_CLAVE_ESTAD_ALTERNATIVA as FAMILIA_ALTERNATIVA
,( select  cuf_denominacion  from PR_CU_FAMILIA where cuf_codigo_familia=PRO_CLAVE_ESTAD_ALTERNATIVA and  cuf_clave=2 and  ROWNUM <= 1) as DESCRIP_ALTER
from PR_FUENTE_APROVISIONAMIENTO
inner join PROVEEDOR on PROVEEDOR.PRO_PROVEEDOR=fap_codigo_proveedor and Fap_codigo_empresa=PROVEEDOR.pro_empresa
inner join 
PR_PRODUCTO on PR_PRODUCTO.PRO_CODIGO_PRODUCTO= Fap_codigo_producto and PR_PRODUCTO.PRO_EMPRESA=Fap_codigo_empresa

 where fap_codigo_empresa in (1,2)";
            return sql;
        }

        private bool Get_Excel()
        {
            Expert con = new Expert();
            string sql = datos_aprovisiona();
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
                Response.AddHeader("Content-disposition", "attachment; filename=Fuentes_Aprovisionamiento.xlsx");
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
        protected void Btexport_Click(object sender, EventArgs e)
        {
            Get_Excel();
        }
    }
}