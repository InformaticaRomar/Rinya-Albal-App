using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.Data;
using Utiles;

namespace Rinya_Albal_App.Logistica.FotoStock
{
    public partial class Movimientos_Ventas_Compras : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string Formato_fecha(string fecha)
        {
            string pattern = "dd/MM/yyyy HH:mm";
            DateTime a = new DateTime();
            string resultado = "";
            if (DateTime.TryParseExact(fecha, pattern, null,
                                   System.Globalization.DateTimeStyles.None, out a))
            {
                resultado = a.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return resultado;
        }
  /*      string consulta_sql(string fecha1, string fecha2) {
            string sql= @"select Y.articulo, Y.Descipcion, Y.[Control exist],  KG
,UD
,Y.Familia
  from (
select  coalesce(b.Artículo, c.Artículo) as articulo , coalesce (b.Descipcion,c.Descipcion ) as Descipcion,    coalesce (b.[Control exist],c.[Control exist]) as [Control exist] , round ( coalesce (c.KG,0) - coalesce (B.KG,0),2) as KG,   coalesce (c.ud,0) - coalesce (B.ud,0) as UD,
 b.KG as Kg_ventas, c.KG as kg_compras, b.UD as ud_ventas, c.UD as ud_compras ,coalesce ( b.Familia,c.Familia) as Familia
from (

select ARTICULO.Artículo, REPLACE(REPLACE(ARTICULO.Descripción, CHAR(10), ''), CHAR(13), '') Descipcion , [Control exist] 

, case when ARTICULO.[Control exist]='K' then sum( case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X') then -1*(cantidad) else (cantidad) end ) when  ARTICULO.[Control exist]='A' then  sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end )  when  ARTICULO.[Control exist]='U' then  sum(case when (almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end ) * ARTICULO.[Factor(Kg/Ud)] else sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end )  end as KG
,  case when ARTICULO.[Control exist]='U' then sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X') then -1*(cantidad) else (cantidad) end ) when  ARTICULO.[Control exist]='A' then  sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cajas) else (cajas) end )  when  ARTICULO.[Control exist]='K' then  sum(case when (almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end ) / ARTICULO.[Factor(Kg/Ud)] else sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end ) end as UD
,Familia
FROM            ALBARAN_LIN INNER JOIN
                         ALBARAN_CABE ON ALBARAN_LIN.Año = ALBARAN_CABE.Año AND ALBARAN_LIN.Empresa = ALBARAN_CABE.Empresa AND ALBARAN_LIN.Serie = ALBARAN_CABE.Serie AND 
                         ALBARAN_LIN.[Nº Albarán] = ALBARAN_CABE.[Nº Albarán]
inner join ARTICULO on ARTICULO.Artículo=ALBARAN_LIN.Artículo
 where  [Fecha Emisión] + convert (time,FechaCreacionACab,108)  between   convert(datetime, '" + fecha1 + @"',120) and  convert(datetime, '" + fecha2 + @"',120)
 and (ALBARAN_LIN.almacén =6)
 group by ARTICULO.Artículo,ARTICULO.Descripción,[Control exist],
 ARTICULO.[Factor(Kg/Ud)],Familia
  ) B 
 full outer join 
  (select ARTICULO.Artículo, REPLACE(REPLACE(ARTICULO.Descripción, CHAR(10), ''), CHAR(13), '') Descipcion , 
[Control exist] 
,  sum (case when ARTICULO.[Control exist]='K' then cantidad  else case  when  ARTICULO.[Control exist]='A' then   cantidad else case  when  ARTICULO.[Control exist]='U' then    (case when cantidad=0 then cajas else cantidad end) * ARTICULO.[Factor(Kg/Ud)] end end end) as KG
,sum (case when ARTICULO.[Control exist]='U' then case when cantidad=0 then cajas else cantidad  end  when  ARTICULO.[Control exist]='A' then   cajas else case  when  ARTICULO.[Control exist]='K' then  (case when cantidad=0 then cajas else cantidad end) / ARTICULO.[Factor(Kg/Ud)] end end ) as UD
,Familia

FROM            RECEPCION_LIN INNER JOIN
                         RECEPCION_CABE ON RECEPCION_LIN.Año = RECEPCION_CABE.Año AND RECEPCION_LIN.Empresa = RECEPCION_CABE.Empresa AND RECEPCION_LIN.Serie = RECEPCION_CABE.Serie AND 
                         RECEPCION_LIN.[Nº Albarán] = RECEPCION_CABE.[Nº Albarán]
full outer  join ARTICULO on ARTICULO.Artículo=recepcion_lin.Artículo
where RECEPCION_CABE.[Fecha Recepción]+coalesce(RECEPCION_CABE.HoraRecepcion,cast('00:00' as time)) between  convert(datetime, '" + fecha1+ @"',120) and  convert(datetime, '" + fecha2 + @"',120)
and (RECEPCION_LIN.almacén =6 or RECEPCION_LIN.almacén=1)
 group by ARTICULO.Artículo,ARTICULO.Descripción,[Control exist], ARTICULO.[Factor(Kg/Ud)],Familia

 ) c on B.Artículo=c.Artículo ) as Y
 
 order by y.Articulo";
            return sql;
        }*/

        string consulta_sql(string fecha1, string fecha2)
        {
            string sql = @"select  coalesce(b.Artículo, c.Artículo) as articulo , coalesce (b.Descipcion,c.Descipcion ) as Descipcion ,--    coalesce (b.[Control exist],c.[Control exist]) as [Control exist] , 
coalesce ( b.Familia,c.Familia) as Familia, coalesce( b.KG,0) as Kg_ventas, coalesce (c.KG,0) as kg_compras, coalesce(b.UD,0) as ud_ventas, coalesce (c.UD,0) as ud_compras 
,round ( coalesce (c.KG,0) - coalesce (B.KG,0),2) as DIFERENCIA_KG,   coalesce (c.ud,0) - coalesce (B.ud,0) as DIFERENCIA_UD
from (

select ARTICULO.Artículo, REPLACE(REPLACE(ARTICULO.Descripción, CHAR(10), ''), CHAR(13), '') Descipcion , [Control exist] 

, case when ARTICULO.[Control exist]='K' then sum( case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X') then -1*(cantidad) else (cantidad) end ) when  ARTICULO.[Control exist]='A' then  sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end )  when  ARTICULO.[Control exist]='U' then  sum(case when (almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end ) * ARTICULO.[Factor(Kg/Ud)] else sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end )  end as KG
,  case when ARTICULO.[Control exist]='U' then sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X') then -1*(cantidad) else (cantidad) end ) when  ARTICULO.[Control exist]='A' then  sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cajas) else (cajas) end )  when  ARTICULO.[Control exist]='K' then  sum(case when (almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end ) / ARTICULO.[Factor(Kg/Ud)] else sum(case when (ALBARAN_LIN.almacén=1 and ALBARAN_LIN.Serie='X')  then -1*(cantidad) else (cantidad) end ) end as UD
,Familia	
FROM            ALBARAN_LIN INNER JOIN
                         ALBARAN_CABE ON ALBARAN_LIN.Año = ALBARAN_CABE.Año AND ALBARAN_LIN.Empresa = ALBARAN_CABE.Empresa AND ALBARAN_LIN.Serie = ALBARAN_CABE.Serie AND 
                         ALBARAN_LIN.[Nº Albarán] = ALBARAN_CABE.[Nº Albarán]
inner join ARTICULO on ARTICULO.Artículo=ALBARAN_LIN.Artículo
 where  [Fecha Emisión] + convert (time,FechaCreacionACab,108)  between   convert(datetime, '" + fecha1 + @"',120) and  convert(datetime, '" + fecha2 + @"',120)
 and (ALBARAN_LIN.almacén =6)
 group by ARTICULO.Artículo,ARTICULO.Descripción,[Control exist],
 ARTICULO.[Factor(Kg/Ud)],Familia
  ) B 
 full outer join 
  (select ARTICULO.Artículo, REPLACE(REPLACE(ARTICULO.Descripción, CHAR(10), ''), CHAR(13), '') Descipcion , 
[Control exist] 
,  sum (case when ARTICULO.[Control exist]='K' then cantidad  else case  when  ARTICULO.[Control exist]='A' then   cantidad else case  when  ARTICULO.[Control exist]='U' then    (case when cantidad=0 then cajas else cantidad end) * ARTICULO.[Factor(Kg/Ud)] end end end) as KG
,sum (case when ARTICULO.[Control exist]='U' then case when cantidad=0 then cajas else cantidad  end  when  ARTICULO.[Control exist]='A' then   cajas else case  when  ARTICULO.[Control exist]='K' then  (case when cantidad=0 then cajas else cantidad end) / ARTICULO.[Factor(Kg/Ud)] end end ) as UD
,Familia

FROM            RECEPCION_LIN INNER JOIN
                         RECEPCION_CABE ON RECEPCION_LIN.Año = RECEPCION_CABE.Año AND RECEPCION_LIN.Empresa = RECEPCION_CABE.Empresa AND RECEPCION_LIN.Serie = RECEPCION_CABE.Serie AND 
                         RECEPCION_LIN.[Nº Albarán] = RECEPCION_CABE.[Nº Albarán]
full outer  join ARTICULO on ARTICULO.Artículo=recepcion_lin.Artículo
where RECEPCION_CABE.[Fecha Recepción]+coalesce(RECEPCION_CABE.HoraRecepcion,cast('00:00' as time)) between  convert(datetime, '" + fecha1 + @"',120) and  convert(datetime, '" + fecha2 + @"',120)
and (RECEPCION_LIN.almacén =6 or RECEPCION_LIN.almacén=1)
 group by ARTICULO.Artículo,ARTICULO.Descripción,[Control exist], ARTICULO.[Factor(Kg/Ud)],Familia

 ) c on B.Artículo=c.Artículo
 order by articulo";
            return sql;
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
            string F_desde = Formato_fecha(datepicker1.Value);
            string F_hasta = Formato_fecha(datepicker2.Value);
           if (F_desde.Length>0 && F_hasta.Length > 0) { 

            Quality con = new Quality();
            List<string> hideColumns = new List<string>() {

                "orden"
            };
            using (ExcelPackage pck = new ExcelPackage())
            {
                    string sql = consulta_sql(F_desde, F_hasta);
                DataTable table = con.Sql_Datatable(sql);
                    try
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                        ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                        FormatWorksheetData(hideColumns, table, ws);

                        // make sure it is sent as a XLSX file
                        Response.ContentType = "application/vnd.ms-excel";
                        // make sure it is downloaded rather than viewed in the browser window
                        Response.AddHeader("Content-disposition", "attachment; filename=compras-ventas.xlsx");
                        Response.BinaryWrite(pck.GetAsByteArray());
                        Response.End();
                    }
                    catch (Exception ) { }
            }
            }

            

        }
    }
}