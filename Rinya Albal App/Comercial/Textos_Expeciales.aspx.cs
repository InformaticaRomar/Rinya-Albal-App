using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;
namespace Rinya_Albal_App.Comercial
{
    public partial class Textos_Expeciales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
      private  Expert con = new Expert();
        private Quality con2 = new Quality();
        private void Accion(string cod_cliente)
        {
            System.Text.StringBuilder csv = new System.Text.StringBuilder();
            string sql = "select TEC_CLIENTE,coalesce (TEC_SUCURSAL,0) as TEC_SUCURSAL,TEC_TIPO_TEXTO,TEC_TEXTO from TEXTOS_ESPECIALES_CLIENTE where TEC_EMPRESA=1 anD TEC_CLIENTE=" + cod_cliente;
            DataTable datos = con.Sql_Datatable(sql);
            foreach (DataRow row in datos.AsEnumerable())
            {
                string fila = csv.AppendLine(String.Join(";", row.ItemArray)).ToString();
                string sql2 = "insert into [INT_RINYA].[dbo].[CORRECCIONES_EXPERT_QUALITY] ([TEXTOMSJ],[TIPOMSJ],[FECHAMSJ],[PROCESADO]) values ('" + fila+ @"','0801',GETDATE(),0)";
                con2.sql_update(sql2);
            }
            Respuesta_1_lbl.Text = "Integrado con exito...";
            Respuesta_2_lbl.Visible = false;
            BtBuscar.Enabled = true;
        }
        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Respuesta_1_lbl.Text = "Procesando...";
            Respuesta_1_lbl.Visible = true;
            Respuesta_2_lbl.Visible = true;
            BtBuscar.Enabled = false;
            Accion(CL_textBox.Text);
        }
    }
}