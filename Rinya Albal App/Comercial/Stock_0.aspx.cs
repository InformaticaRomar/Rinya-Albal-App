using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;

namespace Rinya_Albal_App.Comercial
{
    public partial class Stock_0 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        DataTable dat { get; set; }
        private DataTable datos_stock(int articulo_)
        {
            string sql = @"select sec_empresa empresa,SEC_CODIGO_PRODUCTO articulo,SEC_CANTIDAD_EXISTENTE Cantidad_Centro,SEX_CANTIDAD_EXISTENTE cantidad_almacen 
from PR_SITUACION_EXIST_CENTRO inner join PR_SITUACION_EXISTENCIA_ALM  on
SEC_CODIGO_PRODUCTO =SEX_CODIGO_PRODUCTO and sec_empresa=sex_empresa
where sec_empresa in (1,2) and SEC_CODIGO_PRODUCTO ='" + articulo_.ToString() + "'";
            Expert datos_expert = new Expert();
            try
            {
                dat = datos_expert.Sql_Datatable(sql);
            }
            catch (Exception ex)
            {
                Respuesta_1_lbl.Text = ex.Message;
            }

            return dat;
        }
        private bool update_articulo(int articulo_num)
        {
            string sql1 = @"update PR_SITUACION_EXIST_CENTRO  set SEC_CANTIDAD_EXISTENTE=0
where sec_empresa in (1,2) AND SEC_CODIGO_PRODUCTO = '" + articulo_num.ToString() + "'";
            Expert datos_expert = new Expert();
            string sql2 = @" '" + articulo_num.ToString() + "'";
            //  MessageBox.Show(sql2);
            datos_expert.sql_update(sql1);
            /* if (datos_expert.sql_update(sql2))
                 return true;*/
            if (datos_expert.sql_update(sql1))
            {

                return datos_expert.sql_update(sql2);
            }
            return false;
        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            int articulo_num = 0;
            if (AR_textBox.Text.Length > 0)
            {
                Respuesta_1_lbl.Text = "Procesando";
                if (int.TryParse(AR_textBox.Text, out articulo_num))
                {
                    //num_art = articulo_num;
                    if (update_articulo(articulo_num))
                    {
                        Respuesta_1_lbl.Text = "Se ha borrado el articulo " + articulo_num.ToString();

                    }
                  
                }

            }
        }
    }
}