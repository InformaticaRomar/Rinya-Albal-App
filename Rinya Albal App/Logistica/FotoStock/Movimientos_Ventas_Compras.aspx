<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Movimientos_Ventas_Compras.aspx.cs" Inherits="Rinya_Albal_App.Logistica.FotoStock.Movimientos_Ventas_Compras" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
             <link href="../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-ui/jquery-ui.js"></script>
       <link href="../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <script src="../../Scripts/jquery-jtemplates.js"></script>
    <script src="../../Scripts/jquery-ui-timepicker-addon.js"></script>
    <link href="../../Scripts/jquery-ui-timepicker-addon.css" rel="stylesheet" type="text/css"/>
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
             dateFormat: 'dd/mm/yy',
             firstDay: 1,
             isRTL: false,
             showMonthAfterYear: false,
             yearSuffix: ''
         };
         
         $.datepicker.setDefaults($.datepicker.regional['es']);
         $.timepicker.regional['es'] = {
             	                timeOnlyTitle: 'Elegir una hora',
             	                timeText: 'Hora',
             	                hourText: 'Horas',
                              minuteText: 'Minutos',
             	                secondText: 'Segundos',
             	                millisecText: 'Milisegundos',
             	                timezoneText: 'Huso horario',
             	                currentText: 'Ahora',
             	                closeText: 'Cerrar',
             	                timeFormat: 'HH:mm',
             	                amNames: ['a.m.', 'AM', 'A'],
             	                pmNames: ['p.m.', 'PM', 'P'],
             	                isRTL: false
                 };
         $.timepicker.setDefaults($.timepicker.regional['es']);

         $(document).ready(function () {
             $('#<%= datepicker1.ClientID %>').datetimepicker();
             
             $('#<%= datepicker2.ClientID %>').datetimepicker();
            

         });

      
         </script>
      <div class="row">
        <div class="col-md-8">
            <h2>Compras - Ventas </h2>
            <div class="form-group">
               
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" CssClass="col-md-2 control-label" >Desde</asp:Label>
                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Hasta</asp:Label>
                    </div>
                     
                </div>
                <div class="row">

                    <div class="col-md-4">
                          <input  type="text"  id="datepicker1" Class="form-control"  runat="server" EnableViewState="true" style="width: 248px">

                       
                    </div>

                    <div class="col-md-4">
                         <input  type="text" id="datepicker2" Class="form-control"  runat="server" EnableViewState="true" style="width: 248px">
                       
                    </div>
                   
                    <div class="col-md-2">
                       
                       <asp:Button ID="Btexport" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="Btexport_Click"  />
                     </div>
                </div>
                </div>
            </div>
        </div>
</asp:Content>
