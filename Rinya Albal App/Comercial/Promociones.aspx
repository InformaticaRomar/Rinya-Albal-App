<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Promociones.aspx.cs" Inherits="Rinya_Albal_App.Comercial.Promociones" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
    <div class="row">
        <div class="col-md-8">
            <h2>Promociones Expert</h2>
           <table>
        <tr>
              <td>&nbsp;</td>
             </tr>
            
        <tr class="form-horizontal">

                      <th><h4><asp:Label runat="server" AssociatedControlID="CL_textBox" CssClass="control-label">Cliente:</asp:Label> </h4></th>
                  
               

                    <td >

                        <asp:TextBox ID="CL_textBox"  CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                    <td >
                        <asp:Label runat="server" AssociatedControlID="ART_textBox" CssClass="col-md-8 control-label">Articulo:</asp:Label>
                    </td>
                    <td >

                        <asp:TextBox ID="ART_textBox"  CssClass="form-control" runat="server"></asp:TextBox>
                    </td>
                    <td >
                        <asp:Label runat="server"  AssociatedControlID="tarifa_textBox" CssClass="col-md-8 control-label">Tarifa:</asp:Label>
                    </td>
                    <td >

                        <asp:TextBox ID="tarifa_textBox" CssClass="form-control" runat="server"></asp:TextBox>
                     </td>
                    <td >
                        <asp:Button ID="BtBuscar" runat="server" Text="Exportar" CssClass="btn btn-default" OnClick="BtBuscar_Click" />
         </td>
                    <td >
                        </table>
            </div>
        </div>
    <table>
      
            <tr >
                      <th>  <asp:Label runat="server" id="Respuesta_1_lbl"  Visible="false" CssClass=" control-label">Procesando...</asp:Label></th>
                    </tr>
        
            <tr >
                       <th> <asp:Label runat="server" id="Respuesta_2_lbl"  Visible="false" CssClass=" control-label">Por favor espere</asp:Label></th>
                    </tr>
        </table>
</asp:Content>
