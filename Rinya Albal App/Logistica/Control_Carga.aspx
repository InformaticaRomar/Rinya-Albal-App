<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Control_Carga.aspx.cs" Inherits="Rinya_Albal_App.Logistica.Control_Carga" %>
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
           dateFormat: 'dd/mm/yy',
           firstDay: 1,
           isRTL: false,
           showMonthAfterYear: false,
           yearSuffix: ''
       };
  
     $.datepicker.setDefaults($.datepicker.regional['es']);
     $(document).ready(function () {
   var hoy = new Date();
   var semana_dia = new Date(); //hoy.getFullYear(), 0, 1);
   semana_dia.setDate(semana_dia.getDate() - 2);
                // semana_dia.setDate(semana_dia.getDate() - 2);
                 
                 $("#<%= datepicker_1.ClientID %>").datepicker().datepicker("setDate", semana_dia);

                 $("#<%= datepicker_2.ClientID %>").datepicker().datepicker("setDate", new Date());
  if (!($("[id$=datepicker1]").datepicker().attr("Value") == undefined)) {
            
      if ($("[id$=datepicker_1]").datepicker().attr("Value").length > 0) {
          $("[id$=datepicker_1]").datepicker().datepicker("setDate", new Date($("[id$=datepicker_1]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
          $("[id$=datepicker_2]").datepicker().datepicker("setDate", new Date($("[id$=datepicker_2]").datepicker().attr("Value").replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")));
          alert($("[id$=datepicker_1]").datepicker().attr("Value"));
          alert($("[id$=datepicker_2]").datepicker().attr("Value"));
             }
             
         }
     });
        </script>
         <div class="row">
           <div class="col-md-8">
               <h2> Control Carga</h2>
               <div class="form-group">
                   
                <div class="row">
                    <div class="col-md-2">
                        <asp:Label runat="server" CssClass="col-md-2 control-label" >Desde</asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Hasta</asp:Label>
                    </div>
                     
                </div>
                     </div>
                <div class="row">

                    <div class="col-md-2">
                             
                         <asp:TextBox ID="datepicker_1" CssClass="form-control"  runat="server" EnableViewState="true" style="width: 98px"></asp:TextBox>
                         <!-- </div>-->
                  
                    </div>

                    <div class="col-md-2">
                        <asp:TextBox ID="datepicker_2" CssClass="form-control"  runat="server" EnableViewState="true" style="width: 98px"></asp:TextBox>
                        </div>
                   
                    <div class="col-md-2">
                       <div class ="col-md-2">
                           <asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="BtBuscar_Click" /></div>
                    
                     </div>
                  <!--   <div class="row">
                           <hr />-->
                         <div class="container" >
                          <!-- <div class="container" style="overflow: scroll;HEIGHT:100%">-->
             
                   <cc1:SearchableGridView ID="GridView1" Visible="true"
                       
                        AllowSorting="true"
                        AutoGenerateColumns="false" CancelSearchImageURL="Icons/filterCancel.png" 
                        SearchGoFirstImageUrl="Icons/SearchGoFirst.png"
                        SearchGoImageUrl="Icons/SearchGo.png" SearchGoLastImageUrl="Icons/SearchGoLast.png"
                       ExcelImageURL="Icons/Excel.png"
                        SearchGoNextImageUrl="Icons/SearchGoNext.png" SearchGoPreviousImageUrl="Icons/SearchGoPrevious.png"
                        SearchImageURL="Icons/filter.png" ShowFooter="True" CurrentSortDirection="ASC"
                        
                        EmptyDataText="No se han encontrado datos"  runat="server"
                       OnFilterButtonClick="GridView1_FilterButtonClick"
                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnCancelFilterButtonClick="GridView1_CancelFilterButtonClick" OnNavigationButtonClick="GridView1_NavigationButtonClick" OnSorting="GridView1_Sorting" 
                     CellPadding="4" OnPageSizeChanged="GridView1_PageSizeChanged"
                         OnExcelButtonClick="GridView1_ExcelButtonClick" PageSize="3"   >
                         
                       <Columns>
                           <cc1:SearchBoundField DataField="N_albaran" HeaderText="N Albaran" SearchExpression="N_albaran">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="DESCRIPCION" HeaderText="Descripcion" SearchExpression="DESCRIPCION">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                           <cc1:SearchBoundField DataField="Kg_Actuales" HeaderText="KGs Actuales" SearchExpression="Kg_Actuales">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                            <cc1:SearchBoundField DataField="UDs_Actuales" HeaderText="UDs Actuales" SearchExpression="UDs_Actuales">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                            <cc1:SearchBoundField DataField="Dsc_Estado" HeaderText="Estado" SearchExpression="Dsc_Estado">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="Almacen" HeaderText="Almacen" SearchExpression="Almacen">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="LoteInterno" HeaderText="Lote Interno" SearchExpression="LoteInterno">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="FechaCaducidad" HeaderText="F. Cad" SearchExpression="FechaCaducidad">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="FechaFabricacion" HeaderText="F. Fab" SearchExpression="FechaFabricacion">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="SSCC" HeaderText="Matricula" SearchExpression="SSCC">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="Partida" HeaderText="Partida" SearchExpression="Partida">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="Discriminador" HeaderText="Discriminador" SearchExpression="Discriminador">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="Familia" HeaderText="Familia" >
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>


                           </Columns>
                        
                        <FooterStyle BackColor="#424242" Font-Bold="True" ForeColor="White"  />
                        <HeaderStyle BackColor="#424242" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#424242" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                       
                       
                           </cc1:SearchableGridView >
                  </div>
                                  <!-- </div> -->
                            <!-- </div> -->
                   </div>
               
      </div>
</div>
</asp:Content>
