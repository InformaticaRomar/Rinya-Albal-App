using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Data;

namespace Rinya_Albal_App
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            /*   
               DataTable dt_rol = new DataTable("Lineas");
               DataTable dt_User_rol = new DataTable("Lineas");
               try
               {
                   string Con_Q = @"Data Source=192.168.1.195\sqlserver2008;Initial Catalog=QC600;Persist Security Info=True;User ID=dso;Password=dsodsodsData Source=192.168.1.195\sqlserver2008;Initial Catalog=QC600;Persist Security Info=True;User ID=dso;Password=dsodsodso";
                   string sql_rol = "SELECT [IDGrupo] ,[NombreGrupo] FROM [QC600].[dbo].[GRUPO_USUARIOS]";
                   string sql_User_rol = "SELECT [IDUsuario],[IDGrupo] FROM [QC600].[dbo].[USUARIO_GRUPO]";

                   using (SqlConnection cnn = new SqlConnection(Con_Q))
                   {


                       cnn.Open();
                       SqlDataAdapter da = new SqlDataAdapter(sql_rol, cnn);
                       da.Fill(dt_rol);
                       SqlDataAdapter da2 = new SqlDataAdapter(sql_User_rol, cnn);
                       da2.Fill(dt_User_rol);

                       cnn.Close();
                       foreach (DataRow lin in dt_rol.AsEnumerable())
                       {
                           if (!Roles.RoleExists(lin[0].ToString()))
                           {
                               Roles.CreateRole(lin[0].ToString());

                           }
                       }
                       foreach (DataRow lin in dt_User_rol.AsEnumerable())
                       {
                           if (!Roles.IsUserInRole(lin[0].ToString(), lin[1].ToString()))
                           {
                               Roles.AddUserToRole(lin[0].ToString(), lin[1].ToString());
                           }
                       }

                   }
               }
               catch (Exception ex)
               {
                   // Agregar aquí un control de errores para la depuración.
                   // Este mensaje de error no debería reenviarse al que realiza la llamada.
                   System.Diagnostics.Trace.WriteLine("[Error ROLES] Exception " + ex.Message);
               }*/
        }
        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

            HttpCookie decryptedCookie =
                Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (decryptedCookie != null)
            {
               
                FormsAuthenticationTicket ticket =
               FormsAuthentication.Decrypt(decryptedCookie.Value);

                string[] roles = ticket.UserData.Split(new[] { "," },
                     StringSplitOptions.RemoveEmptyEntries);

                var identity = new System.Security.Principal.GenericIdentity(ticket.Name);
                var principal = new System.Security.Principal.GenericPrincipal(identity, roles);

                HttpContext.Current.User = principal;
                System.Threading.Thread.CurrentPrincipal = HttpContext.Current.User;
            }
            // look if any security information exists for this request
            if (HttpContext.Current.User != null)
            {
                // see if this user is authenticated, any authenticated cookie (ticket) exists for this user
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    // see if the authentication is done using FormsAuthentication
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        // Get the roles stored for this request from the ticket
                        // get the identity of the user
                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                        // get the forms authetication ticket of the user
                        FormsAuthenticationTicket ticket = identity.Ticket;
                        // get the roles stored as UserData into the ticket 
                        string[] roles = ticket.UserData.Split(',');
                        // create generic principal and assign it to the current request
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(identity, roles);
                    }
                }
            }
        }
    }
}