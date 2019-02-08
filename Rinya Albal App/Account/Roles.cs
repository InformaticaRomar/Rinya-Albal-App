using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Configuration.Provider;
using System.Data;

namespace Rinya_Albal_App.Account
{
    public class Roles : RoleProvider
    {
        OleDbConnection connection;

        public Roles()
        {
            connection = new OleDbConnection();
            connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlServices"].ConnectionString;
        }
        #region RoleProvider Implementados
        public override string[] GetUsersInRole(string roleName)
        {
            OleDbCommand command = new OleDbCommand();
            List<string> resultado = new List<string>();

            try
            {
                command.Connection = connection;
                command.CommandText = @"select Usuarios.IDUsuario
                        FROM  [QC_PRUEBAS].[dbo].[GRUPO_USUARIOS]
                         INNER JOIN [QC_PRUEBAS].[dbo].[USUARIO_GRUPO] on [USUARIO_GRUPO].IDGrupo=[GRUPO_USUARIOS].IDGrupo
                         INNER JOIN [QC_PRUEBAS].[dbo].Usuarios on [USUARIO_GRUPO].IDUsuario=[USUARIOS].IDUsuario

                          WHERE  ([GRUPO_USUARIOS].IDGrupo='" + roleName + "')";

                // command.Parameters.AddWithValue("@Username", username);
                command.Connection.Open();

                OleDbDataReader rd = command.ExecuteReader();
                while (rd.Read())
                    resultado.Add(rd.GetString(0));

                return resultado.ToArray();
            }
            catch
            {
                throw new Exception("Error al extraer los usuarios por rol.");

            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                }
            }


        }

        public override string[] GetAllRoles()
        {
            OleDbCommand command = new OleDbCommand();
            List<string> resultado = new List<string>();
            try
            {
                command.Connection = connection;
                command.CommandText = "SELECT [IDGrupo] ,[NombreGrupo] FROM [QC_PRUEBAS].[dbo].[GRUPO_USUARIOS]";
                command.Connection.Open();

                OleDbDataReader rd = command.ExecuteReader();
                while (rd.Read())
                    resultado.Add(rd.GetString(0));

                return resultado.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                }
            }
        }
        public override string[] GetRolesForUser(string username)
        {
            OleDbCommand command = new OleDbCommand();
            List<string> resultado = new List<string>();
            try
            {
                command.Connection = connection;
                command.CommandText = @"SELECT [GRUPO_USUARIOS].IDGrupo
                                     FROM  [GRUPO_USUARIOS]
                                     INNER JOIN [USUARIO_GRUPO] on [USUARIO_GRUPO].IDGrupo=[GRUPO_USUARIOS].IDGrupo
                                     INNER JOIN Usuarios on [USUARIO_GRUPO].IDUsuario=[USUARIOS].IDUsuario

                                      WHERE (Usuarios.IDUsuario = '" + username + "')";

                // command.Parameters.AddWithValue("@Username", username);
                command.Connection.Open();

                OleDbDataReader rd = command.ExecuteReader();
                while (rd.Read())
                    resultado.Add(rd.GetString(0));

                return resultado.ToArray();
            }
            catch
            {

                throw new Exception("Error al consultar los roles por usuario.");

            }
            finally
            {
                if (command.Connection != null)
                {
                    if (command.Connection.State == ConnectionState.Open)
                        command.Connection.Close();
                }
            }
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            bool userIsInRole = false;

            OleDbCommand command = new OleDbCommand();

            try
            {
                command.Connection = connection;
                command.CommandText = @"SELECT COUNT(*) FROM  [GRUPO_USUARIOS]
 INNER JOIN [USUARIO_GRUPO] on [USUARIO_GRUPO].IDGrupo=[GRUPO_USUARIOS].IDGrupo
 INNER JOIN Usuarios on [USUARIO_GRUPO].IDUsuario=[USUARIOS].IDUsuario
                                        WHERE (Usuarios.IDUsuario = '" + username + @"')
                                       and ([GRUPO_USUARIOS].IDGrupo= '" + roleName + @"')";

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Rolename", roleName);

                command.Connection.Open();

                int numRoles = (int)command.ExecuteScalar();

                if (numRoles > 0)
                {
                    userIsInRole = true;
                }
            }
            catch (Exception )
            {
                
                throw new Exception("Error al acceder a los datos.");
            }
            finally
            {
                command.Connection.Close();
            }

            return userIsInRole;

        }
        public override bool RoleExists(string roleName)
        {
            bool existe = false;

            OleDbCommand command = new OleDbCommand();

            try
            {
                command.Connection = connection;
                command.CommandText = @"SELECT COUNT(*) FROM  [GRUPO_USUARIOS] where IDGrupo = '" + roleName + "'";


                //command.Parameters.AddWithValue("@ApplicationName",  ApplicationName);            
                command.Connection.Open();

                int numRols = (int)command.ExecuteScalar();

                if (numRols > 0)
                {
                    existe = true;
                }
            }
            catch (Exception )
            {
                throw new Exception("Error al acceder a los Roles");
            }
            finally
            {
                command.Connection.Close();
            }

            return existe;

        }

        #endregion RoleProvider Implementados

        #region RoleProvider NO_Implementados

        public override string ApplicationName
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
            /*   #region Validación de los Roles
               foreach (string rolename in roleNames)
               {
                   if (!RoleExists(rolename))
                   {
                       throw new System.Configuration.Provider.ProviderException("El Rol no Existe.");
                   }
               }
               #endregion Validación de los Roles

               #region validación de los Usuarios
               foreach (string username in usernames)
               {
                   if (username.Contains(","))
                   {
                       throw new ArgumentException("Los Nombres de usuario no pueden contener comas.");
                   }

                   foreach (string rolename in roleNames)
                   {
                       if (IsUserInRole(username, rolename))
                       {
                           throw new ProviderException("El usuario ya pertenece al Rol.");
                       }
                   }
               }
               #endregion validación de los Usuario

               OleDbCommand command = new OleDbCommand();
               OleDbTransaction tran = null;

               try
               {
                   command.Connection = connection;
                   command.CommandText = @"INSERT INTO UsuariosPorRol
                                       (IDUsuario, IDRol)
                                           SELECT Roles.IDRol, Usuarios.IDUsuario
                                           FROM Roles, Usuarios
                                       WHERE (Usuarios.Nombre = @Username) 
                                       AND (Roles.Rol = @RolName) ";

                   command.Parameters.Add("@Username", OleDbType.VarChar, 100);
                   command.Parameters.Add("@RolName", OleDbType.VarChar, 50);


                   command.Connection.Open();
                   tran = command.Connection.BeginTransaction();
                   command.Transaction = tran;

                   foreach (string username in usernames)
                   {
                       foreach (string rolename in roleNames)
                       {
                           command.Parameters["@Username"].Value = username;
                           command.Parameters["@RolName"].Value = rolename;
                           command.ExecuteNonQuery();
                       }
                   }

                   tran.Commit();

               }
               catch
               {
                   tran.Rollback();
               }
               finally
               {
                   if (command.Connection != null)
                   {
                       if (command.Connection.State == ConnectionState.Open)
                           command.Connection.Close();
                   }
               }*/
        }
        public override void CreateRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion RoleProvider NO_Implementados

    }



}