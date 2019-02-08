<%@ Page Title="Iniciar sesión" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Logon.aspx.cs" Inherits="Rinya_Albal_App.Account.Logon" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >
    <h2><%: Title %></h2>
   
     <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                     <h4>Utilice un usuario de quality para iniciar sesión.</h4>
                    <hr />
                      <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                     <div class="form-group">
                         <asp:Label runat="server" AssociatedControlID="txtUserName" CssClass="col-md-2 control-label">Usuario</asp:Label>
                         <div class="col-md-10">
                             <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" TextMode="SingleLine"/>
                            
                             <ASP:RequiredFieldValidator ControlToValidate="txtUserName" Display="Static" ErrorMessage="El campo usuario es obligatorio." runat="server" ID="vUserName" />
                              
                              </div>
                    </div>
                    <div class="form-group">
                         <asp:Label runat="server" AssociatedControlID="txtUserPass" CssClass="col-md-2 control-label">Contraseña</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtUserPass" TextMode="Password" CssClass="form-control" />
                             
                            <asp:RequiredFieldValidator runat="server"  ID="vUserPass"  ControlToValidate="txtUserPass" CssClass="text-danger" ErrorMessage="El campo de contraseña es obligatorio."  />
                               </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <div class="checkbox">
                               
                               <ASP:CheckBox id="chkPersistCookie" runat="server" autopostback="false" />
                                <asp:Label runat="server" AssociatedControlID="chkPersistCookie">¿Recordar cuenta?</asp:Label>
                            </div>
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                             <asp:Button runat="server" OnClick="LogIn" Text="Iniciar sesión" CssClass="btn btn-default" />
                            <input type="submit" Value="Iniciar sesión" runat="server" style="display:none" ID="cmdLogin"><p></p>
                            <asp:Label id="lblMsg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat="server" />

                        </div>
                    </div>
                </div>
                </section>
        </div>
     </div>
                
                 


    </asp:Content>
