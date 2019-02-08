<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Listadp_formulas.aspx.cs" Inherits="Rinya_Albal_App.Produccion.Listadp_formulas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-group">
                  <h2>Listado Despiece Formulacion</h2>
                <div class="row">
                         <div class="panel-body">
            <div class="form-group">
              <div class="container">
                    <div class="row">
    <asp:Button ID="BtBuscar" runat="server" Text="Descargar" CssClass="btn btn-default"  OnClick="Btexport_Click" />
                        </div>
                               </div>
            </div>
               </div>
                    </div>
        </div>
</asp:Content>
