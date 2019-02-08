<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Perdida_Referencias.aspx.cs" Inherits="Rinya_Albal_App.Comercial.Perdida_Referencias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row">
        <div class="col-md-8">
            <h2>Informe Perdida de Referencias</h2>
            <div class="form-group">
                 <div class="col-md-2">
                       
                       <asp:Button ID="Btexport" runat="server" Text="Exportar Excel" CssClass="btn btn-default" OnClick="Btexport_Click" /></div>  <div class="col-md-2">
                     </div>
                </div>
            </div>
         </div>
</asp:Content>
