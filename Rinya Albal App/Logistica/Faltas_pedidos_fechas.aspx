<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Faltas_pedidos_fechas.aspx.cs" Inherits="Rinya_Albal_App.Logistica.Faltas_pedidos_fechas" %>
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
   semana_dia.setDate(hoy.getDate() - 7);
                 
                 $("#<%= datepicker1.ClientID %>").datepicker().datepicker("setDate", semana_dia);

                 $("#<%= datepicker2.ClientID %>").datepicker().datepicker("setDate", new Date());
  if (!($("[id$=datepicker1]").datepicker().attr("Value") == undefined)) {
            
             if ($("[id$=datepicker1]").datepicker().attr("Value").length > 0) {
                 $("[id$=datepicker1]").datepicker().datepicker("setDate", new Date($("[id$=datepicker1]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                 $("[id$=datepicker2]").datepicker().datepicker("setDate", new Date($("[id$=datepicker2]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                
             }
             
         }
     });
        </script>
    <script type="text/javascript" >
    function deshabilitar(boton) {
       document.getElementById(boton).style.visibility = 'hidden';
    }
</script>

           <div class="form-group">
                  <h2>Faltas en pedidos</h2>
                <div class="row">
                         <div class="panel-body">
            <div class="form-group">
              <div class="container">
                    <div class="row">
                        <div class="col-xs-2 col-md-4"><asp:Label runat="server" AssociatedControlID="datepicker1" CssClass="control-label">Desde:</asp:Label></div>
                        <div class="col-xs-2 col-md-4"> <asp:Label runat="server" AssociatedControlID="datepicker2" CssClass="control-label" >Hasta:</asp:Label> </div>                   
                    </div>
                    <div class="row " >
                            <div class="col-xs-2 col-md-4"> <asp:TextBox ID="datepicker1" runat="server"  placeholder="Desde" CssClass=" form-control"></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:TextBox ID="datepicker2"  placeholder="Hasta" runat="server" CssClass=" form-control"></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:Button ID="BtBuscar" runat="server" Text="Descargar" CssClass="btn btn-default"  OnClick="Btexport_Click" /></div>                       
                          </div>   
                    </div>
            </div>
                               </div>
            </div>
               </div>
        

</asp:Content>
