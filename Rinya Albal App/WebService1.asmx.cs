using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Rinya_Albal_App.Models;
using Utiles;
using System.Data;

namespace Rinya_Albal_App
{
    /// <summary>
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]

    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetTableData()
        {
            var echo = int.Parse(HttpContext.Current.Request.Params["sEcho"]);
            var displayLength = int.Parse(HttpContext.Current.Request.Params["iDisplayLength"]);
            var displayStart = int.Parse(HttpContext.Current.Request.Params["iDisplayStart"]);
            var sortOrder = HttpContext.Current.Request.Params["sSortDir_0"].ToString(CultureInfo.CurrentCulture);
            var roleId = HttpContext.Current.Request.Params["roleId"].ToString(CultureInfo.CurrentCulture);
            var search = HttpContext.Current.Request.Params["sSearch"].ToString(CultureInfo.CurrentCulture);
            var records = ObtenerZonas().ToList();
            //Añadido Filtro columnas
            var search2 = HttpContext.Current.Request.Params["sSearch_0"].ToString(CultureInfo.CurrentCulture);
            var search3 = HttpContext.Current.Request.Params["sSearch_1"].ToString(CultureInfo.CurrentCulture);
            if (!records.Any())
            {
                return string.Empty;
            }
            var filterRecords = records;
            if (search.Length > 0)
            {
                /*  var prue  =  from l in records
                                   where l.Zona.Contains(search) || l.Nombre.Contains(search)
                                   select l;*/
                filterRecords = records.Where(l => l.Zona.Contains(search) || l.Nombre.Contains(search)).ToList(); // prue.ToList();
            }
            if (search2.Length > 0)
            {
                /*  var prue  =  from l in records
                                   where l.Zona.Contains(search) || l.Nombre.Contains(search)
                                   select l;*/
                filterRecords = filterRecords.Where(l => l.Zona.Contains(search2)).ToList(); // prue.ToList();
            }
            if (search3.Length > 0)
            {
                /*  var prue  =  from l in records
                                   where l.Zona.Contains(search) || l.Nombre.Contains(search)
                                   select l;*/
                filterRecords = filterRecords.Where(l => l.Nombre.Contains(search3)).ToList(); // prue.ToList();
            }
            //orderedResults.Select(s => s.Zona.Contains(search) || s.Nombre.Contains(search));

            var orderedResults = sortOrder == "asc"
                                 ? filterRecords.OrderBy(o => o.Zona)
                                 : filterRecords.OrderByDescending(o => o.Zona);
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart + 1;
            //  var filterRecords = (orderedResults);




            var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();
            var hasMoreRecords = false;

            var sb = new StringBuilder();
            sb.Append(@"{" + "\"sEcho\": " + echo + ",");
            sb.Append("\"recordsTotal\": " + records.Count + ",");
            sb.Append("\"recordsFiltered\": " + records.Count + ",");
            sb.Append("\"iTotalRecords\": " + records.Count + ",");
            sb.Append("\"iTotalDisplayRecords\": " + records.Count + ",");
            sb.Append("\"aaData\": [");
            foreach (var result in pagedResults)
            {
                if (hasMoreRecords)
                {
                    sb.Append(",");
                }

                sb.Append("[");
                sb.Append("\"" + result.Zona + "\",");
                sb.Append("\"" + result.Nombre + "\",");
                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server' height='16' width='16' alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");
            return sb.ToString();
        }
        [WebMethod(EnableSession = true)]

        public void update_Data(Zonas datos)
        {
            string sql = "update [QC600].[dbo].[ZONA] set [Nombre]='" + datos.Nombre + "' where [Zona]=" + datos.Zona;
            Utiles.Quality lanza = new Quality();
            lanza.sql_update(sql);
           
        }
        private string GetRecordsFromDatabaseWithFilter()
        {
            return null;
        }


        public static int ToInt(string toParse)
        {
            int result;
            if (int.TryParse(toParse, out result)) return result;

            return result;
        }
        public WebService1()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
        [WebMethod(EnableSession = true)]
        public void data(Zonas datos)
        {
            int a = 0;
            a = a + 1;
        }


        public string HelloWorld()
        {
            //string result = "";
            //  var serializer = new XmlSerializer(typeof(List<string>));

            /*
            XElement xml = new XElement("Zona",
                from emp in ObtenerZonas()
                select new XElement("ZONA",
                             new XAttribute("ID", emp.Zona),
                               new XElement("FName", emp.Nombre)
                           ));*/
            DataTable dt = ObtenerZonas2();
            System.Web.Script.Serialization.JavaScriptSerializer serializer2 = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return GetTableData();

        }
        public IEnumerable<Zonas> ObtenerZonas()
        {
            string sql = @"SELECT [Zona],[Nombre] FROM [QC600].[dbo].[ZONA]";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Zonas> lista_datos = new List<Zonas>();
            foreach (DataRow lin in datos.AsEnumerable())
            {
                Zonas linea = new Zonas { Zona = lin[0].ToString(), Nombre = lin[1].ToString() };
                lista_datos.Add(linea);
            }

            return lista_datos;
        }
        public DataTable ObtenerZonas2()
        {
            string sql = @"SELECT [Zona],[Nombre] FROM [QC600].[dbo].[ZONA]";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Zonas> lista_datos = new List<Zonas>();

            return datos;
        }
        public class Zonas
        {
            public string Zona { get; set; }
            public string Nombre { get; set; }

        }
    }
}
