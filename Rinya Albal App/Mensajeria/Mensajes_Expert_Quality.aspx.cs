using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Rinya_Albal_App.Mensajeria
{
    public partial class Mensajes_Expert_Quality : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack == true)
            {
                datos = Session["datos"] as DataTable;
            }

        }
        private DataTable datos { get; set; }
        private bool Datos_Quality(string Tipo_mensajes)
        {
            string sql = string.Empty;
            bool resultado = false;
            Quality con = new Quality();
            if (TBLabel.Text.Length <= 0)
            {
                sql = @"SELECT top 20 [NUMMSJ] as [Numero Mensaje],Convert (varchar,[FECHAMSJ],103)as [Fecha Mensaje],[TIPOMSJ] as [Tipo Mensaje],[TEXTOMSJ],[PROCESADO] as [Error]
                        FROM[INT_RINYA].[dbo].[MENSAJES_EXPERT_QUALITY]
                        where [TIPOMSJ]  like '" + Tipo_mensajes + "%' order by NUMMSJ desc ";
            }
            else
            {
                sql = @"SELECT  [NUMMSJ] as [Numero Mensaje],Convert (varchar,[FECHAMSJ],103)as [Fecha Mensaje],[TIPOMSJ] as [Tipo Mensaje],[TEXTOMSJ],[PROCESADO] as [Error]
                        FROM[INT_RINYA].[dbo].[MENSAJES_EXPERT_QUALITY]
                        where [TIPOMSJ]  like '" + Tipo_mensajes + "%' AND [TEXTOMSJ] like '%" + TBLabel.Text + ";%' order by NUMMSJ desc ";
            }

            datos = con.Sql_Datatable(sql);
            Session.Add("datos", datos);
            DataView source = new DataView(datos);
            if (datos != null)
                if (datos.Rows.Count > 0)
                { resultado = true; }
            GridView1.DataSource = source;
            GridView1.DataBind();
            GridView1.Visible = true;
            return resultado;
        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            // (Combotipo as HtmlSelect)
            //combotipo.
            string Tipo_mensajes = "0" + Combotipo.Value.ToString() + "0";
            Datos_Quality(Tipo_mensajes);
        }
    }
}