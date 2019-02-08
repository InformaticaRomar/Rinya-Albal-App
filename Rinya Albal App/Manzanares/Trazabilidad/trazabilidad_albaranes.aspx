<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="trazabilidad_albaranes.aspx.cs" Inherits="Rinya_Albal_App.Manzanares.trazabilidad_albaranes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
      <script src="../Scripts/jquery-ui/jquery-ui.js"></script>
     <script src="../Scripts/jquery-jtemplates.js"></script>
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
   var semana_dia = new Date();// new Date(hoy.getFullYear(), 0, 1);
  
   semana_dia.setDate(semana_dia.getDate() - 30);
                // semana_dia.setDate(semana_dia.getDate() - 2);
                 
                 $("#<%= datepicker_1.ClientID %>").datepicker().datepicker("setDate", semana_dia);

                 $("#<%= datepicker_2.ClientID %>").datepicker().datepicker("setDate", new Date());
  if (!($("[id$=datepicker_1]").datepicker().attr("Value") == undefined)) {
            
             if ($("[id$=datepicker_1]").datepicker().attr("Value").length > 0) {
                 $("[id$=datepicker_1]").datepicker().datepicker("setDate", new Date($("[id$=datepicker_1]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                 $("[id$=datepicker_2]").datepicker().datepicker("setDate", new Date($("[id$=datepicker_2]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                // alert($("[id$=datepicker_1]").datepicker().attr("Value"));
                 //alert($("[id$=datepicker_2]").datepicker().attr("Value"));
             }
             
         }
     });
        </script>
       <div class="panel panel-default">
<div class="panel-heading">
            <h3 class="panel-title">Busqueda</h3>
          </div>

     <div class="panel-body">
            <div class="form-group">
              <div class="container">
                    <div class="row">
                        <div class="col-xs-2 col-md-4"><asp:Label runat="server" AssociatedControlID="datepicker_1" CssClass="control-label">Desde:</asp:Label></div>
                        <div class="col-xs-2 col-md-4"> <asp:Label runat="server" AssociatedControlID="datepicker_2" CssClass="control-label" >Hasta:</asp:Label> </div>                   
                    </div>
                    <div class="row " >
                            <div class="col-xs-2 col-md-4"> <asp:TextBox ID="datepicker_1" runat="server"   CssClass=" form-control"></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:TextBox ID="datepicker_2"   runat="server" CssClass=" form-control"></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default"  OnClick="BtBuscar_Click"/></div>                       
                          </div>   
                      
           
                    <div class="row">
                         
                            <div class="col-xs-2 col-md-4"><asp:Label id="label_sscc" runat="server" AssociatedControlID="Nalbaran" CssClass="control-label">Nº Albaran:</asp:Label></div>
                      
                            <div class="col-xs-2 col-md-4"><asp:TextBox ID="Nalbaran"  runat="server" CssClass="form-control" ></asp:TextBox></div>
                            <div class="col-xs-2 col-md-4"><asp:Button ID="Button1" runat="server" Text="Buscar" CssClass="btn btn-default"  OnClick="BtBuscar_Click" /></div>  
                         <div class="col-xs-2 col-md-4"><asp:Button ID="Btexport" runat="server" Text="Exportar" CssClass="btn btn-default"    Enabled="false" OnClick="Btexport_Click"/>  </div>
                          </div>
                      

               

               </div>
            </div>
            </div>
        <asp:GridView ID="GridView1" Visible="false" CssClass="table table-striped table-bordered table-hover"  EmptyDataText="No se han encontrado datos"  runat="server"></asp:GridView>
   
</div>
  
</asp:Content>
