using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;
using OfficeOpenXml;
using System.Diagnostics;

namespace Rinya_Albal_App.Produccion
{
    public partial class Fabricacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          //  if (!this.IsPostBack)
             //   BtBuscar.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(BtBuscar, "Nothing") + ";");

        }
        Quality con = new Quality();
        private static void FormatWorksheetData(List<string> dateColumns,  DataTable table, ExcelWorksheet ws)
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
        DataTable datos_leche_(string desde, string hasta)
        {
            string sql = @";with  ascendientes(ARTICULO,NOMBRE ,KG,UD,SSCC,lote_,AÑO,EMPRESA,DISCRIM_2,fecha,LOTE) as (
SELECT     [DESPIECE PARTIDAS_LIN].Producto as ARTICULO,ARTICULO.Descripción as NOMBRE, [DESPIECE PARTIDAS_LIN].Peso AS KG, [DESPIECE PARTIDAS_LIN].[Piezas generadas] AS UD, [DESPIECE PARTIDAS_LIN].SSCC,[DESPIECE PARTIDAS_LIN].[Nº lote] as lote_
,[DESPIECE PARTIDAS].Año,[DESPIECE PARTIDAS].Empresa,ARTICULO.Discrim_2 as DISCRIM_2,case when CONVERT(time,  [DESPIECE PARTIDAS].hora, 108) between CONVERT(time, '18:00:00', 108)  and CONVERT(time, '23:59:59', 108) then DATEADD(dd,1, [DESPIECE PARTIDAS].hora) else[DESPIECE PARTIDAS].hora end as fecha,[DESPIECE PARTIDAS].[Nº lote] as LOTE
FROM            [DESPIECE PARTIDAS] INNER JOIN
                         [DESPIECE PARTIDAS_LIN] ON [DESPIECE PARTIDAS].Año = [DESPIECE PARTIDAS_LIN].Año AND [DESPIECE PARTIDAS].Empresa = [DESPIECE PARTIDAS_LIN].Empresa AND 
                         [DESPIECE PARTIDAS].[Nº Despiece] = [DESPIECE PARTIDAS_LIN].[Nº Despiece] INNER JOIN
                         ARTICULO ON [DESPIECE PARTIDAS_LIN].Producto = ARTICULO.Artículo
WHERE      
([DESPIECE PARTIDAS].Terminal > 0)  and [DESPIECE PARTIDAS].ARtículo<>628 AND 
                        ( ([DESPIECE PARTIDAS_LIN].[Serie lote] = 'D') OR ARTICULO.Discrim_2='LS')
UNION ALL

select  
dpl.Producto as ARTICULO,ARTICULO.Descripción as NOMBRE, round ((dpl.Peso/dp.Peso)*ascendientes.KG,3) AS KG, dpL.[Piezas generadas] as UD, dpl.SSCC ,null as lote_, dp.Año as AÑO,dp.Empresa,ARTICULO.Discrim_2 as DISCRIM_2
,case when CONVERT(time,  dp.hora, 108) between CONVERT(time, '18:00:00', 108)  and CONVERT(time, '23:59:59', 108) then DATEADD(dd,1, dp.hora) else dp.hora end as fecha,ascendientes.LOTE
			FROM            [DESPIECE PARTIDAS_LIN] as dpL 
			inner join ARTICULO on ARTICULO.Artículo=dpL.producto
			
			INNER JOIN
                         [DESPIECE PARTIDAS] as dp ON dpL.Año = dp.Año AND dpL.Empresa = dp.Empresa AND 
                         dpL.[Nº Despiece] = dp.[Nº Despiece] and dp.Peso>0
						inner join ascendientes on dpL.Año = ascendientes.Año AND dpL.Empresa = ascendientes.Empresa AND 
                         dp.[Nº lote] = ascendientes.lote_ and dpl.[Nº lote]<>0 and ascendientes.DISCRIM_2<>'LS'
						   and  ( (dpl.[Serie lote] = 'D') OR ARTICULO.Discrim_2='LS')
						 )


						select AÑO,EMPRESA,LOTE,CONVERT(varchar,FECHA,103) as FECHA, Coalesce([LECHE DE VACA],0) AS [LECHE DE VACA], Coalesce([LECHE DE CABRA],0) AS[LECHE DE CABRA],Coalesce([LECHE DE OVEJA],0)[LECHE DE OVEJA]
						from(
						 select AÑO,EMPRESA,LOTE,fecha,NOMBRE,KG
						 from ascendientes
						  where DISCRIM_2='LS' and fecha between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)
						  ) as source
						  pivot( 
						  SUM(KG)
						  for NOMBRE in ([LECHE DE VACA],[LECHE DE CABRA],[LECHE DE OVEJA])
						  )as pvt";
            return con.Sql_Datatable(sql);

        }
        DataTable datos_produccion_pre(string desde, string hasta) {
            string sql = @"select distinct t.AÑO,t.EMPRESA,LOTE,t.FECHA,ARTICULO,DESCRIPCION,AVG(UD) AS UD ,ROUND (AVG(KG),3) AS KG ,ROUND(AVG(KG_UD),3) AS KG_UD, case when Dp2.Artículo is not null then Dp2.Artículo else Dp3.Artículo end Artículo
, case when Dp2.Artículo is not null then ARTICULO.Descripción else art.Descripción end Descripción
,case when Dp2.Artículo is not null then LOTE else Dp3.[Nº Lote] end LoteFORM
FROM (select 
DP.Año AS AÑO,DP.Empresa AS EMPRESA
,DPL.[Nº Lote] as LOTE, CONVERT(varchar,coalesce (dp.Hora, dp.FEcha) ,103) as FECHA

,dp.Artículo as ARTICULO ,ART2.DESCRIPCIÓN AS DESCRIPCION 
,dp.[Nº despieces] AS UD

,case when ART.[Control exist]='U' then avg(ART.[Factor(Kg/Ud)] )else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end
*dp.[Nº despieces]  AS KG
,case when ART.[Control exist]='U' then AVG(ART.[Factor(Kg/Ud)]) else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end AS KG_UD

 from  (  [DESPIECE PARTIDAS] DP INNER JOIN
                         [DESPIECE PARTIDAS_LIN] DPL ON DP.Año = DPL.Año AND DP.Empresa = DPL.Empresa AND 
                         DP.[Nº Despiece] = DPL.[Nº Despiece] AND DPL.[Piezas generadas]>0  
						  INNER JOIN
                         ARTICULO ART ON DPL.Producto = ART.Artículo AND ( ART.Discrim_4 like'%V%'))
						 inner join 
						 ARTICULO ART2 on dp.Artículo=ART2.Artículo
WHERE       dp.[Nº despieces] >0 and
 dp.Fecha between  convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)
GROUP BY DPL.[Nº Lote],DP.Año,DP.Empresa,dp.[Nº despieces] ,dp.Artículo,ART2.DESCRIPCIÓN,ART.[Control exist]
,dp.Hora, dp.FEcha) T
left join [DESPIECE PARTIDAS] Dp2 on dp2.[Año Lote]=t.AÑO and dp2.[Empresa Lote]= t.EMPRESA and dp2.[Nº Lote]=t.LOTE  and dp2.Usuario='FORM'
left join [DESPIECE PARTIDAS_LIN] dpl on dpl.[Año Lote]=t.AÑO and dpl.[Empresa Lote]= t.EMPRESA and dpl.[Nº Lote]=t.LOTE 
inner join  [DESPIECE PARTIDAS] Dp3 on dp3.[Año Lote]=dpl.AÑO and dp3.[Empresa Lote]= dpl.EMPRESA and dp3.[Nº Despiece]=dpl.[Nº Despiece]  --and dp2.Usuario IS NULL
left join ARTICULO on ARTICULO.Artículo=dp2.Artículo
inner join ARTICULO art on art.Artículo=Dp3.Artículo and   ART.Discrim_4 like'%F%'
GROUP BY t.AÑO,t.EMPRESA,LOTE,t.FECHA,DESCRIPCION,ARTICULO, Dp2.Artículo, ARTICULO.Descripción, dp3.Artículo ,  art.Descripción,dp3.[Nº Lote]";

            return con.Sql_Datatable(sql);

        }
        DataTable datos_produccion_(string desde, string hasta)
        {
            /*string sql = @"SELECT  
DP.Año AS AÑO,DP.Empresa AS EMPRESA
,DPL.[Nº Lote] as LOTE, CONVERT(varchar,[DESPIECE PARTIDAS_LIN].[FechaHora],103) as FECHA
,[DESPIECE PARTIDAS_LIN].PRODUCTO AS ARTICULO,ARTICULO.DESCRIPCIÓN AS DESCRIPCION ,[DESPIECE PARTIDAS_LIN].[Piezas generadas] AS UD, ROUND([DESPIECE PARTIDAS_LIN].[Piezas generadas] * (sum( DPL.Peso) / sum (DPL.[Piezas generadas])),3) AS KG, ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) AS KG_UD --, DPL.Peso/DPL.[Piezas generadas] AS PESO_UNIDAD, DPL.SSCC,

 FROM         (  [DESPIECE PARTIDAS] DP INNER JOIN
                         [DESPIECE PARTIDAS_LIN] DPL ON DP.Año = DPL.Año AND DP.Empresa = DPL.Empresa AND 
                         DP.[Nº Despiece] = DPL.[Nº Despiece] AND DPL.[Piezas generadas]>0  
						  INNER JOIN
                         ARTICULO ART ON DPL.Producto = ART.Artículo AND ( ART.Discrim_4 like'%V%')) 
					 INNER JOIN [DESPIECE PARTIDAS_LIN] ON [DESPIECE PARTIDAS_LIN].[Nº Lote]=DPL.[Nº Lote] AND [DESPIECE PARTIDAS_LIN].AÑO=DPL.AÑO
					 AND [DESPIECE PARTIDAS_LIN].EMPRESA=DP.EMPRESA and DPL.[Piezas generadas]>0 and DPL.Peso>0
					INNER JOIN ARTICULO ON [DESPIECE PARTIDAS_LIN].Producto=ARTICULO.Artículo  AND 
(ARTICULO.discrim_4 like 'PR')
					 
WHERE      
 [DESPIECE PARTIDAS_LIN].[FechaHora] between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)
GROUP BY DPL.[Nº Lote],DP.Año,DP.Empresa,[DESPIECE PARTIDAS_LIN].PRODUCTO,ARTICULO.DESCRIPCIÓN,[DESPIECE PARTIDAS_LIN].[Piezas generadas],[DESPIECE PARTIDAS_LIN].[FechaHora]";
*/
     /*       string sql = @"select 
DP.Año AS AÑO,DP.Empresa AS EMPRESA
,DPL.[Nº Lote] as LOTE, CONVERT(varchar,dp.Fecha,103) as FECHA

,dp.Artículo,ART2.DESCRIPCIÓN AS DESCRIPCION 
,dp.[Nº despieces] AS UD


,case when ART.[Control exist]='U' then ART.[Factor(Kg/Ud)] else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end
*dp.[Nº despieces]  AS KG
,case when ART.[Control exist]='U' then ART.[Factor(Kg/Ud)] else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end AS KG_UD

 from  (  [DESPIECE PARTIDAS] DP INNER JOIN
                         [DESPIECE PARTIDAS_LIN] DPL ON DP.Año = DPL.Año AND DP.Empresa = DPL.Empresa AND 
                         DP.[Nº Despiece] = DPL.[Nº Despiece] AND DPL.[Piezas generadas]>0  
						  INNER JOIN
                         ARTICULO ART ON DPL.Producto = ART.Artículo AND ( ART.Discrim_4 like'%V%'))
						 inner join 
						 ARTICULO ART2 on dp.Artículo=ART2.Artículo
WHERE       dp.[Nº despieces] >0 and
 dp.Fecha between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)
GROUP BY DPL.[Nº Lote],DP.Año,DP.Empresa,dp.[Nº despieces] ,dp.Artículo,ART2.DESCRIPCIÓN,ART.[Control exist],ART.[Factor(Kg/Ud)]
,dp.Fecha";*/

            string sql = @"select dp2.AÑO,dp2.EMPRESA,LOTE,dp2.FECHA,ARTICULO,DESCRIPCION,AVG(UD) AS UD ,ROUND (AVG(KG),3) AS KG ,ROUND(AVG(KG_UD),3) AS KG_UD, Dp2.Artículo, ARTICULO.Descripción
FROM (select 
DP.Año AS AÑO,DP.Empresa AS EMPRESA
,DPL.[Nº Lote] as LOTE, CONVERT(varchar,coalesce (dp.Hora, dp.FEcha) ,103) as FECHA

,dp.Artículo as ARTICULO ,ART2.DESCRIPCIÓN AS DESCRIPCION 
,dp.[Nº despieces] AS UD

,case when ART.[Control exist]='U' then avg(ART.[Factor(Kg/Ud)] )else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end
*dp.[Nº despieces]  AS KG
,case when ART.[Control exist]='U' then AVG(ART.[Factor(Kg/Ud)]) else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end AS KG_UD

 from  (  [DESPIECE PARTIDAS] DP INNER JOIN
                         [DESPIECE PARTIDAS_LIN] DPL ON DP.Año = DPL.Año AND DP.Empresa = DPL.Empresa AND 
                         DP.[Nº Despiece] = DPL.[Nº Despiece] AND DPL.[Piezas generadas]>0  
						  INNER JOIN
                         ARTICULO ART ON DPL.Producto = ART.Artículo AND ( ART.Discrim_4 like'%V%'))
						 inner join 
						 ARTICULO ART2 on dp.Artículo=ART2.Artículo
WHERE       dp.[Nº despieces] >0 and
 dp.Fecha between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)
						
GROUP BY DPL.[Nº Lote],DP.Año,DP.Empresa,dp.[Nº despieces] ,dp.Artículo,ART2.DESCRIPCIÓN,ART.[Control exist]
,dp.Hora, dp.FEcha) T
inner join [DESPIECE PARTIDAS] Dp2 on dp2.[Año Lote]=t.AÑO and dp2.[Empresa Lote]= t.EMPRESA and dp2.[Nº Lote]=t.LOTE and dp2.Usuario='FORM'
left join ARTICULO on ARTICULO.Artículo=dp2.Artículo
GROUP BY dp2.AÑO,dp2.EMPRESA,LOTE,dp2.FECHA,DESCRIPCION,ARTICULO, Dp2.Artículo, ARTICULO.Descripción";

            return con.Sql_Datatable(sql);

        }
        DataTable datos_produccion_Venta(string desde, string hasta)
        {
            string sql= @"select  AÑO,EMPRESA,LOTE,FECHA,ARTICULO,DESCRIPCION,AVG(UD) AS UD ,ROUND (AVG(KG),3) AS KG ,ROUND(AVG(KG_UD),3) AS KG_UD

,ROUND (sum(UD_P_FINAL),3) UD_P_FINAL
,ROUND (sum(KG_P_FINAL),3) KG_P_FINAL

,ROUND (sum(ud_venta),3) UD_VENTA
,ROUND (sum(kg_venta),3) KG_VENTA

,AVG(UD) - ROUND (sum(UD_P_FINAL),3) DIF_UD_PRE_FINAL
,AVG(KG) - ROUND (sum(KG_P_FINAL),3) DIF_KG_PRE_FINAL
,AVG(UD) - ROUND (sum(ud_venta),3) DIF_UD_PRE_VENTA
,AVG(KG) - ROUND (sum(kg_venta),3) DIF_KG_PRE_VENTA
 from (
select 
DP.Año AS AÑO,DP.Empresa AS EMPRESA
,DPL.[Nº Lote] as LOTE, CONVERT(varchar,coalesce (dp.Hora, dp.FEcha) ,103) as FECHA

,dp.Artículo as ARTICULO ,REPLACE(REPLACE(ART2.DESCRIPCIÓN,CHAR(10),''),CHAR(13),'') AS DESCRIPCION 
,(DP.[Nº despieces]) AS UD
,dpl.Producto ,REPLACE(REPLACE(ART.DESCRIPCIÓN,CHAR(10),''),CHAR(13),'') as producto_descrip

,case when ART.[Control exist]='U' then avg(ART.[Factor(Kg/Ud)] )else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end
*DP.[Nº despieces]  AS KG
,case when ART.[Control exist]='U' then AVG(ART.[Factor(Kg/Ud)]) else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end AS KG_UD
,DPL.[Piezas generadas] AS UD_P_FINAL
,case when ART.[Control exist]='U' then avg(ART.[Factor(Kg/Ud)] )else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end
*DPL.[Piezas generadas]  AS KG_P_FINAL
,case when ART.[Control exist]='U' then avg(ART.[Factor(Kg/Ud)] )else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end
 AS KG_P_FINAL_prue
,round (sum (ap.Unidades),3) as UD_VENTA
,case when ART.[Control exist]='U' then avg(ART.[Factor(Kg/Ud)] )else
ROUND ( sum( DPL.Peso) / sum (DPL.[Piezas generadas]),3) end * sum (ap.Unidades) as KG_VENTA


 from  (  [DESPIECE PARTIDAS] DP INNER JOIN
                         [DESPIECE PARTIDAS_LIN] DPL ON DP.Año = DPL.Año AND DP.Empresa = DPL.Empresa AND 
                         DP.[Nº Despiece] = DPL.[Nº Despiece] AND DPL.[Piezas generadas]>0  
						  INNER JOIN
                         ARTICULO ART ON DPL.Producto = ART.Artículo AND ( ART.Discrim_4 like'%V%'))
						 inner join 
						 ARTICULO ART2 on dp.Artículo=ART2.Artículo
						 left join 
						 albaran_partida AP on  dp.año=AP.[Año partida] and dp.empresa=ap.[Empresa partida] and DPL.[Nº Lote]=ap.partida and ap.serie<>'X' and dpl.producto = ap.Artículo 
WHERE       dp.[Nº despieces] >0 and
dp.Fecha between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)


GROUP BY DPL.[Nº Lote],DP.Año,DP.Empresa,DP.[Nº despieces]  ,dp.Artículo,ART2.DESCRIPCIÓN,ART.[Control exist],DPL.[Piezas generadas]
,dp.Hora, dp.FEcha,dpl.Producto ,art.DESCRIPCIÓN
) t


GROUP BY AÑO, EMPRESA, LOTE, FECHA, DESCRIPCION, articulo ";
            return con.Sql_Datatable(sql);
        }
        protected void Btexport_Click(object sender, EventArgs e)
        {
            //BtBuscar.Enabled = false;
            DataTable datos_leche = datos_leche_(datepicker1.Text, datepicker2.Text);
            DataTable datos_produccion= datos_produccion_(datepicker1.Text, datepicker2.Text);
            DataTable datos_produccion_venta = datos_produccion_Venta(datepicker1.Text, datepicker2.Text);
            DataTable  datos_produccion_pre_= datos_produccion_pre(datepicker1.Text, datepicker2.Text);

            if (datos_leche.Rows.Count > 0 && datos_produccion.Rows.Count > 0)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    datos_leche.TableName = "LECHE";
                    datos_produccion.TableName = "PRODUCCION";
                    datos_produccion_pre_.TableName = "PRODUCCION_PRE";
                    datos_produccion_venta.TableName = "PR_VENTA";
                    List<string> dateColumns = new List<string>() { "FECHA" };
                    ExcelWorksheet LT = pck.Workbook.Worksheets.Add("Leche");
                    ExcelWorksheet PR = pck.Workbook.Worksheets.Add("Produccion");
                    ExcelWorksheet PRo = pck.Workbook.Worksheets.Add("Produccion_pre");
                    ExcelWorksheet PR_VE = pck.Workbook.Worksheets.Add("Produccion_Venta");
                    LT.Cells["A1"].LoadFromDataTable(datos_leche, true, OfficeOpenXml.Table.TableStyles.Medium6);
                    FormatWorksheetData(dateColumns, datos_leche, LT);
                    PR.Cells["A1"].LoadFromDataTable(datos_produccion, true, OfficeOpenXml.Table.TableStyles.Medium2);
                    FormatWorksheetData(dateColumns, datos_produccion, PR);
                    PRo.Cells["A1"].LoadFromDataTable(datos_produccion_pre_, true, OfficeOpenXml.Table.TableStyles.Medium2);
                    FormatWorksheetData(dateColumns, datos_produccion_pre_, PRo);
                    PR_VE.Cells["A1"].LoadFromDataTable(datos_produccion_venta, true, OfficeOpenXml.Table.TableStyles.Medium12);
                    FormatWorksheetData(dateColumns, datos_produccion_venta, PR_VE);
                    Response.ContentType = "application/vnd.ms-excel";

                    Response.AddHeader("Content-disposition", "attachment; filename=Input_Produccion.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                   // BtBuscar.Style.Add("visibility", "visible"); //--; = true;
                }
            }
        }
    }
 }