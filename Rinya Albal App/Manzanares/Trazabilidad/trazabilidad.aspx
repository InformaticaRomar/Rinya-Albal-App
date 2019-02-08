<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="trazabilidad.aspx.cs" Inherits="Rinya_Albal_App.Manzanares.trazabilidad" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
              <td>&nbsp;</td>
             </tr>
            
        <tr class="form-horizontal">
             
            <th><h4> Lote:</h4></th>
         <td >
              <asp:Panel runat="server" DefaultButton="Button1">
             <asp:TextBox ID="TBLabel" runat="server" CssClass="form-control" TextMode="SingleLine" />
                   <asp:Button ID="Button1" runat="server" OnClick="BtBuscar_Click" Style="display: none" />
            </asp:Panel>
        </td>
            
            <td><asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default"  OnClick="BtBuscar_Click" /></td>
              <td><asp:Button ID="Btexport" runat="server" Text="Exportar" CssClass="btn btn-default"    Enabled="false" OnClick="Btexport_Click"/></td>
            
        </tr>
        <tr>
              <td>&nbsp;</td>
             </tr>
        <asp:GridView ID="GridView1" Visible="false" CssClass="table table-striped table-bordered table-hover"  EmptyDataText="No se han encontrado datos"  runat="server"></asp:GridView>
   

    </table>
</asp:Content>
