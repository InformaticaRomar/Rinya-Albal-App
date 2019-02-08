<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sincronizar.aspx.cs" Inherits="Rinya_Albal_App.Comercial.Tienda.Sincronizar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
              <div class="form-group">
                  <h2>Sincronizar </h2>
                <div class="row">
                    
                   
                     
                     <div class="col-md-*"><asp:Button ID="BtSincro_stock" runat="server" Text="Sincronizar Stock" CssClass="btn btn-default" OnClick="Sincro_stock_Click" /></div>
                    </div>
                   <div class="row"><br />
                       </div>
                     <div class="row">
                     <div class="col-md-*"><asp:Button ID="Btsincro_Ticket" runat="server" Text="Sincronizar Tickets" CssClass="btn btn-default" OnClick="Sincro_Ticket_Click" /></div>
                    </div>
                  <div class="row">
                     <div class="col-md-*"><asp:Button ID="Btsincro_Cliente" runat="server" Text="Sincronizar Clientes" CssClass="btn btn-default" OnClick="Btsincro_Cliente_Click"  /></div>
                    </div>
                  <div class="row">
                     <div class="col-md-*"><asp:Button ID="Btsincro_Articulo" runat="server" Text="Sincronizar Precios" CssClass="btn btn-default" OnClick="Btsincro_Articulo_Click"  /></div>
                    </div>
              
              </div>
</asp:Content>
