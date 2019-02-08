<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Informe_Confirming.aspx.cs"  EnableEventValidation="true" Inherits="Rinya_Albal_App.Manzanares.Finanzas.Informes.Informe_Confirming" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <link href="../../../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
      <script src="../../../Scripts/jquery-ui/jquery-ui.js"></script>
     <script src="../../../Scripts/jquery-jtemplates.js"></script>
     <script type="text/javascript">
         function loadComboBox(methodname, comboname, selected) {

             var template = "{#foreach $T as record}\
                        <option value='{$T.record.VALOR}'>{$T.record.VALOR}</option>\
                    {#/for}";

             var combo = $("[id$=" + comboname + "]");

             $.ajax({
                 type: "POST",
                 contentType: "application/json; charset=utf-8",
                 url: "../WebService_finanzas.asmx/" + methodname,
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
           dateFormat: 'dd-mm-yy',
           firstDay: 1,
           isRTL: false,
           showMonthAfterYear: false,
           yearSuffix: ''
       };
     $.datepicker.setDefaults($.datepicker.regional['es']);
     $(document).ready(function () {
         loadComboBox("GetEntidad", "txtEntidad_", 1);
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

             }
         }
        $("#<%= datepicker_1b.ClientID %>").datepicker().datepicker("setDate", semana_dia);

                 $("#<%= datepicker_2b.ClientID %>").datepicker().datepicker("setDate", new Date());
  if (!($("[id$=datepicker_1b]").datepicker().attr("Value") == undefined)) {
            
             if ($("[id$=datepicker_1b]").datepicker().attr("Value").length > 0) {
                 $("[id$=datepicker_1b]").datepicker().datepicker("setDate", new Date($("[id$=datepicker_1b]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                 $("[id$=datepicker_2b]").datepicker().datepicker("setDate", new Date($("[id$=datepicker_2b]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                
             }
             
  }
                 $("#<%= datepicker_1c.ClientID %>").datepicker().datepicker("setDate", semana_dia);

                 $("#<%= datepicker_2c.ClientID %>").datepicker().datepicker("setDate", new Date());
  if (!($("[id$=datepicker_1c]").datepicker().attr("Value") == undefined)) {
            
             if ($("[id$=datepicker_1c]").datepicker().attr("Value").length > 0) {
                 $("[id$=datepicker_1c]").datepicker().datepicker("setDate", new Date($("[id$=datepicker_1c]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                 $("[id$=datepicker_2c]").datepicker().datepicker("setDate", new Date($("[id$=datepicker_2c]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
                
             }
             
         }
     });
        </script>
     <h3>Informacion Confirming</h3>
   <div class="panel-heading">
            <h3 class="panel-title">Busqueda</h3>
          </div>

     <div class="panel-body">
            <div class="form-group">
              <div class="container">
                    <div class="row">
                           <label for="txtEntidad_">Entidad:&nbsp;</label>
              <div >
                   <select  id="txtEntidad_" runat="server" name="txtEntidad_" class="form-control required" required >
                       
                   </select></div>
                        <div ><asp:Button ID="Button4" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="Button4_Click"  /></div>
         </div>
               <div class="row"><asp:Label runat="server" CssClass="control-label">Fecha Emision:</asp:Label></div>
                        <div class="col-xs-2 col-md-4">
                            <asp:Label runat="server" AssociatedControlID="datepicker_1" CssClass="control-label">Desde:</asp:Label>
                            <asp:TextBox ID="datepicker_1" runat="server"   CssClass=" form-control"></asp:TextBox>
                        </div>
                        <div class="col-xs-2 col-md-4">
                             <asp:Label runat="server" AssociatedControlID="datepicker_2" CssClass="control-label" >Hasta:</asp:Label> 
                            <asp:TextBox ID="datepicker_2"   runat="server" CssClass=" form-control"></asp:TextBox>
                        </div>  
                                      
                    </div>
                        <div class="row"> <div class="col-xs-2 col-md-4"><asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default"  OnClick="BtBuscar_Click"/></div>  </div>  
                  </div>
               
    <p>&nbsp;</p> 
         <div class="container">
                    <div class="row">
                        <div class="row"> <asp:Label runat="server" CssClass="control-label">Fecha Pago:</asp:Label></div>
                        <div class="col-xs-2 col-md-4">
                            <asp:Label runat="server" AssociatedControlID="datepicker_1b" CssClass="control-label">Desde:</asp:Label>
                            <asp:TextBox ID="datepicker_1b" runat="server"   CssClass=" form-control"></asp:TextBox>
                        </div>
                        <div class="col-xs-2 col-md-4">
                             <asp:Label runat="server" AssociatedControlID="datepicker_2b" CssClass="control-label" >Hasta:</asp:Label> 
                            <asp:TextBox ID="datepicker_2b"   runat="server" CssClass=" form-control"></asp:TextBox>
                        </div>  
                                      
                    </div>
                        <div class="row"> <div class="col-xs-2 col-md-4"><asp:Button ID="Button2" runat="server" Text="Buscar" CssClass="btn btn-default"  OnClick="BtBuscar_2_Click"/></div>  </div>  
                  </div>
                 <div class="container">
                    <div class="row">
                        <div class="row"> <asp:Label runat="server" CssClass="control-label">Fecha Vencimiento:</asp:Label></div>
                        <div class="col-xs-2 col-md-4">
                            <asp:Label runat="server" AssociatedControlID="datepicker_1c" CssClass="control-label">Desde:</asp:Label>
                            <asp:TextBox ID="datepicker_1c" runat="server"   CssClass=" form-control"></asp:TextBox>
                        </div>
                        <div class="col-xs-2 col-md-4">
                             <asp:Label runat="server" AssociatedControlID="datepicker_2c" CssClass="control-label" >Hasta:</asp:Label> 
                            <asp:TextBox ID="datepicker_2c"   runat="server" CssClass=" form-control"></asp:TextBox>
                        </div>  
                                      
                    </div>
                        <div class="row"> <div class="col-xs-2 col-md-4"><asp:Button ID="Button1" runat="server" Text="Buscar" CssClass="btn btn-default"  OnClick="BtBuscar_3_Click"/></div>  </div>  
                  </div>
          </div>
         <div class="row"> <div class="col-xs-2 col-md-4"><asp:Button ID="Button3" runat="server" Text="Excel" CssClass="btn btn-default"  OnClick="BtBuscar_4_Click" Height="36px"/></div>  </div>  

       
    
    <h3>
        <span style="float:left;"><asp:Label ID="lblInfo" runat="server" /></span>
         <span style="float:right;"><small>Total Lineas:</small> <asp:Label ID="lblTotalClientes" runat="server" CssClass="label label-warning" /></span>
    
    </h3>
    <p>&nbsp;</p>
     <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:QC600ConnectionString %>" 
        DeleteCommand="DELETE FROM [CONFIRMING] WHERE [ID] = @ID" 

              SelectCommand="SELECT   [ID]
      ,Convert(varchar,[FECHA_EMISION],103) as [FECHA_EMISION]
      ,[ENTIDAD]
      , Convert(varchar,[FECHA_PAGO],103) as [FECHA_PAGO]
      ,[PROVEEDOR]
      ,[N_FACTURA]
      ,[EMPRESA]
      ,[IMPORTE]
      ,Convert(varchar,[FECHA_VENCIMIENTO],103) as [FECHA_VENCIMIENTO]
      ,[MES_VENCIMIENTO]
      ,[NOMINAL]
      ,[PAGADO] FROM [CONFIRMING]" 
         onselected="SqlDataSource1_Selected"
        UpdateCommand="UPDATE [CONFIRMING] SET [ENTIDAD] = @Entidad,
        [PROVEEDOR]=@proveedor,[N_FACTURA]=@N_factura, [EMPRESA]=@empresa,[IMPORTE]=@importe
       WHERE [ID] = @ID" OnUpdating="SqlDataSource1_Updating">
        <DeleteParameters>
            <asp:Parameter Name="ID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
             <asp:Parameter Name="Entidad" Type="String" />
           
            <asp:Parameter Name="proveedor" Type="String" />
            <asp:Parameter Name="N_factura" Type="String" />
            <asp:Parameter Name="empresa" Type="String" />
            <asp:Parameter Name="importe" Type="Double" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:Panel ID="Panel_grid" runat="server" Visible="False">
    <asp:GridView ID="GridView_Confirming"  runat ="server"  DataSourceID="SqlDataSource1"
        AutoGenerateColumns="False" 
        CssClass="table table-bordered bs-table" 
        DataKeyNames="ID" 
        OnRowDeleted="GridView_Confirming_RowDeleted" 
        OnRowUpdated="GridView_Confirming_RowUpdated" 
        OnRowEditing="GridView_Confirming_RowEditing" 
        OnDataBound="GridView_Confirming_DataBound" 
        onpageindexchanging="GridView1_PageIndexChanging"
        allowpaging="true" OnPreRender="GridView_Confirming_PreRender" 
       OnRowCommand="GridView_Confirming_RowCommand" >
 
        <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        <emptydatatemplate>
            ¡No hay Lineas de confirming pendientes!  
        </emptydatatemplate>           
 
        <%--Paginador...--%>        
        <pagertemplate>
        <div class="row" style="margin-top:20px;">
            <div class="col-lg-1" style="text-align:right;">
                <h5><asp:label id="MessageLabel" text="Ir a la pág." runat="server" /></h5>
            </div>
             <div class="col-lg-1" style="text-align:left;">
                <asp:dropdownlist id="PageDropDownList" Width="50px" autopostback="true" onselectedindexchanged="PageDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control" /></h3>
            </div>
            <div class="col-lg-10" style="text-align:right;">
                <h3><asp:label id="CurrentPageLabel" runat="server" CssClass="label label-warning" /></h3>
            </div>
        </div>        
        </pagertemplate>             
 
        <Columns>
            <%--CheckBox para seleccionar varios registros...--%>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                <ItemTemplate>
                    <asp:CheckBox ID="chkEliminar" runat="server" AutoPostBack="true" OnCheckedChanged="chk_OnCheckedChanged" />
                </ItemTemplate>
            </asp:TemplateField>            
 
            <%--botones de acción sobre los registros...--%>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px">
                <ItemTemplate>
                    <%--Botones de eliminar y editar cliente...--%>
    
                    <asp:Button ID="btnDelete" runat="server" Text="Quitar" CssClass="btn btn-danger" CausesValidation="true" OnClientClick="if (!confirm('¿Seguro que deseas borrar el confirming?')) return false;" CommandName="Delete"   UseSubmitBehavior="False"/>
                    <asp:Button ID="btnEdit" runat="server" Text="Editar" CssClass="btn btn-info" CommandName="Edit"   CausesValidation="true" UseSubmitBehavior="False" />
                </ItemTemplate>
                <edititemtemplate>
                    <%--Botones de grabar y cancelar la edición de registro...--%>
                        <asp:Button ID="btnUpdate" runat="server" Text="Grabar" CssClass="btn btn-success" CommandName="Update" OnClientClick="return confirm('¿Seguro que quiere modificar los datos de pago?');" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="btn btn-default" CommandName="Cancel"  CausesValidation="true" UseSubmitBehavior="False"/>
                </edititemtemplate>
            </asp:TemplateField>
 
            <%--campos no editables...--%>
            <asp:BoundField DataField="ID" HeaderText="Nº" InsertVisible="False" ReadOnly="True" SortExpression="ID" ControlStyle-Width="70px" />
            <asp:BoundField DataField="FECHA_EMISION" HeaderText="F. Emision" InsertVisible="False" ReadOnly="True" SortExpression="FECHA_EMISION" ControlStyle-Width="70px" />
             <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Entidad">
                <ItemTemplate>
                  <%# DataBinder.Eval(Container.DataItem,"ENTIDAD")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TxtEntidad" runat="server" Text='<%# Bind("ENTIDAD")%>' CssClass="form-control" ></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="FECHA_PAGO" HeaderText="F. Pago" ReadOnly="True"  />
            
            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Proveedor">
                <ItemTemplate>
                     <%# DataBinder.Eval(Container.DataItem,"PROVEEDOR")%>
                    
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TxtPROVEEDOR" runat="server" Text='<%# Bind("PROVEEDOR")%>' CssClass="form-control" ></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Nº Factura">
                <ItemTemplate>
                     <%# DataBinder.Eval(Container.DataItem,"N_FACTURA")%>
                   
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TxtN_FACTURA" runat="server" Text='<%# Bind("N_FACTURA")%>' CssClass="form-control" ></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Empresa">
                <ItemTemplate>
                      <%# DataBinder.Eval(Container.DataItem,"EMPRESA")%>
                   
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TxtEMPRESA" runat="server" Text='<%# Bind("EMPRESA")%>' CssClass="form-control" ></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Importe €">
                <ItemTemplate>
                       <%# DataBinder.Eval(Container.DataItem,"IMPORTE")%>
                   
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TxtIMPORTE" runat="server" Text='<%# Bind("IMPORTE")%>' CssClass="form-control" ></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
          
            <asp:BoundField DataField="FECHA_VENCIMIENTO" HeaderText="F. Vencimiento" ReadOnly="True"  />
            
            <asp:BoundField DataField="MES_VENCIMIENTO" HeaderText="Mes" ReadOnly="True"  />
            <asp:BoundField DataField="NOMINAL" HeaderText="Nominal" ReadOnly="True"  />
            <asp:BoundField DataField="PAGADO" HeaderText="Pagado" ReadOnly="True"/>
            
            <%--campos editables...--%>
           
            
        </Columns>
    </asp:GridView>
   
    <p style="text-align:center;">
        <asp:LinkButton ID="btnQuitarSeleccionados" runat="server" CssClass="btn btn-lg btn-danger disabled" OnClientClick="return confirm('¿Quitar linea/s de la lista?');" OnClick="btnQuitarSeleccionados_Click"><span class="glyphicon glyphicon-trash"></span>&nbsp; Quitar lineas seleccionadas</asp:LinkButton>
    </p>
        </asp:Panel>
</asp:Content>
