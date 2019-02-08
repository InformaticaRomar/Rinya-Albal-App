using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using OfficeOpenXml;

namespace Rinya_Albal_App.Comercial.Tienda
{
    public partial class Comprobacion_Tickets : System.Web.UI.Page
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
                catch {  }
            }
            // get all data and autofit
            r = ws.Cells[1, 1, rowCount + 1, columnCount-1];
            r.AutoFitColumns();


        }


        DataTable cierre_tienda(string desde, string hasta)
        {
            string sql = @"select  cast (day(DATENEW) as varchar(2) )+'/'+cast(month(DATENEW) as varchar(2))+'/' + cast(year(DATENEW) as varchar(4))  Fecha_Cierre 
--,PAYMENT 
,round( sum(case when PAYMENT ='cash' then units*price  else 0 end ),2)as Contado
, round( sum(case when PAYMENT ='debt' then units*price  else 0 end ),2) as [A cuenta],
round( sum(case when PAYMENT ='magcard' then units*price  else 0 end),2) as [Tarjeta] 
, Round (sum(units*price),2) Total
from (
select distinct  DATENEW, ticketid,line,product,units,price,PAYMENT FROM OPENQUERY(UNICENTA, ' SELECT  py.`PAYMENT`, r1.DATENEW,t1.ticketid,tl1.line,tl1.product ,round((case when isnull(`tl1`.`PRODUCT`) then ((round(((`tl1`.`PRICE` * 100) / (select `tl2`.`PRICE` from `ticketlines` `tl2` where ((`tl2`.`TICKET` = `tl1`.`TICKET`) and (`tl2`.`LINE` = (`tl1`.`LINE` - 1))))),0) * `tl1`.`UNITS`) / 100) else `tl1`.`UNITS` end),3) AS `UNITS`
 ,
 round((case when isnull(`tl1`.`PRODUCT`) then (select `tl2`.`PRICE` from `ticketlines` `tl2` 
 where ((`tl2`.`TICKET` = `tl1`.`TICKET`) and (`tl2`.`LINE` = (`tl1`.`LINE` - 1)))) else `tl1`.`PRICE` end),3) AS `PRICE`
 FROM `tickets` t1, `ticketlines` tl1, `receipts` r1,`payments` py 
 where t1.ID = tl1.Ticket and r1.ID=t1.ID and py.`RECEIPT`=r1.ID
and r1.DATENEW between  STR_TO_DATE(''" + desde + @"'', ''%d/%m/%Y'') and STR_TO_DATE(''" + hasta + @"'', ''%d/%m/%Y'')
ORDER BY `TICKETID` DESC  
 ') ) t 
 group by datepart(year,DATENEW), 
    datepart(month,DATENEW), 
    datepart(day,DATENEW) ";
                /* @"select  cast (day(DATENEW) as varchar(2) )+'/'+cast(month(DATENEW) as varchar(2))+'/' + cast(year(DATENEW) as varchar(4))  Fecha_Cierre, Round (sum(units*price),2) dinero
from (
select distinct  DATENEW, ticketid,product,units,price FROM OPENQUERY(UNICENTA, 'SELECT  receipts.DATENEW,`tickets`.ticketid,`ticketlines`.product ,ticketlines.units , `ticketlines`.price    FROM `tickets`, `ticketlines`, `receipts` where `tickets`.ID = `ticketlines`.Ticket and receipts.ID=`tickets`.ID
and `tickets`.ticketid>=233 and receipts.DATENEW between  STR_TO_DATE(''" + desde + @"'', ''%d/%m/%Y'') and STR_TO_DATE(''" + hasta + @"'', ''%d/%m/%Y'')
ORDER BY `TICKETID` DESC  
 ') ) t 
 group by datepart(year,DATENEW), 
    datepart(month,DATENEW), 
    datepart(day,DATENEW)	";*/
            Quality con = new Quality();

            return con.Sql_Datatable(sql);
        }
        DataTable cierre_quality(string desde, string hasta)
        {
            string sql = @"select  cast (day([Fecha Emisión]) as varchar(2) )+'/'+cast(month([Fecha Emisión]) as varchar(2))+'/' + cast(year([Fecha Emisión]) as varchar(4))  

as Fecha_Cierre, round (Sum(total), 2) as Importe
from (select ALBARAN_CABE.[Fecha Emisión] , ALBARAN_LIN.Año,ALBARAN_LIN.Empresa,ALBARAN_LIN.Serie,ALBARAN_LIN.[Nº Albarán],[Nº linea Albarán], Artículo, sum(cantidad)*[Precio Venta] total 
from ALBARAN_LIN inner join ALBARAN_CABE on ALBARAN_LIN.Año=ALBARAN_CABE.año and ALBARAN_LIN.Empresa= ALBARAN_CABE.Empresa and ALBARAN_LIN.serie= ALBARAN_CABE.serie and ALBARAN_LIN.[Nº Albarán]=ALBARAN_CABE.[Nº Albarán]
  where ALBARAN_LIN.serie='C' and ALBARAN_CABE.[Fecha Emisión] 
  between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)  
group by ALBARAN_LIN.Año,ALBARAN_LIN.Empresa,ALBARAN_LIN.Serie,ALBARAN_LIN.[Nº Albarán], Artículo,[Nº linea Albarán],[Precio Venta],ALBARAN_CABE.[Fecha Emisión]

)  t

group by datepart(year,[Fecha Emisión]), 
    datepart(month,[Fecha Emisión]), 
    datepart(day,[Fecha Emisión])	";
            Quality con = new Quality();

            return con.Sql_Datatable(sql);
        }
        DataTable Ticket_Dinero(string desde, string hasta)
        {
            string sql = @"select ticketid ticket, Round (sum(units*price),2) Importe
from (
select distinct ticketid,product,units,price FROM OPENQUERY(UNICENTA, 'SELECT `tickets`.ticketid,`ticketlines`.product ,ticketlines.units , `ticketlines`.price    FROM `tickets`, `ticketlines`, `receipts` where `tickets`.ID = `ticketlines`.Ticket and receipts.ID=`tickets`.ID
and `tickets`.ticketid>=233 and receipts.DATENEW between  STR_TO_DATE(''"+desde+ @"'', ''%d/%m/%Y'') and STR_TO_DATE(''" + hasta + @"'', ''%d/%m/%Y'')
ORDER BY `TICKETID` DESC  
 ') ) t 
 group by ticketid	";
            Quality con = new Quality();

            return con.Sql_Datatable(sql);
        }

        DataTable Albaran_dinero(string desde, string hasta)
        {
            string sql = @"select [Nº Albarán]-100000 as ticket, round (Sum(total), 2) as Importe
from (select ALBARAN_LIN.Año,ALBARAN_LIN.Empresa,ALBARAN_LIN.Serie,ALBARAN_LIN.[Nº Albarán],[Nº linea Albarán], Artículo, sum(cantidad)*[Precio Venta] total 
from ALBARAN_LIN inner join ALBARAN_CABE on ALBARAN_LIN.Año=ALBARAN_CABE.año and ALBARAN_LIN.Empresa= ALBARAN_CABE.Empresa and ALBARAN_LIN.serie= ALBARAN_CABE.serie and ALBARAN_LIN.[Nº Albarán]=ALBARAN_CABE.[Nº Albarán]
  where ALBARAN_LIN.serie='C' and ALBARAN_CABE.[Fecha Emisión] between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)  
group by ALBARAN_LIN.Año,ALBARAN_LIN.Empresa,ALBARAN_LIN.Serie,ALBARAN_LIN.[Nº Albarán], Artículo,[Nº linea Albarán],[Precio Venta]

)  t


group by [Nº Albarán]	";
            Quality con = new Quality();

            return con.Sql_Datatable(sql);
        }
        DataTable Cierre_expert_old(string desde, string hasta)
        {
           /* string sql = @" select to_char(AEN_FECHA_ALBARAN,'dd/mm/yyyy') Fecha_Cierre, round (SUM(t.Importe),2) Importe
from 
(select  AEN_FECHA_ALBARAN,dpc_ejercicio_lpr  as anyo,DPC_EMPRESA as empresa, DPC_NUMERO_ALBARAN as nAlbaran, DPC_ARTICULO as articulo,sum(DPC_CANTIDAD_UMV) as Cantidad, sum(DPC_PRECIO_ARTICULO) precio,  sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) Importe
from CUERPO_PEDIDO_CLIENTE ,
ALBARAN_ENVIO, DIRECCION_CLIENTE
 where dpc_empresa=1
 and DPC_CANTIDAD_UMV>0 and 
 dpc_empresa=aen_empresa
 and DPC_NUMERO_ALBARAN=ALBARAN_ENVIO.AEN_NUMERO_ALBARAN and
 dpc_ejercicio_lpr=ALBARAN_ENVIO.AEN_EJERCICIO and
 dic_empresa=dpc_empresa and DIC_cliente=ALBARAN_ENVIO.AEN_CLIENTE and
 DIRECCION_CLIENTE.DIC_SUCURSAL =ALBARAN_ENVIO.AEN_SUCURSAL_ENVIO and
 DIC_GRUPO_TARIFA_ALTERN='600' and
      dpc_contabilidad=0 and
      dpc_ejercicio_lpr>=2017 
      and ALBARAN_ENVIO.AEN_FECHA_ALBARAN between to_date('" + desde + @"') and to_date('" + hasta + @"')
      and DPC_NUMERO_ALBARAN between 100000 and 300000
            GROUP BY dpc_ejercicio_lpr, DPC_EMPRESA,DPC_NUMERO_ALBARAN, DPC_ARTICULO,AEN_FECHA_ALBARAN ) t 
            group by t.AEN_FECHA_ALBARAN";*/
            string sql = @"
select to_char(FECHA,'dd/mm/yyyy') Fecha_Cierre, round (SUM(IMPORTE),2) IMPORTE, round (SUM(IMPORTE_IVA),2) Importe_IVA
from 
(
select FECHA , anyo, EMPRESA,NALBARAN,ARTICULO,CANTIDAD, PRECIO, IMPORTE, V_IVA,round (IMPORTE +V_IVA,2) as IMPORTE_IVA  from (
select  AEN_FECHA_ALBARAN fecha ,dpc_ejercicio_lpr  as anyo,DPC_EMPRESA as empresa, DPC_NUMERO_ALBARAN as nAlbaran, DPC_ARTICULO as articulo,sum(DPC_CANTIDAD_UMV) as Cantidad, sum(DPC_PRECIO_ARTICULO) precio,  sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) Importe
,
((SELECT IVC_PORC_IVA_1
          FROM TIPO_IVA_CUERPO
         WHERE IVC_EMPRESA          = DPC_EMPRESA          AND
               IVC_CODIGO_IVA       = DPC_TIPO_IVA       AND
               IVC_FECHA_APLICACION = (SELECT MAX(IVC_FECHA_APLICACION)
                                         FROM TIPO_IVA_CUERPO
                                        WHERE IVC_EMPRESA           = DPC_EMPRESA     AND
                                              IVC_CODIGO_IVA        = DPC_TIPO_IVA  AND
                                              IVC_FECHA_APLICACION <= TRUNC(DBDATE$)))/100)* sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) v_iva
from CUERPO_PEDIDO_CLIENTE ,
ALBARAN_ENVIO, DIRECCION_CLIENTE
 where dpc_empresa=1
 and DPC_CANTIDAD_UMV>0 and 
 dpc_empresa=aen_empresa
 and DPC_NUMERO_ALBARAN=ALBARAN_ENVIO.AEN_NUMERO_ALBARAN and
 dpc_ejercicio_lpr=ALBARAN_ENVIO.AEN_EJERCICIO and
 dic_empresa=dpc_empresa and DIC_cliente=ALBARAN_ENVIO.AEN_CLIENTE and
 DIRECCION_CLIENTE.DIC_SUCURSAL =ALBARAN_ENVIO.AEN_SUCURSAL_ENVIO and
 DIC_GRUPO_TARIFA_ALTERN='600' and
      dpc_contabilidad=0 and
      dpc_ejercicio_lpr>=2017 
      and ALBARAN_ENVIO.AEN_FECHA_ALBARAN between to_date('" + desde + @"') and to_date('" + hasta + @"')
      and DPC_NUMERO_ALBARAN between 100000 and 300000
             and DPC_NUMERO_ALBARAN between 100000 and 300000
            GROUP BY dpc_ejercicio_lpr, DPC_EMPRESA,DPC_NUMERO_ALBARAN, DPC_ARTICULO,AEN_FECHA_ALBARAN ,DPC_TIPO_IVA
            union
SELECT 
CHD_FECHA_DEVOLUCION as fecha,to_number( to_char(CHD_FECHA_DEVOLUCION,'YYYY') )as anyo,CHD_EMPRESA as empresa,             
       CASE WHEN CHD_NUMERO_HOJA > 10000000 * to_number(to_char(CHD_FECHA_DEVOLUCION,'YY')) THEN
          CHD_NUMERO_HOJA - 10000000 * to_number(to_char(CHD_FECHA_DEVOLUCION,'YY'))
       ELSE CHD_NUMERO_HOJA END AS NALBARAN, 
       DPC_ARTICULO articulo
,sum(DPC_CANTIDAD_UMV) as Cantidad, sum(DPC_PRECIO_ARTICULO) precio,  sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) Importe,

((SELECT IVC_PORC_IVA_1
          FROM TIPO_IVA_CUERPO
         WHERE IVC_EMPRESA          = CHD_EMPRESA          AND
               IVC_CODIGO_IVA       = DPC_TIPO_IVA       AND
               IVC_FECHA_APLICACION = (SELECT MAX(IVC_FECHA_APLICACION)
                                         FROM TIPO_IVA_CUERPO
                                        WHERE IVC_EMPRESA           = CHD_EMPRESA     AND
                                              IVC_CODIGO_IVA        = DPC_TIPO_IVA  AND
                                              IVC_FECHA_APLICACION <= TRUNC(DBDATE$)))/100)* sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) v_iva
  FROM CABECERA_HOJA_DEVOLUCION 
       inner join DIRECCION_CLIENTE on  DIRECCION_CLIENTE.DIC_SUCURSAL=CHD_SUCURSAL and  DIC_cliente=chd_cliente and chd_empresa=dic_empresa and  DIC_GRUPO_TARIFA_ALTERN='600'
       inner join CUERPO_PEDIDO_CLIENTE on CHD_NUMERO_ABONO=CUERPO_PEDIDO_CLIENTE.DPC_NUMERO_PEDIDO and dpc_empresa=chd_empresa and dpc_ejercicio_lpr is null and DPC_NUMERO_ALBARAN =0 and dpc_empresa=1
WHERE CHD_EMPRESA          IN (1,2)           AND
       CHD_CONTABILIDAD     = '0'              AND
       CHD_FECHA_DEVOLUCION  BETWEEN  to_date('" + desde + @"', 'dd/mm/yyyy') AND  to_date('" + hasta + @"', 'dd/mm/yyyy')           
GROUP BY to_char(CHD_FECHA_DEVOLUCION,'YYYY'), to_char(CHD_FECHA_DEVOLUCION,'YY'),
         CHD_EMPRESA ,
         CHD_NUMERO_HOJA,
         CHD_FECHA_DEVOLUCION, DPC_ARTICULO,DPC_TIPO_IVA)
) group by FECHA";
            Expert con = new Expert();
            DataTable datos = con.Sql_Datatable(sql);
            datos.Columns[0].ColumnName = "Cierre_tienda";
            datos.Columns[1].ColumnName = "Importe";
            return datos;
        }

        DataTable Albaran_expert_OLD(string desde, string hasta)
        {
            string sql = @"Select nAlbaran -100000 ticket,round (SUM(IMPORTE),2) Importe, round (SUM(IMPORTE_IVA),2) IMPORTE_IVA
from 
(
select FECHA , anyo, EMPRESA,NALBARAN,ARTICULO,CANTIDAD, PRECIO, IMPORTE, V_IVA,round (IMPORTE +V_IVA,2) as IMPORTE_IVA  from (
select  AEN_FECHA_ALBARAN fecha ,dpc_ejercicio_lpr  as anyo,DPC_EMPRESA as empresa, DPC_NUMERO_ALBARAN as nAlbaran, DPC_ARTICULO as articulo,sum(DPC_CANTIDAD_UMV) as Cantidad, sum(DPC_PRECIO_ARTICULO) precio,  sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) Importe
,
((SELECT IVC_PORC_IVA_1
          FROM TIPO_IVA_CUERPO
         WHERE IVC_EMPRESA          = DPC_EMPRESA          AND
               IVC_CODIGO_IVA       = DPC_TIPO_IVA       AND
               IVC_FECHA_APLICACION = (SELECT MAX(IVC_FECHA_APLICACION)
                                         FROM TIPO_IVA_CUERPO
                                        WHERE IVC_EMPRESA           = DPC_EMPRESA     AND
                                              IVC_CODIGO_IVA        = DPC_TIPO_IVA  AND
                                              IVC_FECHA_APLICACION <= TRUNC(DBDATE$)))/100)* sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) v_iva
from CUERPO_PEDIDO_CLIENTE ,
ALBARAN_ENVIO, DIRECCION_CLIENTE
 where dpc_empresa=1
 and DPC_CANTIDAD_UMV>0 and 
 dpc_empresa=aen_empresa
 and DPC_NUMERO_ALBARAN=ALBARAN_ENVIO.AEN_NUMERO_ALBARAN and
 dpc_ejercicio_lpr=ALBARAN_ENVIO.AEN_EJERCICIO and
 dic_empresa=dpc_empresa and DIC_cliente=ALBARAN_ENVIO.AEN_CLIENTE and
 DIRECCION_CLIENTE.DIC_SUCURSAL =ALBARAN_ENVIO.AEN_SUCURSAL_ENVIO and
 DIC_GRUPO_TARIFA_ALTERN='600' and
      dpc_contabilidad=0 and
      dpc_ejercicio_lpr>=2017 
      and ALBARAN_ENVIO.AEN_FECHA_ALBARAN between to_date('" + desde + @"') and to_date('" + hasta + @"')
      and DPC_NUMERO_ALBARAN between 100000 and 300000
             and DPC_NUMERO_ALBARAN between 100000 and 300000
            GROUP BY dpc_ejercicio_lpr, DPC_EMPRESA,DPC_NUMERO_ALBARAN, DPC_ARTICULO,AEN_FECHA_ALBARAN ,DPC_TIPO_IVA
            union
SELECT 
CHD_FECHA_DEVOLUCION as fecha,to_number( to_char(CHD_FECHA_DEVOLUCION,'YYYY') )as anyo,CHD_EMPRESA as empresa,             
       CASE WHEN CHD_NUMERO_HOJA > 10000000 * to_number(to_char(CHD_FECHA_DEVOLUCION,'YY')) THEN
          CHD_NUMERO_HOJA - 10000000 * to_number(to_char(CHD_FECHA_DEVOLUCION,'YY'))
       ELSE CHD_NUMERO_HOJA END AS NALBARAN, 
       DPC_ARTICULO articulo
,sum(DPC_CANTIDAD_UMV) as Cantidad, sum(DPC_PRECIO_ARTICULO) precio,  sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) Importe,

((SELECT IVC_PORC_IVA_1
          FROM TIPO_IVA_CUERPO
         WHERE IVC_EMPRESA          = CHD_EMPRESA          AND
               IVC_CODIGO_IVA       = DPC_TIPO_IVA       AND
               IVC_FECHA_APLICACION = (SELECT MAX(IVC_FECHA_APLICACION)
                                         FROM TIPO_IVA_CUERPO
                                        WHERE IVC_EMPRESA           = CHD_EMPRESA     AND
                                              IVC_CODIGO_IVA        = DPC_TIPO_IVA  AND
                                              IVC_FECHA_APLICACION <= TRUNC(DBDATE$)))/100)* sum(DPC_CANTIDAD_UMV)*sum(DPC_PRECIO_ARTICULO) v_iva
  FROM CABECERA_HOJA_DEVOLUCION 
       inner join DIRECCION_CLIENTE on  DIRECCION_CLIENTE.DIC_SUCURSAL=CHD_SUCURSAL and  DIC_cliente=chd_cliente and chd_empresa=dic_empresa and  DIC_GRUPO_TARIFA_ALTERN='600'
       inner join CUERPO_PEDIDO_CLIENTE on CHD_NUMERO_ABONO=CUERPO_PEDIDO_CLIENTE.DPC_NUMERO_PEDIDO and dpc_empresa=chd_empresa and dpc_ejercicio_lpr is null and DPC_NUMERO_ALBARAN =0 and dpc_empresa=1
WHERE CHD_EMPRESA          IN (1,2)           AND
       CHD_CONTABILIDAD     = '0'              AND
       CHD_FECHA_DEVOLUCION  BETWEEN  to_date('" + desde + @"', 'dd/mm/yyyy') AND  to_date('" + hasta + @"', 'dd/mm/yyyy')           
GROUP BY to_char(CHD_FECHA_DEVOLUCION,'YYYY'), to_char(CHD_FECHA_DEVOLUCION,'YY'),
         CHD_EMPRESA ,
         CHD_NUMERO_HOJA,
         CHD_FECHA_DEVOLUCION, DPC_ARTICULO,DPC_TIPO_IVA))  group by nAlbaran";
            Expert con = new Expert();
            DataTable datos = con.Sql_Datatable(sql);
            datos.Columns[0].ColumnName = "ticket";
            datos.Columns[1].ColumnName = "Importe";
            return datos;
        }
        DataTable Cierre_expert(string desde, string hasta) {
            string sql = @"  select to_char(CIF_FECHA_FACTURA,'dd/mm/yyyy') Fecha_Cierre, sum(cif_importe_fra) IMPORTE, sum(cif_importe_fra_iva) IMPORTE_IVA from (
select CIF_FECHA_FACTURA ,CIF_EJERCICIO,CIF_SECUENCIA_FACTURA-100000 , cif_importe_fra, cif_importe_fra_iva  from CABECERA_IMPRESO_FRA_CLTE where cif_empresa=1 and cif_ejercicio >=2017 and CIF_SERIE_FACTURA ='--TPV' and CIF_SECUENCIA_FACTURA>100000
and CIF_FECHA_FACTURA BETWEEN  to_date('" + desde + @"', 'dd/mm/yyyy') AND  to_date('" + hasta + @"', 'dd/mm/yyyy')           
) group by CIF_FECHA_FACTURA";
            Expert con = new Expert();
            DataTable datos = con.Sql_Datatable(sql);
            datos.Columns[0].ColumnName = "Cierre_tienda";
            datos.Columns[1].ColumnName = "Importe";
            return datos;
        
    }
        DataTable Albaran_expert(string desde, string hasta)
        {
            string sql = @"select  ticket, sum(cif_importe_fra) IMPORTE, sum(cif_importe_fra_iva) IMPORTE_IVA from (
select CIF_FECHA_FACTURA ,CIF_EJERCICIO,CIF_SECUENCIA_FACTURA-100000 ticket, cif_importe_fra, cif_importe_fra_iva  from CABECERA_IMPRESO_FRA_CLTE where cif_empresa=1 and cif_ejercicio >=2017 and CIF_SERIE_FACTURA ='--TPV' and CIF_SECUENCIA_FACTURA>100000
and CIF_FECHA_FACTURA BETWEEN to_date('" + desde + @"', 'dd/mm/yyyy') AND  to_date('" + hasta + @"', 'dd/mm/yyyy')           
) group by ticket";
            Expert con = new Expert();
            DataTable datos = con.Sql_Datatable(sql);
            datos.Columns[0].ColumnName = "ticket";
            datos.Columns[1].ColumnName = "Importe";
            return datos;
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (System.Reflection.PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                    else
                        continue;
                }
            }
            return obj;
        }
        class lista_datos 
        {
            public string ticket { get; set; }
            public string Importe { get; set; }
        }
        public  DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            System.Reflection.PropertyInfo[] Props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (System.Reflection.PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        DataTable comparar_datos_quality_tienda(string desde, string hasta) {
            string sql = @"select quality.fecha, tienda.TICKETID , tienda.linea,tienda.producto
,round (quality.cantidad - tienda.cantidad,2) as Diferencia_cantidad
,round (quality.precio - tienda.precio,2) as Diferencia_precio
,round (quality.cantidad - tienda.cantidad,2) *round (quality.precio - tienda.precio,2) as Diferencia_importe

from
(
select FECHA, TICKETID,linea,producto,case when [precio] <0 then [cantidad]* (-1) else [cantidad] end cantidad,case when [precio] <0 then precio* (-1) else precio end precio,importe FROM OPENQUERY(UNICENTA, 'SELECT * FROM `ticket_tienda` ') WHERE fecha between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)   ) tienda
inner join 
(select FechaCreacionALin as FECHA,
[Nº Albarán] -100000 as TICKETID,
[Nº linea albarán] as linea,
Artículo as producto,
Cantidad as cantidad,
[Precio Venta] as precio,
round (Cantidad*[Precio Venta],3) importe
 from ALBARAN_LIN where  serie='C' and FechaCreacionALin between convert(datetime,'" + desde + @"',103) and convert(datetime,'" + hasta + @"',103)  
 ) quality on quality.TICKETID=tienda.TICKETID and quality.linea=tienda.linea

 where (round (quality.cantidad - tienda.cantidad,3) not between -0.01 and 0.01) Or (round (quality.precio - tienda.precio,3) not between -0.01 and 0.01) or round (quality.cantidad - tienda.cantidad,2) *round (quality.precio - tienda.precio,2) not between -0.01 and 0.01
";
            Quality con = new Quality();

            return con.Sql_Datatable(sql);
        }
        DataTable comparar_datos(DataTable tienda, DataTable quality)
        {
            List<lista_datos> tenda = ConvertDataTable<lista_datos>( tienda);
            List<lista_datos> quali= ConvertDataTable<lista_datos>(quality);
            List<lista_datos> listaPedidosClientes = (from c in tenda   // Lista de clientes
                                       from p in quali     // Lista de pedidos
                                       where p.ticket == c.ticket            // Filtro: ID del pedido == ID del cliente
                                       && p.Importe !=c.Importe
                                       && Math.Round(Convert.ToDouble(c.Importe) - Convert.ToDouble(p.Importe.Replace('.', ',')), 3)!=0
                                                      select new lista_datos(){ ticket = p.ticket, Importe = (Math.Round(Convert.ToDouble(c.Importe) - Convert.ToDouble(p.Importe.Replace('.',',')) , 2)).ToString() }).ToList(); 
            

            DataTable datos= ToDataTable<lista_datos>(listaPedidosClientes);

            return datos;
        }

        
        protected void Btexport_Click(object sender, EventArgs e)
        {
            string F_desde = datepicker1.Value;
            string F_hasta = datepicker2.Value;
            DataTable Tienda = Ticket_Dinero(F_desde.Replace("-",@"/"), F_hasta.Replace("-", @"/"));
            DataTable cierre_tenda= cierre_tienda(F_desde.Replace("-", @"/"), F_hasta.Replace("-", @"/"));
            DataTable Quality = Albaran_dinero(F_desde.Replace("-", @"/"), F_hasta.Replace("-", @"/"));
            DataTable cierre_qualt= cierre_quality(F_desde.Replace("-", @"/"), F_hasta.Replace("-", @"/"));
            DataTable Expert = Albaran_expert(F_desde.Replace("-", @"/"), F_hasta.Replace("-", @"/"));
            DataTable cierre_Expert = Cierre_expert(F_desde.Replace("-", @"/"), F_hasta.Replace("-", @"/"));
            comparar_datos(Tienda, Quality);
            using (ExcelPackage pck = new ExcelPackage())
            {
                Tienda.TableName = "Tienda";
                cierre_tenda.TableName = "Cierre_Tienda";
                cierre_qualt.TableName = "Cierre_Quality";
                cierre_Expert.TableName = "Cierre_Expert";
                Quality.TableName = "Quality";
                Expert.TableName = "Expert";
                List<string> dateColumns = new List<string>() { "Fecha_Cierre" };
                ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("Tienda");
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Quality");
                
                ws1.Cells["A1"].LoadFromDataTable(Tienda, true, OfficeOpenXml.Table.TableStyles.Light16);
                ws1.Cells["E1"].LoadFromDataTable(cierre_tenda, true, OfficeOpenXml.Table.TableStyles.Light13);
                ws2.Cells["A1"].LoadFromDataTable(Quality, true, OfficeOpenXml.Table.TableStyles.Light17);
                ws2.Cells["E1"].LoadFromDataTable(cierre_qualt, true, OfficeOpenXml.Table.TableStyles.Light18);
                ExcelWorksheet ws3 = pck.Workbook.Worksheets.Add("Expert");
                ws3.Cells["A1"].LoadFromDataTable(Expert, true, OfficeOpenXml.Table.TableStyles.Light11);
                ws3.Cells["E1"].LoadFromDataTable(cierre_Expert, true, OfficeOpenXml.Table.TableStyles.Light10);
                FormatWorksheetData(dateColumns, cierre_tenda, ws1);
                FormatWorksheetData(dateColumns, cierre_qualt, ws2);

                if (Tienda.Rows.Count > 0 && Quality.Rows.Count > 0)
                {
                   // DataTable comparar = comparar_datos(Tienda, Quality);
                    DataTable comparar = comparar_datos_quality_tienda(F_desde.Replace("-", @"/"), F_hasta.Replace("-", @"/"));
                    DataTable comparar2 = comparar_datos(Quality, Expert);
                    comparar2.TableName= "Descuadre2";
                    comparar.TableName = "Descuadre";
                    ExcelWorksheet ws4 = pck.Workbook.Worksheets.Add("Descuadre Tienda y Quality");
                    ws4.Cells["A2"].Value = "Tienda - Quality";
                    ws4.Cells["A3"].LoadFromDataTable(comparar, true, OfficeOpenXml.Table.TableStyles.Light14);
                    // ExcelWorksheet ws5 = pck.Workbook.Worksheets.Add("Descuadre Tienda, Expert y Quality");
                    ws4.Cells["J2"].Value = "Tienda - Expert";
                    ws4.Cells["J3"].LoadFromDataTable(comparar2, true, OfficeOpenXml.Table.TableStyles.Light15);
                }

                // make sure it is sent as a XLSX file
                // Response.ContentType = "application/vnd.ms-excel";
                Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                Response.AppendHeader("content-disposition", "attachment; filename=Control_Tienda.xlsx");
                // make sure it is downloaded rather than viewed in the browser window
               // Response.AddHeader("Content-disposition", "attachment; filename=Control_Tienda");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

        }
    }
}