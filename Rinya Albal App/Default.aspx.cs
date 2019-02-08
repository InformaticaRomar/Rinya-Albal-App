using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Rinya_Albal_App
{

    public partial class _Default : Page
    {

        private void Signout_Click(object sender, System.EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Account/logon.aspx", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {


        }


    }
}