using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
namespace Rinya_Albal_App.Comercial.Tienda
{
    public partial class Sincronizar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        Quality con = new Quality();
        protected void Sincro_stock_Click(object sender, EventArgs e)
        {
            if (con.sql_update("EXEC [QC_PRUEBAS].[dbo].[SINCRO_STOCK_TPV]")) { 
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Stock Sincronizado con exito');", true);
            }
        }

        protected void Sincro_Ticket_Click(object sender, EventArgs e)
        {
           if( con.sql_update("EXEC [INT_TIENDA].[dbo].[IMPORTADOR_UNICENTA]"))
            {
                if(con.sql_update("EXEC [INT_TIENDA].[dbo].[IMPORTADOR]"))
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Tickets sincronizados con exito');", true);
                }
            }
            
            
        }

        protected void Btsincro_Cliente_Click(object sender, EventArgs e)
        {
            if (con.sql_update("EXEC [QC_PRUEBAS].[dbo].[SINCRO_CLIENTE_TPV]"))
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Stock Sincronizado con exito');", true);
            }
        }

        protected void Btsincro_Articulo_Click(object sender, EventArgs e)
        {

            if (con.sql_update("EXEC [QC_PRUEBAS].[dbo].[SINCRO_ARTICULO_TPV]"))
            {

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Stock Sincronizado con exito');", true);
            }
        }
    }
}