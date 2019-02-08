<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirming.aspx.cs" Inherits="Rinya_Albal_App.Manzanares.Finanzas.Confirming" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <link href="../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
     
    <script src="../../Scripts/jquery-ui/jquery-ui.js"></script>
     <script src="../../Scripts/jquery-jtemplates.js"></script>

     <script type="text/javascript"> 
        
         function loadComboBox(methodname, comboname, selected) {

             var template = "{#foreach $T as record}\
                        <option value='{$T.record.VALOR}'>{$T.record.VALOR}</option>\
                    {#/for}";

             var combo = $("[id$="+comboname+"]" );

             $.ajax({
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 url: "WebService_finanzas.asmx/" + methodname,
                 data: '{}',
                 dataType: "json",

                 success: function (data) {


                     combo.setTemplate(template);
                     combo.processTemplate(JSON.parse(data.d));

                 },
                 error: function (request, status, error) {
                     alert(JSON.parse(request.responseText).Message);
                 }
             })

             //combo.val(selected).change();
             combo.selectmenu();
             combo.val(selected).change();
             
             // combo.selectmenu().attr("selectedIndex", selected);
            // combo.trigger("chosen:updated").selectmenu().selectmenu('refresh', true);
            // combo.selectmenu('refresh', true);
            // combo.select2();




         }

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
     $(document).ready(function () {
         loadComboBox("GetEntidad", "txtEntidad_", 1);
         loadComboBox("GetEmpresa", "txtEmpresa", 2);
         
   var hoy = new Date();
   var semana_dia = new Date();
   semana_dia.setDate(semana_dia.getDate() + 1);


                         
   $("#<%= txtfecha_emision.ClientID %>").datepicker().datepicker("setDate", new Date());
        
         $("#<%= txtfecha_pago.ClientID %>").datepicker().datepicker("setDate", semana_dia);
         if (!($("[id$=txtfecha_emision]").datepicker().attr("Value") == undefined)) {
            
             if ($("[id$=txtfecha_emision]").datepicker().attr("Value").length > 0) {
                 $("[id$=txtfecha_emision]").datepicker().datepicker("setDate", new Date($("[id$=txtfecha_emision]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                 $("[id$=txtfecha_pago]").datepicker().datepicker("setDate", new Date($("[id$=txtfecha_pago]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
              
             }
             
         }
     });
        </script>
      <h2>Confirming</h2>
    <!--<asp:Label runat="server" ID="Sql_">sql </asp:Label>-->
    <div class="panel panel-default">
     <div class="panel-heading">
            <h3 class="panel-title">Datos</h3>
         
          </div>

     <div class="panel-body">
            <div class="form-group">
              <div class="container">
                     <div class="row">
            <div class="col-lg-8 col-md-8">
                <label for="txtfecha_emision">Fecha de Emisión:&nbsp;</label>
                <asp:TextBox ID="txtfecha_emision" runat="server"  CssClass=" form-control  required"></asp:TextBox>
                   
                <label for="txtEntidad_">Entidad:&nbsp;</label>
              <div  class="cell">
                   <select  id="txtEntidad_" runat="server" name="txtEntidad_" class="form-control required" required ></select></div>
                <label for="txtfecha_pago">Fecha Pago:&nbsp;</label>
                   <asp:TextBox ID="txtfecha_pago" runat="server" CssClass="form-control required"></asp:TextBox>
                    <label for="txtProveedor">Proveedor:&nbsp;</label>
                    <input runat="server" type="text" id="txtProveedor" name="txtProveedor" class="form-control required" placeholder="Indica el Proveedor" required >    
                   <label for="txtFactura">Nº Factura:&nbsp;</label>
                    <input runat="server" type="text" id="txtFactura" name="txtFactura" class="form-control required" placeholder="Indica Numero Factura" required>    
                  
                <label for="txtEmpresa">Empresa:&nbsp;</label>
                 <select  runat="server" id="txtEmpresa" name="txtEmpresa" class="form-control required" required ></select>
                    
                <label for="txtImporte">Importe:&nbsp;</label>
                    <input runat="server" type="number" step="any"  id="txtImporte" name="txtImporte" class="form-control" placeholder="Indica Importe" required>
                <!-- <label for="txtNominal">Nominal:&nbsp;</label>
                    <input runat="server" type="number" step="any"  id="txtNominal" name="txtNominal" class="form-control" placeholder="Indica Importe Nominal"  >-->
                <label for="txtPagado">Pagado:&nbsp;</label>
                    <input runat="server" type="number" step="any"  id="txtPagado" name="txtPagado" class="form-control" placeholder="Indica Importe Pagado" >
               
            </div>
            <p>&nbsp;</p>
            <div class="form-group text-center">
                <asp:Button ID="btnEnviar" runat="server" class="btn btn-lg btn-primary" Text="Grabar Datos" OnClick="btnEnviar_Click"  />
            </div>
        </div>
                  

               </div>
            </div>
            </div>
           
    </div>
</asp:Content>
