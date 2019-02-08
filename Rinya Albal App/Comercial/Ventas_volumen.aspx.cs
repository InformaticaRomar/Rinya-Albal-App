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
    public partial class Ventas_volumen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        DataTable Cierre_exper_old(string desde, string hasta)
        {
            string sql = "     select dpc_empresa \"Empresa\", dpc_cliente \"Codigo\", DIC_RAZON_SOCIAL \"Nom. Cliente\" , " +
 "DPC_SERIE_FACTURA \"Serie Factura\", " +
 "DPC_SECUENCIA_FACTURA \"Num.Fra\" " +
 ", CPM_SUCURSAL_ENVIO \"Sucursal\", " +
 "DPC_ARTICULO \"Articulo\", " +
 "DPC_DENOMINACION_ARTICULO \"Desc.artículo\", " +
 "(SELECT " +
 "SUBSTR(CUF_DENOMINACION, 0, INSTR(CUF_DENOMINACION, '-')-1)  " +
 "FROM PR_PRODUCTO  " +
 "INNER JOIN PR_CU_FAMILIA ON CUF_CODIGO_FAMILIA=PRO_CLAVE_ESTAD_PRINCIPAL AND PRO_EMPRESA=CUF_EMPRESA " +
 "WHERE PRO_CODIGO_PRODUCTO=DPC_ARTICULO AND PRO_EMPRESA=1) AS \"Familia\" " +
 ",  " +
 "(SELECT " +
 "SUBSTR(SUBSTR(CUF_DENOMINACION, INSTR(CUF_DENOMINACION, ' - ')+2) , 0, INSTR(SUBSTR(CUF_DENOMINACION, INSTR(CUF_DENOMINACION, ' - ')+2) , ' - ')-1)  " +
 "FROM PR_PRODUCTO  " +
 "INNER JOIN PR_CU_FAMILIA ON CUF_CODIGO_FAMILIA=PRO_CLAVE_ESTAD_PRINCIPAL AND PRO_EMPRESA=CUF_EMPRESA " +
 "WHERE PRO_CODIGO_PRODUCTO=DPC_ARTICULO AND PRO_EMPRESA=1) AS \"Clasificación\" " +
 ", " +
 " " +
 "     case when (select NVL(MAX(REPLACE(PCA_VALOR_CARACTERISTICA, " +
 "                               '\"', " +
 "                               '')), " +
 "                   'N') FROM PR_CU_CARAC_PRODUCTO " +
 "         WHERE PCA_EMPRESA = DPC_EMPRESA AND " +
 "               PCA_CODIGO_PRODUCTO = DPC_ARTICULO AND " +
 "               PCA_CODIGO_CARACTERISTICA = 'PESO_FIJO' )=0 then 1 else " +
 "   tO_NUMBER(( NVL(NVL(DECODE(UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(DPC_EMPRESA, " +
 "                                                               DPC_ARTICULO, " +
 "                                                               PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                                 'UNIDAD_MEDIDA_UNIDADES'), " +
 "                                                               1, " +
 "                                                               PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                                 'UNIDAD_MEDIDA_GRAMO')), " +
 "                      0, " +
 "                      NULL, " +
 "                      UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(DPC_EMPRESA, " +
 "                                                               DPC_ARTICULO, " +
 "                                                               PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                                 'UNIDAD_MEDIDA_UNIDADES'), " +
 "                                                               1, " +
 "                                                               PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                                 'UNIDAD_MEDIDA_GRAMO'))), " +
 "               UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(DPC_EMPRESA, " +
 "                                                        DPC_ARTICULO, " +
 "                                                        PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                          'UNIDAD_MEDIDA_UNIDADES'), " +
 "                                                        1, " +
 "                                                        PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                          'UNIDAD_MEDIDA_KILOS')) ), " +
 "           1)) )end                                                                                  PESO " +
 ", " +
 " NVL(SUBSTR(CARACTERISTICAS.EVALUACION(DIC_EMPRESA            , " +
 "                                             DIC_SECUENCIA_PLANTILLA, " +
 "                                             'DISCRIM_3'             ) " +
 "                                             , 0, 2),'**')        Ruta, " +
 "                                              " +
 "                                             DPC_REPRESENTANTE Comercial, " +
 "   DPC_UMV as \"UMV\", " +
 "   DPC_CANTIDAD as \"Cant.UMV\" " +
 "   , " +
 "   DPC_CANTIDAD *  " +
 "     case when (select NVL(MAX(REPLACE(PCA_VALOR_CARACTERISTICA, " +
 "                               '\"', " +
 "                               '')), " +
 "                   'N') FROM PR_CU_CARAC_PRODUCTO " +
 "         WHERE PCA_EMPRESA = DPC_EMPRESA AND " +
 "               PCA_CODIGO_PRODUCTO = DPC_ARTICULO AND " +
 "               PCA_CODIGO_CARACTERISTICA = 'PESO_FIJO' )=0 then 1 else " +
 "   tO_NUMBER(( NVL(NVL(DECODE(UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(DPC_EMPRESA, " +
 "                                                               DPC_ARTICULO, " +
 "                                                               PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                                 'UNIDAD_MEDIDA_UNIDADES'), " +
 "                                                               1, " +
 "                                                               PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                                 'UNIDAD_MEDIDA_GRAMO')), " +
 "                      0, " +
 "                      NULL, " +
 "                      UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(DPC_EMPRESA, " +
 "                                                               DPC_ARTICULO, " +
 "                                                               PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                                 'UNIDAD_MEDIDA_UNIDADES'), " +
 "                                                               1, " +
 "                                                               PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                                 'UNIDAD_MEDIDA_GRAMO'))), " +
 "               UTILIDADES.CONVERTIR_UNIDADES_PRODUCTO_F(DPC_EMPRESA, " +
 "                                                        DPC_ARTICULO, " +
 "                                                        PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                          'UNIDAD_MEDIDA_UNIDADES'), " +
 "                                                        1, " +
 "                                                        PARAMETROS.V_ALFA(DPC_EMPRESA, " +
 "                                                                          'UNIDAD_MEDIDA_KILOS')) ), " +
 "           1)) )end  \"Cantidad Kg\" " +
 "                                                                                   " +
 "                                                                                  , " +
 "                                                                                   " +
 "                                                                              TO_CHAR(DPC_FECHA_FACTURA, 'WW') AS SEMANA, " +
 "                                                                               " +
 " to_char(to_date(to_char( EXTRACT(MONTH from DPC_FECHA_FACTURA)),'mm'),'Month','NLS_DATE_LANGUAGE = SPANISH') " +
 "Mes " +
 ",TO_CHAR(DPC_FECHA_FACTURA, 'YYYY') AS Año " +
 ", DPC_FECHA_FACTURA as \"F. Factura\" " +
 ",DPC_TARIFA as \"Tarifa\" " +
 ",DPC_PRECIO_TARIFA as \"Precio Tarifa\" " +
 ",DPC_TIPO_DTO as \"Dto\" " +
 ",((Cpm_Tipo_dto_rappel_fra /100) * DPC_PRECIO_COMERCIAL) +DPC_PRECIO_COMERCIAL  \"Precio Fra\" " +
 ",0 \"DTO PP\" " +
 ",((Cpm_Tipo_dto_rappel_fra /100) * DPC_PRECIO_COMERCIAL) +DPC_PRECIO_COMERCIAL \"Precio Net PP\" " +
 ",(Cpm_Tipo_dto_rappel_fra /100) \"Dto pie\" " +
 ",DPC_PRECIO_COMERCIAL \"Precio Neto Neto\" " +
 ",DPC_BRUTO_COMISION  \"Importe final\" " +
 ",DPC_PRECIO_LIMITE	\"Precio Limite\" " +
 " " +
 " " +
 " " +
 "from CUERPO_PEDIDO_CLIENTE  " +
 "inner join CABECERA_PEDIDO_CLIENTE on CPM_EMPRESA            = DPC_EMPRESA         AND " +
 "       CPM_CONTABILIDAD       = DPC_CONTABILIDAD    AND " +
 "       CPM_CLIENTE            = DPC_CLIENTE         AND " +
 "       CPM_NUMERO_PEDIDO      = DPC_NUMERO_PEDIDO " +
 "       inner join DIRECCION_CLIENTE ON  DIRECCION_CLIENTE.DIC_CLIENTE =DPC_CLIENTE AND DIRECCION_CLIENTE.DIC_SUCURSAL=CPM_SUCURSAL_ENVIO " +
 "       AND DIRECCION_CLIENTE.DIC_EMPRESA=DPC_EMPRESA " +
 "        " +
 "where dpc_empresa=1"
 + "and DPC_FECHA_FACTURA between to_date('" + desde + @"') and to_date('" + hasta + @"')";
            Expert con = new Expert();
            DataTable datos = con.Sql_Datatable(sql);
            return datos;
        }

        DataTable Cierre_expert(string desde, string hasta)
        {
            string sql = "select * from V_VENTAS_VOL where  \"F. Factura\" between to_date('" + desde + @"') and to_date('" + hasta + @"')";
            Expert con = new Expert();
        DataTable datos = con.Sql_Datatable(sql);
            return datos;
        }

    protected void Btexport_Click(object sender, EventArgs e)
        {
            string F_desde = datepicker1.Value;
            string F_hasta = datepicker2.Value;
            DataTable Expert = Cierre_expert(F_desde.Replace("-", @"/"), F_hasta.Replace("-", @"/"));
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(Expert, true, OfficeOpenXml.Table.TableStyles.Light13);
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("€ por cliente");

                var dataRange = ws.Cells[ws.Dimension.Address.ToString()];
                dataRange.AutoFitColumns();
                var pivotTable = ws2.PivotTables.Add(ws2.Cells["A3"], dataRange, "€xcliente");
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
                pivotTable.FirstDataCol = 1;
                pivotTable.RowHeaderCaption = "Empresa";

                var EmpresaField = pivotTable.Fields["Empresa"];
                pivotTable.PageFields.Add(EmpresaField);
                EmpresaField.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                var AnyoField = pivotTable.Fields["Año"];
                pivotTable.PageFields.Add(AnyoField);
                AnyoField.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;
                
                var ComercialField = pivotTable.Fields["Comercial"];
                pivotTable.PageFields.Add(ComercialField);
                ComercialField.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

            
                var Importe_Field = pivotTable.Fields["Importe final"];
                pivotTable.DataFields.Add(Importe_Field);

                var MesField = pivotTable.Fields["Mes"];
                                pivotTable.ColumnFields.Add(MesField);
                var ClienteDataField = pivotTable.Fields["Nom. Cliente"];
                pivotTable.RowFields.Add(ClienteDataField);
                var FamiliaField = pivotTable.Fields["Familia"];
                pivotTable.RowFields.Add(FamiliaField);
                var ArticuloField = pivotTable.Fields["Articulo"];
                pivotTable.RowFields.Add(ArticuloField);
                var DesArticuloField = pivotTable.Fields["Desc.artículo"];
                pivotTable.RowFields.Add(DesArticuloField);

                ExcelWorksheet ws3 = pck.Workbook.Worksheets.Add("Kg por cliente");
                var pivotTable2 = ws3.PivotTables.Add(ws3.Cells["A3"], dataRange, "kgxcliente");
                pivotTable2.MultipleFieldFilters = true;
                pivotTable2.RowGrandTotals = true;
                pivotTable2.ColumGrandTotals = false;
                pivotTable2.Compact = true;
                pivotTable2.CompactData = true;
                pivotTable2.GridDropZones = false;
                pivotTable2.Outline = false;
                pivotTable2.OutlineData = false;
                pivotTable2.ShowError = true;
                pivotTable2.ErrorCaption = "[error]";
                pivotTable2.ShowHeaders = true;
                pivotTable2.UseAutoFormatting = true;
                pivotTable2.ApplyWidthHeightFormats = true;
                pivotTable2.ShowDrill = true;
                pivotTable2.FirstDataCol = 1;
                pivotTable2.RowHeaderCaption = "Empresa";

                var EmpresaField2 = pivotTable2.Fields["Empresa"];
                pivotTable2.PageFields.Add(EmpresaField2);
                EmpresaField2.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                var anyoField2 = pivotTable2.Fields["Año"];
                pivotTable2.PageFields.Add(anyoField2);
                anyoField2.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                var ComercialField2 = pivotTable2.Fields["COMERCIAL"];
                pivotTable2.PageFields.Add(ComercialField2);
                anyoField2.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                var Cantidad_Field = pivotTable2.Fields["Cantidad Kg"];
                pivotTable2.DataFields.Add(Cantidad_Field);

                var MesField2 = pivotTable2.Fields["Mes"];
                pivotTable2.ColumnFields.Add(MesField2);

                var Cod_Cliente_Field = pivotTable2.Fields["Codigo"];
                pivotTable2.RowFields.Add(Cod_Cliente_Field);

                var Cliente_Field = pivotTable2.Fields["Nom. Cliente"];
                pivotTable2.RowFields.Add(Cliente_Field);
                var Des_ArticuloField = pivotTable2.Fields["Desc.artículo"];
                pivotTable2.RowFields.Add(Des_ArticuloField);
                var Clasificacion_Field = pivotTable2.Fields["Familia"];
                pivotTable2.RowFields.Add(Clasificacion_Field);
                var Familia_Field = pivotTable2.Fields["Clasificación"];
                pivotTable2.RowFields.Add(Familia_Field);


                ExcelWorksheet ws4 = pck.Workbook.Worksheets.Add("Kg por Comercial");
                var pivotTable3 = ws4.PivotTables.Add(ws4.Cells["A3"], dataRange, "kgcomercial");
                pivotTable3.MultipleFieldFilters = true;
                pivotTable3.RowGrandTotals = true;
                pivotTable3.ColumGrandTotals = false;
                pivotTable3.Compact = true;
                pivotTable3.CompactData = true;
                pivotTable3.GridDropZones = false;
                pivotTable3.Outline = false;
                pivotTable3.OutlineData = false;
                pivotTable3.ShowError = true;
                pivotTable3.ErrorCaption = "[error]";
                pivotTable3.ShowHeaders = true;
                pivotTable3.UseAutoFormatting = true;
                pivotTable3.ApplyWidthHeightFormats = true;
                pivotTable3.ShowDrill = true;
                pivotTable3.FirstDataCol = 1;
                pivotTable3.RowHeaderCaption = "Empresa";

                var EmpresaField3 = pivotTable3.Fields["Empresa"];
                pivotTable3.PageFields.Add(EmpresaField3);
                EmpresaField2.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                var ClienteField3 = pivotTable3.Fields["Nom. Cliente"];
                pivotTable3.PageFields.Add(ClienteField3);
                ClienteField3.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                var anyoField3 = pivotTable3.Fields["Año"];
                pivotTable3.PageFields.Add(anyoField3);
                anyoField3.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

                

                var Cantidad_Field2 = pivotTable3.Fields["Cantidad Kg"];
                pivotTable3.DataFields.Add(Cantidad_Field2);

                var MesField3 = pivotTable3.Fields["Mes"];
                pivotTable3.ColumnFields.Add(MesField3);

                var Comercial_Field = pivotTable3.Fields["COMERCIAL"];
                pivotTable3.RowFields.Add(Comercial_Field);

                var Familia_Field2 = pivotTable3.Fields["Clasificación"];
                pivotTable3.RowFields.Add(Familia_Field);

                var Clasificacion_Field2 = pivotTable3.Fields["Familia"];
                pivotTable3.RowFields.Add(Clasificacion_Field);

                var Atr_Field2 = pivotTable3.Fields["Articulo"];
                pivotTable3.RowFields.Add(Atr_Field2);
                var nomAtr_Field2 = pivotTable3.Fields["Desc.artículo"];
                pivotTable3.RowFields.Add(nomAtr_Field2);

                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Ventas_volumen.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
           
        }
    }
}