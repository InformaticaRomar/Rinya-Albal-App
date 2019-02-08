using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;

namespace Rinya_Albal_App.Account
{
    public partial class Logon : System.Web.UI.Page
    {
        private bool ValidateUser(string userName, string passWord)
        {
            SqlConnection conn;
            SqlCommand cmd;
            string lookupPassword = null;

            // Buscar nombre de usuario no válido.
            // el nombre de usuario no debe ser un valor nulo y debe tener entre 1 y 15 caracteres.
            if ((null == userName) || (0 == userName.Length) || (userName.Length > 15))
            {
                System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of userName failed.");
                return false;
            }

            // Buscar contraseña no válida.
            // La contraseña no debe ser un valor nulo y debe tener entre 1 y 25 caracteres.
            if ((null == passWord) || (0 == passWord.Length) || (passWord.Length > 25))
            {
                System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of passWord failed.");
                return false;
            }

            try
            {
                // Consultar con el administrador de SQL Server para obtener una conexión apropiada
                // cadena que se utiliza para conectarse a su SQL Server local.
                //Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso
                conn = new SqlConnection(@"Data Source=172.16.0.12;Initial Catalog=QC_PRUEBAS;Persist Security Info=True;User ID=dso;Password=dsodsodso");
                conn.Open();

                // Crear SqlCommand para seleccionar un campo de contraseña desde la tabla de usuarios dado el nombre de usuario proporcionado.
                cmd = new SqlCommand("SELECT [Password]  FROM [QC_PRUEBAS].[dbo].[USUARIOS] where [IDUsuario]=@userName", conn);
                cmd.Parameters.Add("@userName", SqlDbType.VarChar, 25);
                cmd.Parameters["@userName"].Value = userName;

                // Ejecutar el comando y capturar el campo de contraseña en la cadena lookupPassword.
                lookupPassword = (string)cmd.ExecuteScalar();

                // Comando de limpieza y objetos de conexión.
                cmd.Dispose();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                // Agregar aquí un control de errores para la depuración.
                // Este mensaje de error no debería reenviarse al que realiza la llamada.
                System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception " + ex.Message);
            }

            // Si no se encuentra la contraseña, devuelve false.
            if (null == lookupPassword)
            {
                // Para más seguridad, puede escribir aquí los intentos de inicio de sesión con error para el registro de eventos.
                return false;
            }

            // Comparar lookupPassword e introduzca passWord, usando una comparación que distinga mayúsculas y minúsculas.
            string SaltKey = "estaeslaclavedeaccesodecomparacion";
            Byte[] cipherTextBytes = System.Text.Encoding.GetEncoding(1252).GetBytes(lookupPassword);
            byte[] keyBytes = System.Text.Encoding.ASCII.GetBytes(SaltKey);
            string rtn = "";
            int temp2 = 0;
            string result = "";
            byte[] cipherTextBytes2 = System.Text.Encoding.GetEncoding(1252).GetBytes(passWord);
            for (int i = 0; i < cipherTextBytes2.Length; i++)
            {
                temp2 = cipherTextBytes2[i] + keyBytes[i];
                if (temp2 < 0)
                {
                    temp2 = temp2 - 255;
                }
                result = result + Convert.ToChar(temp2);
            }
                int n = cipherTextBytes.Length;
            int temp = 0;
            for (int i = 0; i < cipherTextBytes.Length; i++)
            {
                temp = cipherTextBytes[i] - keyBytes[i];
                if (temp < 0)
                {
                    temp = temp + 255;
                }
                rtn = rtn + Convert.ToChar(temp);

            }
            return rtn.ToUpper().Equals(passWord.ToUpper()); ;
        }
        private void cmdLogin_ServerClick(object sender, System.EventArgs e)
        {
            if (ValidateUser(txtUserName.Text, txtUserPass.Text))
            {
                FormsAuthenticationTicket tkt;
                string cookiestr;
                HttpCookie ck;
                tkt = new FormsAuthenticationTicket(1, txtUserName.Text, DateTime.Now,
          DateTime.Now.AddHours(8), chkPersistCookie.Checked, "your custom data");
                cookiestr = FormsAuthentication.Encrypt(tkt);
                ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                if (chkPersistCookie.Checked)
                    ck.Expires = tkt.Expiration;
                ck.Path = FormsAuthentication.FormsCookiePath;
                Response.Cookies.Add(ck);

                string strRedirect;
                strRedirect = Request["ReturnUrl"];
                if (strRedirect == null)
                    strRedirect = "~/Default.aspx";
                Response.Redirect(strRedirect, true);
            }
            else
                Response.Redirect("logon.aspx", true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            this.cmdLogin.ServerClick += new System.EventHandler(this.cmdLogin_ServerClick);
        }

        protected void LogIn(object sender, EventArgs e)
        {
            this.cmdLogin_ServerClick(sender, e);
        }
    }
}