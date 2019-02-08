<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos_Tablet.aspx.cs" Inherits="Rinya_Albal_App.Pedidos.Pedidos_Tablet" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-ui/jquery-ui.js"></script>

    <script type="text/javascript"> 
     $.datepicker.regional['es'] = {
           closeText: 'Cerrar',
           prevText: '<Ant',
           nextText: 'Sig>',
           currentText: 'Hoy',
           monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
           monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
           dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
           dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
           dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
           weekHeader: 'Sm',
           dateFormat: 'dd-mm-yy',
           firstDay: 1,
           isRTL: false,
           showMonthAfterYear: false,
           yearSuffix: ''
       };
     $.datepicker.setDefaults($.datepicker.regional['es']);
     $(document).ready(function () {
   var hoy = new Date();
                 var semana_dia = new Date();
                 semana_dia.setDate(semana_dia.getDate() - 2);
                 $("#<%= datepicker1.ClientID %>").datepicker().datepicker("setDate", semana_dia);

                 $("#<%= datepicker2.ClientID %>").datepicker().datepicker("setDate", new Date());
  if (!($("[id$=datepicker1]").datepicker().attr("Value") == undefined)) {
            
             if ($("[id$=datepicker1]").datepicker().attr("Value").length > 0) {
                 $("[id$=datepicker1]").datepicker().datepicker("setDate", new Date($("[id$=datepicker1]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                 $("[id$=datepicker2]").datepicker().datepicker("setDate", new Date($("[id$=datepicker2]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                // alert($("[id$=datepicker1]").datepicker().attr("Value"));
                 //alert($("[id$=datepicker2]").datepicker().attr("Value"));
             }
             
         }
     });
        </script>
    <div class="row">
        <div class="col-md-8">
            <h2>CONTROL PEDIDOS TABLET</h2>
            <div class="form-group">
               
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" CssClass="col-md-2 control-label" >Desde</asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Hasta</asp:Label>
                    </div>
                      <div class="col-md-2">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Comercial</asp:Label>
                    </div>
                </div>
                <div class="row">

                    <div class="col-md-2">
                          <input  type="text"  id="datepicker1" Class="form-control"  runat="server" EnableViewState="true" style="width: 98px">

                       
                    </div>

                    <div class="col-md-2">
                         <input  type="text" id="datepicker2" Class="form-control"  runat="server" EnableViewState="true" style="width: 98px">
                       
                    </div>
                    <div class="col-md-4">
                       
                         <asp:DropDownList ID="comercial" CssClass="form-control" runat="server"></asp:DropDownList>
                       
                    </div>
                    <div class="col-md-2">
                       
                        <asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default"   /></div>  <div class="col-md-2">
                    <asp:Button ID="Btexport" runat="server" Text="Exportar Excel" CssClass="btn btn-default" OnClick="Btexport_Click" /> </div>
                </div>

                <div class="row">
                    <hr />
                    <div class="container">
                        <asp:ListView ID="lvPedidos" runat="server" GroupPlaceholderID="groupPlaceHolder1" ItemPlaceholderID="itemPlaceHolder1" OnPagePropertiesChanging="OnPagePropertiesChanging">
<LayoutTemplate>
    <table class="table" style="clear: both; border-collapse: collapse;">
        <tr>
            <th>
                Cliente
            </th>
            <th>
               Sucursal
            </th>
            <th>
                Fecha Pedido
            </th>
             <th>
               Nº Pedido
            </th>
             <th>
               Pedido Tablet
            </th>
             <th>
               Representante
            </th>
             <th>
               Observaciones
            </th>
        </tr>
        <asp:PlaceHolder runat="server" ID="groupPlaceHolder1"></asp:PlaceHolder>
        <tr>
            <td colspan = "7">
                <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lvPedidos" PageSize="10">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="false" ShowPreviousPageButton="true"
                            ShowNextPageButton="false" />
                        <asp:NumericPagerField ButtonType="Link" />
                        <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="false" ShowPreviousPageButton = "false" />
                    </Fields>
                </asp:DataPager>
            </td>
        </tr>
    </table>
</LayoutTemplate>
<GroupTemplate>
    <tr>
        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
    </tr>
</GroupTemplate>
<ItemTemplate>
    <td>
        <%# Eval("WCPC_CLIENTE_ERP") %>
    </td>
    <td>
        <%# Eval("WCPC_SUC_RECEPTOR_MERC") %>
    </td>
    <td>
        <%# Eval("WCPC_FECHAPED") %>
    </td>
    <td>
        <%# Eval("WCPC_NUMERO_PEDIDO_ERP") %>
    </td>
     <td>
        <%# Eval("WCPC_NUMPED") %>
    </td>
    <td>
        <%# Eval("WCPC_REPRESENTANTE") %>
    </td>
    <td>
        <%# Eval("WCPC_OBSERVACIONES") %>
    </td>
</ItemTemplate>
                            </asp:ListView>
                        <!-- <div class="container" style="overflow: scroll;HEIGHT:100%">-->

                       
                    </div>
                    <!-- </div> -->
                </div>
            </div>
        </div>
    </div>
</asp:Content>
