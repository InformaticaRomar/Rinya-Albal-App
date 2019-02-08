<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mensajes_Expert_Quality.aspx.cs" Inherits="Rinya_Albal_App.Mensajeria.Mensajes_Expert_Quality" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script  type="text/JavaScript">
    $(document).ready(function(){
        $("#Combotipo").change(function (event) {
            var valor_combo = $("#Combotipo").val(); // document.getElementById('select').value
           // alert(valor_combo);
            switch (valor_combo){
                case '1':
                    document.getElementById('LBBuscar').innerHTML = ' Articulo:';
                    break;
                case '2':
                    document.getElementById('LBBuscar').innerHTML = ' Cliente:';
                    break;
                case '3':
                    document.getElementById('LBBuscar').innerHTML = ' Proveedor:';
                    break;
                case '4':
                    document.getElementById('LBBuscar').innerHTML = ' P Compra:';
                    break;
                case '5':
                    document.getElementById('LBBuscar').innerHTML = ' P Venta:';
                    break;
                case '6':
                    document.getElementById('LBBuscar').innerHTML = ' Articulo Cliente:';
                    break;
                case '7':
                    document.getElementById('LBBuscar').innerHTML = ' Precio Venta:';
                    break;
            }
            
        });
    });
</script>
     <table>
        <tr>
              <td>&nbsp;</td>
             </tr>
            
        <tr class="form-horizontal">
             
            <th><h4> Tipo:</h4></th><th> <select runat="server" id="Combotipo" class="js-example-basic-single"  >
                <option value=1>Maestro Articulo</option>
                <option value=2>Maestro Cliente</option>
                <option value=3>Maestro Proveedor</option>
                <option value=4>Pedido Compra</option>
                <option value=5>Pedido Venta</option>
                <option value=6>Articulo Cliente</option>
                <option value=7>Precio Venta</option>
                                         </select></th>
            <td> <label   id="LBBuscar"> Articulo:</label ></td>
         <td >
              <asp:Panel runat="server" DefaultButton="Button1">
             <asp:TextBox ID="TBLabel" runat="server" CssClass="form-control" TextMode="SingleLine" />
                   <asp:Button ID="Button1" runat="server"  Style="display: none" />
            </asp:Panel>
        </td>
            
            <td><asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="BtBuscar_Click"  /></td>
              <td><asp:Button ID="Btexport" runat="server" Text="Exportar" CssClass="btn btn-default"    Enabled="false" /></td>
            
        </tr>
        <tr>
              <td>&nbsp;</td>
             </tr>
        <asp:GridView ID="GridView1" Visible="false" CssClass="table table-striped table-bordered table-hover"  EmptyDataText="No se han encontrado datos"  runat="server"></asp:GridView>
   

    </table>
</asp:Content>
