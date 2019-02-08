using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Utiles;
using System.Data;

namespace Rinya_Albal_App.Manzanares.Finanzas
{
    /// <summary>
    /// Summary description for WebService_finanzas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class WebService_finanzas : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string GetEntidad()
        {
            string sql = "select [ID], [ENTIDAD] as VALOR FROM [CONFIRMING_ENTIDAD]";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod(EnableSession = true)]
        public string GetEmpresa()
        {
            string sql = "select [ID], [EMPRESA] as VALOR FROM [CONFIRMING_EMPRESA]";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
