<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stock_0.aspx.cs" Inherits="Rinya_Albal_App.Comercial.Stock_0" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
   
        
        
          <div class="form-group">
                  <h2>Poner Stock Expert a 0</h2>
                <div class="row">
                    
                   
                     <div class="col-md-*"><asp:Label runat="server" AssociatedControlID="AR_textBox" CssClass="control-label">Articulo:</asp:Label></div>
                <div class="col-md-*"> <asp:TextBox ID="AR_textBox"  CssClass="form-control" runat="server"></asp:TextBox></div>
  <div class="col-md-*"><asp:Button ID="BtBuscar" runat="server" Text="Vaciar" CssClass="btn btn-default" OnClick="BtBuscar_Click" /></div>
                    </div>
              <div class="row">
                    
               <div class="col-md-*"><asp:Label runat="server" id="Respuesta_1_lbl"  Visible="false" CssClass=" control-label">Procesando...</asp:Label></div>
                   <div class="col-md-*"><asp:Label runat="server" id="Respuesta_2_lbl"  Visible="false" CssClass=" control-label">Por favor espere</asp:Label></div>
                  </div>
              </div>
      
</asp:Content>
