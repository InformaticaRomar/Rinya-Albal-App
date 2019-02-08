<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Control_stock.aspx.cs" Inherits="Rinya_Albal_App.Logistica.Control_stock" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/jquery-ui/jquery-ui.css" rel="stylesheet" type="text/css" />
    <div class="row">
        <div class="col-md-8">
            <h2>CONTROL STOCK</h2>
            <div class="form-group">
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label runat="server" AssociatedControlID="DropDownDesde" CssClass="col-md-2 control-label">Desde</asp:Label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label runat="server" AssociatedControlID="DropDownHasta" CssClass="col-md-2 control-label">Hasta</asp:Label>
                    </div>
                </div>
                <div class="row">

                    <div class="col-md-4">

                        <asp:DropDownList ID="DropDownDesde" CssClass="form-control" runat="server">
                            <asp:ListItem Selected="True">1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-4">
                        <asp:DropDownList ID="DropDownHasta" CssClass="form-control" runat="server">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem Selected="True">9</asp:ListItem>
                        </asp:DropDownList>
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
                                <cc1:SearchBoundField DataField="ARTICULO" HeaderText="Articulo" SearchExpression="ARTICULO">
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
                                <cc1:SearchBoundField DataField="Familia" HeaderText="Familia">
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
