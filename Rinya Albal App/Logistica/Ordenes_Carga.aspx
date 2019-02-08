<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ordenes_Carga.aspx.cs" Inherits="Rinya_Albal_App.Logistica.Ordenes_Carga" %>


<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     
      <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
    <div class="row">
        <div class="col-md-8">
            <h2>CONTROL LOGISTICO SILLA</h2>
            <div class="form-group">
                <div class="row">
                    <div class="col-md-8">
                        <asp:Label runat="server" AssociatedControlID="OC_textBoxs" CssClass="col-md-8 control-label">Orden de Carga:</asp:Label>
                    </div>
                    
                </div>
                <div class="row">

                    <div class="col-md-4">

                        <asp:TextBox ID="OC_textBoxs" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-md-2">
                        <asp:Button ID="BtBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="BtBuscar_Click" /></div>

                </div>
                <div class="row">
                    <hr />
                    <div class="container">
                        <!-- <div class="container" style="overflow: scroll;HEIGHT:100%">-->

                        <cc1:SearchableGridView ID="GridView1" Visible="true"
                            AllowSorting="true"
                            AutoGenerateColumns="false" CancelSearchImageURL="Icons/filterCancel.png"
                            SearchGoFirstImageUrl="Icons/SearchGoFirst.png"
                            SearchGoImageUrl="Icons/SearchGo.png" SearchGoLastImageUrl="Icons/SearchGoLast.png"
                            ExcelImageURL="Icons/Excel.png"
                            SearchGoNextImageUrl="Icons/SearchGoNext.png" SearchGoPreviousImageUrl="Icons/SearchGoPrevious.png"
                            SearchImageURL="Icons/filter.png" ShowFooter="True" CurrentSortDirection="ASC"
                            EmptyDataText="No se han encontrado datos" runat="server"
                            OnFilterButtonClick="GridView1_FilterButtonClick"
                            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnCancelFilterButtonClick="GridView1_CancelFilterButtonClick" OnNavigationButtonClick="GridView1_NavigationButtonClick" OnSorting="GridView1_Sorting"
                            CellPadding="4" OnPageSizeChanged="GridView1_PageSizeChanged"
                            OnExcelButtonClick="GridView1_ExcelButtonClick" PageSize="3">

                            <Columns>
                                <cc1:SearchBoundField DataField="N_OF" HeaderText="N OF" SearchExpression="N_OF">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="F_ENTREGA" HeaderText="F ENTREGA" SearchExpression="F_ENTREGA">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="120px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>

                                <cc1:SearchBoundField DataField="ARTICULO" HeaderText="ARTICULO" SearchExpression="ARTICULO">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>

                                <cc1:SearchBoundField DataField="DESCRIPCION" HeaderText="DESCRIPCION" SearchExpression="DESCRIPCION">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>

                                <cc1:SearchBoundField DataField="SECCION" HeaderText="SECCION" SearchExpression="SECCION    ">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="UDS_VENTA" HeaderText="UDs VENTA" SearchExpression="UDS_VENTA">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="Kg_Pedido" HeaderText="Kg Pedido" SearchExpression="Kg_Pedido">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="Ud_Pedido" HeaderText="Ud Pedido" SearchExpression="Ud_Pedido">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="Ud_Servidas" HeaderText="Ud Servidas" SearchExpression="Ud_Servidas">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="KG_Actuales" HeaderText="KG Actuales" SearchExpression="KG_Actuales">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="Ud_Actuales" HeaderText="Ud Actuales" SearchExpression="Ud_Actuales">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="KG_Pendiente" HeaderText="KG Pendiente" SearchExpression="KG_Pendiente">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>
                                <cc1:SearchBoundField DataField="Ud_Pendiente" HeaderText="Ud_Pendiente" SearchExpression="Ud_Pendiente">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </cc1:SearchBoundField>


                            </Columns>

                            <FooterStyle BackColor="#424242" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#424242" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#424242" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />


                        </cc1:SearchableGridView>
                    </div>
                    <!-- </div> -->
                </div>
            </div>
        </div>
    </div>


</asp:Content>
