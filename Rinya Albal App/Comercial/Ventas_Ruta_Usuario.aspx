<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ventas_Ruta_Usuario.aspx.cs" Inherits="Rinya_Albal_App.Comercial.Ventas_Ruta_Usuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <link href="../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-ui/jquery-ui.js"></script>

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
   var semana_dia = new Date(hoy.getFullYear(),0, 1);
                // semana_dia.setDate(semana_dia.getDate() - 2);
                 
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
            <h2>Informe Ventas Articulo Pedido</h2>
            <div class="form-group">
               
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" CssClass="col-md-2 control-label" >Desde</asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Hasta</asp:Label>
                    </div>
                     
                </div>
                <div class="row">

                    <div class="col-md-2">
                          <input  type="text"  id="datepicker1" Class="form-control"  runat="server" EnableViewState="true" style="width: 98px">

                       
                    </div>

                    <div class="col-md-2">
                         <input  type="text" id="datepicker2" Class="form-control"  runat="server" EnableViewState="true" style="width: 98px">
                       
                    </div>
                   
                    <div class="col-md-2">
                       
                       <asp:Button ID="Btexport" runat="server" Text="Exportar Excel" CssClass="btn btn-default" OnClick="Btexport_Click" /></div>  <div class="col-md-2">
                     </div>
                </div>
                </div>
            </div>
        </div>
    </asp:Content>