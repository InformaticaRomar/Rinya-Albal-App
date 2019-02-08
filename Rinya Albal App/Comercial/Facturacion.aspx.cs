using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using OfficeOpenXml;

namespace Rinya_Albal_App.Comercial
{
    public partial class Facturacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private static void FormatWorksheetData(List<string> dateColumns, DataTable table, ExcelWorksheet ws)
        {
            int columna_inicio_tabla = 5;
            int columnCount = table.Columns.Count + columna_inicio_tabla;
            int rowCount = table.Rows.Count;
            string nombre = table.TableName;

            ExcelRange r;

            // which columns have dates in
            for (int i = columna_inicio_tabla; i < columnCount; i++)
            {
                try
                {
                    // if (dateColumns.Contains(ws.Tables[nombre].Columns[i-1].Name))
                    // if cell header value matches a date column
                    if (dateColumns.Contains(ws.Cells[1, i].Value.ToString()))
                    {
                        // r=ws.Tables[nombre].Columns[i].Position
                        //int l = ws.Tables[nombre].Address //.Columns[i].DataCellStyleName;
                        r = ws.Cells[2, i, rowCount + 1, i];
                        r.AutoFitColumns();
                        r.Style.Numberformat.Format = @"dd/MMM/yyyy";
                    }
                }
                catch { }
            }
            // get all data and autofit
            r = ws.Cells[1, 1, rowCount + 1, columnCount - 1];
            r.AutoFitColumns();


        }


        DataTable Facturacion_expert(string desde, string hasta)
        {
            string sql = "  SELECT " +
"CIF_EMPRESA AS \"EMPRESA\"" +
",CIF_CLIENTE AS \"CLIENTE\"" +
",CIF_SC_ENV_FRA AS \"SUCURSAL\"" +
", CIF_NOMBRE_CLIENTE AS \"NOMBRE_CLIENTE\"" +
",CIF_SERIE_FACTURA AS \"SERIE\"" +
",CIF_SECUENCIA_FACTURA AS \"N_FACTURA\"" +
",TO_CHAR(CIF_FECHA_FACTURA, 'MONTH') AS \"MES\"" +
",CIF_FECHA_FACTURA AS \"FECHA_FACTURA\"" +

", dif_ag_transporte AS \"AG_TRANSPORTE\"" +
", dif_nombre_agencia AS \"NOMBRE_AGENCIA\"  " +
",CIF_REPRESENTANTE AS \"COMERCIAL\"" +
",Cif_NOMBRE_AGENTE AS \"NOMBRE_COMERCIAL\"" +
",substr(cif_pob_env_fra,1,instr(cif_pob_env_fra,'-')-1)  AS \"CP\"" +

", dif_articulo  AS \"ARTICULO\"" +
",dif_nombre_articulo AS \"NOMBRE_ARTICULO\"" +
",DIF_PRECIO AS \"Precio\"" +
",round (CASE WHEN (DIF_PRECIO*DIF_CANTIDAD_UMV)=0 THEN 0 ELSE (((DIF_PRECIO*DIF_CANTIDAD_UMV)-DIF_IMPORTE_ESTADISTICA)*100)/(DIF_PRECIO*DIF_CANTIDAD_UMV) END ,2 ) AS \"Factor Descuento\"" +
",DIF_CANTIDAD_UMV  as \"Cant.UMV\" " +
",dif_UMV AS \"UMV\" " +
",CASE WHEN dif_UMV='KG' THEN DIF_CANTIDAD_UMV ELSE " +
"Round (DIF_CANTIDAD_UMV * NUMBER_TEST( case when NUMBER_TEST((select NVL(MAX(REPLACE(PCA_VALOR_CARACTERISTICA,'\"','')),'N') FROM PR_CU_CARAC_PRODUCTO WHERE PCA_EMPRESA = DIF_EMPRESA AND PCA_CODIGO_PRODUCTO = DIF_ARTICULO AND PCA_CODIGO_CARACTERISTICA = 'PESO_FIJO') )=0 then 1 ELSE NUMBER_TEST(( NVL(NVL(DECODE(UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(DIF_EMPRESA, DIF_ARTICULO, PARAMETROS.V_ALFA(DIF_EMPRESA, 'UNIDAD_MEDIDA_UNIDADES'), 1, PARAMETROS.V_ALFA(DIF_EMPRESA,   'UNIDAD_MEDIDA_GRAMO')), 0,NULL,UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(dif_EMPRESA,dif_articulo, PARAMETROS.V_ALFA(DIF_EMPRESA,'UNIDAD_MEDIDA_UNIDADES'), 1,PARAMETROS.V_ALFA(dif_EMPRESA,'UNIDAD_MEDIDA_GRAMO'))),UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(DIF_EMPRESA,DIF_ARTICULO,PARAMETROS.V_ALFA(DIF_EMPRESA, 'UNIDAD_MEDIDA_UNIDADES'),     1, PARAMETROS.V_ALFA(DIF_EMPRESA, 'UNIDAD_MEDIDA_KILOS')) ), 1)) )end ), 3) END AS \"Cantidad Kg\"  " +
",DIF_IMPORTE_ESTADISTICA AS \"Importe\"" +
",(SELECT SUBSTR(CUF_DENOMINACION, 0, INSTR(CUF_DENOMINACION, '-')-1)  FROM PR_PRODUCTO  INNER JOIN PR_CU_FAMILIA ON CUF_CODIGO_FAMILIA=PRO_CLAVE_ESTAD_PRINCIPAL AND PRO_EMPRESA=CUF_EMPRESA WHERE PRO_CODIGO_PRODUCTO=DIF_ARTICULO AND PRO_EMPRESA=1) AS \"Familia\" " +
",(SELECT SUBSTR(SUBSTR(CUF_DENOMINACION, INSTR(CUF_DENOMINACION, ' - ')+2) , 0, INSTR(SUBSTR(CUF_DENOMINACION, INSTR(CUF_DENOMINACION, ' - ')+2) , ' - ')-1)  FROM PR_PRODUCTO  INNER JOIN PR_CU_FAMILIA ON CUF_CODIGO_FAMILIA=PRO_CLAVE_ESTAD_PRINCIPAL AND PRO_EMPRESA=CUF_EMPRESA WHERE PRO_CODIGO_PRODUCTO=DIF_ARTICULO AND PRO_EMPRESA=1)AS \"Clasificación\"   " +
"" +
"" +
" FROM DETALLE_IMPRESO_FRA_CLTE " +
",CABECERA_IMPRESO_FRA_CLTE" +
"" +
" WHERE  dif_empresa IN (1,3)  AND cif_empresa IN (1,3)  AND dif_ejercicio>=2017 AND cif_ejercicio>=2017 AND CIF_CONFECCIONADA ='S'" +
" AND DIF_EMPRESA=CIF_EMPRESA AND Dif_ejercicio=cif_ejercicio   AND DIF_CONTABILIDAD = CIF_CONTABILIDAD" +
" AND DIF_EJERCICIO= cIF_EJERCICIO AND DIF_NUMERO_FACTURA= CIF_NUMERO_FACTURA" +
" and CIF_FECHA_FACTURA between  to_date('" + desde + @"', 'dd/mm/yyyy') AND  to_date('" + hasta + @"', 'dd/mm/yyyy')";
            Expert con = new Expert();
            DataTable datos = con.Sql_Datatable(sql);
           
            return datos;
        

        }
        protected void Btexport_Click(object sender, EventArgs e)
        {
            string F_desde = datepicker1.Value;
            string F_hasta = datepicker2.Value;
            DataTable facturacion_expert = Facturacion_expert(F_desde.Replace("-", @"/"), F_hasta.Replace("-", @"/"));
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("Facturas");
                ws1.Cells["A1"].LoadFromDataTable(facturacion_expert, true, OfficeOpenXml.Table.TableStyles.Light16);
                List<string> dateColumns = new List<string>() { "FECHA_FACTURA" };
                FormatWorksheetData(dateColumns, facturacion_expert, ws1);

                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                Response.AppendHeader("content-disposition", "attachment; filename=Informe_Facturacion.xlsx");
                // make sure it is downloaded rather than viewed in the browser window
                // Response.AddHeader("Content-disposition", "attachment; filename=Control_Tienda");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
        }
        }
}