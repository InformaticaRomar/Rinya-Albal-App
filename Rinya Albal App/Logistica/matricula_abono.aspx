<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="matricula_abono.aspx.cs" Inherits="Rinya_Albal_App.Logistica.matricula_abono" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
   
    <div class="row">
        <div class="col-md-8">
            <h2>Matricula Abono</h2>
            <div class="form-group">
                <div class="row">
                    <div class="col-md-8">
                        <asp:Label runat="server" AssociatedControlID="OC_textBoxs" CssClass="col-md-8 control-label">Matricula:</asp:Label>
                    </div>
                    
                </div>
                <div class="row">

                    <div class="col-md-4">

                        <asp:TextBox ID="OC_textBoxs" minlength="18" maxlength="20" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" /></div>

                </div>
                </div>
            </div>
        </div>
   
</asp:Content>
