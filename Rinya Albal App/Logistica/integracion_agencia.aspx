<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="integracion_agencia.aspx.cs" Inherits="Rinya_Albal_App.Logistica.integracion_agencia" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css"/>
     <div class="row">
           <div class="col-md-8">
               <h2> Integracion con Agencia </h2>
               <div class="form-group">
                   <div class="row">
                       <div class="col-md-4">
                   <asp:Label runat="server" AssociatedControlID="DropDownDesde" CssClass="col-md-2 control-label">Agencia</asp:Label>
                           </div>
                      
                       </div>
                   <div class="row">
                       
                      <div class="col-md-4">
                             
                          <asp:DropDownList ID="DropDownDesde"  CssClass="form-control" runat="server">
                              <asp:ListItem  Value="">Todas</asp:ListItem>
                              <asp:ListItem Value="101">SDF</asp:ListItem>
                              <asp:ListItem Value="102">HERMES</asp:ListItem>
                              <asp:ListItem Value="105">Logifrio</asp:ListItem>
                              <asp:ListItem Value="106">E.G. Barcelona</asp:ListItem>
                              <asp:ListItem Value="109">Integra2</asp:ListItem>
                              <asp:ListItem Value="112">E.G Valencia</asp:ListItem>
                              <asp:ListItem Value="113">E.G Alicante/Murcia</asp:ListItem>
                              <asp:ListItem Value="115">Recoge el cliente</asp:ListItem>
                              <asp:ListItem Value="118">E.G Andalucia</asp:ListItem>
                              <asp:ListItem Value="122">Portugal</asp:ListItem>
                              <asp:ListItem Value="123">Marsol</asp:ListItem>
                              <asp:ListItem Value="124">Logifrio</asp:ListItem>
                              <asp:ListItem Value="125">Prats</asp:ListItem>
                              <asp:ListItem Value="126">E.G Madrid</asp:ListItem>
                              <asp:ListItem Selected="True" Value="127">DLR</asp:ListItem>
                                
                                 </asp:DropDownList>
                          </div>
                  
                       <div class ="col-md-2"><asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="BtBuscar_Click" /></div>
                    
                     </div>
                     <div class="row">
                           <hr />
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
                         OnExcelButtonClick="GridView1_ExcelButtonClick" PageSize="5"   >
                         
                       <Columns>
                           <cc1:SearchBoundField DataField="CLIENTE" HeaderText="CLIENTE" SearchExpression="CLIENTE">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="N_PEDIDO" HeaderText="N.PEDIDO" SearchExpression="N_PEDIDO">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                           <cc1:SearchBoundField DataField="N_BUL" HeaderText="N.BUL" SearchExpression="N_BUL">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                            <cc1:SearchBoundField DataField="NOMBRE" HeaderText="NOMBRE" SearchExpression="NOMBRE">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>

                            <cc1:SearchBoundField DataField="DIRECCION" HeaderText="DIRECCION" SearchExpression="DIRECCION">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="C_POS" HeaderText="C.POS" SearchExpression="C_POS">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="POBLACION" HeaderText="POBLACION" SearchExpression="POBLACION">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="KILOS" HeaderText="KILOS" SearchExpression="KILOS">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="FECHA_ENTREGA" HeaderText="FECHA ENTREGA" SearchExpression="FECHA_ENTREGA">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="100px" HorizontalAlign="Left" /> 
                                </cc1:SearchBoundField>
                           <cc1:SearchBoundField DataField="OBSERVACIONES" HeaderText="OBSERVACIONES" SearchExpression="OBSERVACIONES">
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
                         </div>
                   </div>
               </div>
      </div>

</asp:Content>
