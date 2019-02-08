using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

[assembly: TagPrefix("CustomControls", "CustomControls")]
namespace CustomControls
{
    

        public class SearchGridEventArgs : EventArgs
    {
        private DataTable _SearchFilterValues;
        public SearchGridEventArgs(DataTable searchFilterValues)
        {
            _SearchFilterValues = searchFilterValues;
        }

        public DataTable SearchFilterValues
        {
            get
            {
                return this._SearchFilterValues;
            }

        }
    }
    public class NavigationButtonEventArgs : EventArgs
    {
        private readonly NavigationButtonsTypes _navigationButtonsType;
        private int? _pageIndex;

        public NavigationButtonEventArgs(NavigationButtonsTypes navigationButtonsType)
        {
            _navigationButtonsType = navigationButtonsType;
        }

        public NavigationButtonEventArgs(NavigationButtonsTypes navigationButtonsType, int? pageIndex)
        {
            _navigationButtonsType = navigationButtonsType;
            _pageIndex = pageIndex;
        }

        public NavigationButtonsTypes NavigationButtonsType
        {
            get
            {
                return this._navigationButtonsType;
            }
        }

        public int? PageIndex
        {
            get
            {
                return this._pageIndex;
            }
        }
    }

    public class PageSizeChangeEventArgs : System.EventArgs
    {
        public int NewPageSize;
        public PageSizeChangeEventArgs(int pNewPageSize)
        {
            NewPageSize = pNewPageSize;
        }
    }

    //Type of navigation buttons
    public enum NavigationButtonsTypes
    {
        GoFirst = 1,
        GoPrevious = 2,
        GoNext = 3,
        GoLast = 4,
        GoToPage = 5
    };
    public enum ShowPager
    {
        None,
        OnTop,
        OnBotton,
        OnTopAndBottom
    }
    [
    ToolboxData("<{0}:SearchableGridView runat=server></{0}:SearchableGridView>")
    ]
    public class SearchableGridView : GridView
    {
        #region eventos
        //Event for Search grid
        public delegate void SearchGridEventHandler(object sender, SearchGridEventArgs e);
        public event SearchGridEventHandler FilterButtonClick;
        protected virtual void OnFilterButtonClick(SearchGridEventArgs e)
        {
            if (FilterButtonClick != null)
            {
                FilterButtonClick(this, e);
            }
        }

        //Event for Search Cancel
        public delegate void CancelSearchGridEventHandler(object sender, SearchGridEventArgs e);
        public event CancelSearchGridEventHandler CancelFilterButtonClick;
        protected virtual void OnCancelFilterButtonClick(SearchGridEventArgs e)
        {
            if (CancelFilterButtonClick != null)
            {
                CancelFilterButtonClick(this, e);
            }
        }

        public delegate void ExcelGridEventHandler(object sender, SearchGridEventArgs e);
        public event ExcelGridEventHandler ExcelButtonClick;
        protected virtual void OnExcelButtonClick(SearchGridEventArgs e)
        {
            if (ExcelButtonClick != null)
            {
                ExcelButtonClick(this, e);
            }
        }


        public delegate void PageSizeChangeHandler(object sender, PageSizeChangeEventArgs e);
        public event PageSizeChangeHandler PageSizeChanged;
        protected virtual void OnPageSizeChanged(PageSizeChangeEventArgs e)
        {
            if (PageSizeChanged != null)
            {
                PageSizeChanged(this, e);
            }
        }
        //Event for Navigation Buttons
        public delegate void NavigationButtonEventHandler(object sender, NavigationButtonEventArgs e);
        public event NavigationButtonEventHandler NavigationButtonClick;
        protected virtual void OnNavigationButtonClick(NavigationButtonEventArgs e)
        {
            if (NavigationButtonClick != null)
            {
                NavigationButtonClick(this, e);
            }
        }
        #endregion

        #region Controls and constants
        // Controls to implement the search feature

        TextBox _txtSearch;
        ImageButton _btnSearch;
        ImageButton _btnExcel;
        ImageButton _btnSearchCancel;
        Panel _pnlSearchFooter;

        //Navigation Controls
        ImageButton btnSearchGoFirst;
        ImageButton btnSearchGoPrevious;
        ImageButton btnSearchGoNext;
        ImageButton btnSearchGoLast;
        ImageButton btnSearchGoPage;
        TextBox txtSearchGo;
        Label lblRecStatus;

        DataTable dtSearchFilter;
        //PAGGING
        private ShowPager _ShowMrllCustomPaging;
        private DropDownList ddlPager = new DropDownList();
        private DropDownList ddlSort = new DropDownList();
        private DropDownList ddlNumberOfPages = new DropDownList();
        private LinkButton lnkPrev = new LinkButton();
        private LinkButton lnkNext = new LinkButton();
        private string _CustomPagerCssClass = string.Empty;
        private string _DefaultSortExpression = string.Empty;

        private DropDownList ddlFooterPager = new DropDownList();
        private LinkButton lnkFooterPrev = new LinkButton();
        private LinkButton lnkFooterNext = new LinkButton();

        private Label lblStatus = new Label();
        private Label lbkFooterStatus = new Label();

       

        public ShowPager ShowMrllCustomPaging
        {
            get { return _ShowMrllCustomPaging; }
            set { _ShowMrllCustomPaging = value; }
        }
        public string DefaultSortExpression
        {
            get { return _DefaultSortExpression; }
        }
        public string CustomPagerCssClass
        {
            get { return _CustomPagerCssClass; }
            set { _CustomPagerCssClass = value; }
        }

        private const string SHOW_TOTAL_ROWS = "ShowTotalRows";
        private const string NO_OF_ROWS = "NoOfRows";
        private const string SHOW_ROWNUM = "ShowRowNum";

        private const string SHOW_EMPTY_FOOTER = "ShowEmptyFooter";
        private const string SHOW_EMPTY_HEADER = "ShowEmptyHeader";
        private const string SEARCH_IMAGE_URL = "SearchImageUrl";
        private const string CANCEL_SEARCH_IMAGE_URL = "CancelSearchImageUrl";
        private const string EXCEL_IMAGE_URL = "ExcelImageUrl"; 
        private const string TOTAL_SEARCH_RECORDS = "TotalSearchRecords";
        private const string TOTAL_SEARCH_PAGES = "TotalSearchPages";
        private const string CURRENT_SEARCH_PAGE_NO = "CurrentSearchPageNo";
        private const string CURRENT_SORT_EXPRESSION = "CurrentSortExpression";
        private const string CURRENT_SORT_DIRECTION = "CurrentSortDirection";

        private const string SEARCH_GO_LAST_IMAGE_URL = "SearchGoLastImageUrl";
        private const string SEARCH_GO_PREVIOUS_IMAGE_URL = "SearchGoPreviousImageUrl";
        private const string SEARCH_GO_NEXT_IMAGE_URL = "SearchGoNextImageUrl";
        private const string SEARCH_GO_FIRST_IMAGE_URL = "SearchGoFirstImageUrl";
        private const string SEARCH_GO_IMAGE_URL = "SearchGoImageUrl";
        private const string SELECTABLE_DATA_ROW = "SelectableDataRow";
        private const string SEARCH_FILTERS = "SearchFilters";
        private const string SEARCH_FILTER_CHANGED = "SearchFilterChanged";


        //ArrayList _lstSearchFilter;
        ArrayList _lstHiddenColumns;

        #endregion

        #region Constructor
       
        public SearchableGridView ()
               : base()
        {
            //By default turn on the footer shown property
            ShowFooter = true;

            //Datatable for Search Filter
            dtSearchFilter = new DataTable("dtSearchFilter");
            dtSearchFilter.Columns.Add("SearchField", typeof(string));
            dtSearchFilter.Columns.Add("SearchValue", typeof(string));
            ViewState[SEARCH_FILTERS] = dtSearchFilter;
        }
        #endregion

        #region Appearance
        [Category("Appearance")]
        [DefaultValue(true)]
        [Bindable(BindableSupport.No)]
        public bool ShowEmptyFooter
        {
            get
            {
                if (this.ViewState[SHOW_EMPTY_FOOTER] == null)
                {
                    this.ViewState[SHOW_EMPTY_FOOTER] = true;
                }

                return (bool)this.ViewState[SHOW_EMPTY_FOOTER];
            }
            set
            {
                this.ViewState[SHOW_EMPTY_FOOTER] = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(0)]
        [Bindable(BindableSupport.No)]
        public int TotalSearchRecords
        {
            get
            {
                if (this.ViewState[TOTAL_SEARCH_RECORDS] == null)
                {
                    this.ViewState[TOTAL_SEARCH_RECORDS] = 0;
                }

                return (int)this.ViewState[TOTAL_SEARCH_RECORDS];
            }
            set
            {
                if (value > -1)
                {
                    this.ViewState[TOTAL_SEARCH_RECORDS] = value;
                    if (PageSize > 0)
                    {
                        int intTotalPages = (TotalSearchRecords / PageSize);
                        if (TotalSearchRecords % PageSize > 0) { intTotalPages++; }
                        TotalSearchPages = intTotalPages;
                    }
                }
                else
                {
                    throw new Exception("Invalid Total Search Records.");
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(0)]
        [Bindable(BindableSupport.No)]
        public int TotalSearchPages
        {
            get
            {
                if (this.ViewState[TOTAL_SEARCH_PAGES] == null)
                {
                    this.ViewState[TOTAL_SEARCH_PAGES] = 0;
                }

                return (int)this.ViewState[TOTAL_SEARCH_PAGES];
            }
            set
            {
                if (value > -1)
                {
                    this.ViewState[TOTAL_SEARCH_PAGES] = value;
                }
                else
                {
                    throw new Exception("Invalid Total Search Records.");
                }
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        [Bindable(BindableSupport.No)]
        public int? CurrentSearchPageNo
        {
            get
            {
                if (this.ViewState[CURRENT_SEARCH_PAGE_NO] == null)
                {
                    this.ViewState[CURRENT_SEARCH_PAGE_NO] = null;
                }

                return (int?)this.ViewState[CURRENT_SEARCH_PAGE_NO];
            }
            set
            {
                if (value > 0)
                {
                    this.ViewState[CURRENT_SEARCH_PAGE_NO] = value;
                }
                else
                {
                    throw new Exception("Invalid Page Number.");
                }
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public bool ShowEmptyHeader
        {
            get
            {
                if (this.ViewState[SHOW_EMPTY_HEADER] == null)
                {
                    this.ViewState[SHOW_EMPTY_HEADER] = true;
                }

                return (bool)this.ViewState[SHOW_EMPTY_HEADER];
            }
            set
            {
                this.ViewState[SHOW_EMPTY_HEADER] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public bool ShowTotalRows
        {
            get
            {
                if (this.ViewState[SHOW_TOTAL_ROWS] == null)
                {
                    this.ViewState[SHOW_TOTAL_ROWS] = true;
                }

                return (bool)this.ViewState[SHOW_TOTAL_ROWS];
            }
            set
            {
                this.ViewState[SHOW_TOTAL_ROWS] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string SearchImageURL
        {
            get
            {
                if (this.ViewState[SEARCH_IMAGE_URL] == null)
                {
                    this.ViewState[SEARCH_IMAGE_URL] = "";
                }

                return (string)this.ViewState[SEARCH_IMAGE_URL];
            }
            set
            {
                this.ViewState[SEARCH_IMAGE_URL] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string SearchGoFirstImageUrl
        {
            get
            {
                if (this.ViewState[SEARCH_GO_FIRST_IMAGE_URL] == null)
                {
                    this.ViewState[SEARCH_GO_FIRST_IMAGE_URL] = "";
                }

                return (string)this.ViewState[SEARCH_GO_FIRST_IMAGE_URL];
            }
            set
            {
                this.ViewState[SEARCH_GO_FIRST_IMAGE_URL] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string SearchGoNextImageUrl
        {
            get
            {
                if (this.ViewState[SEARCH_GO_NEXT_IMAGE_URL] == null)
                {
                    this.ViewState[SEARCH_GO_NEXT_IMAGE_URL] = "";
                }

                return (string)this.ViewState[SEARCH_GO_NEXT_IMAGE_URL];
            }
            set
            {
                this.ViewState[SEARCH_GO_NEXT_IMAGE_URL] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string SearchGoPreviousImageUrl
        {
            get
            {
                if (this.ViewState[SEARCH_GO_PREVIOUS_IMAGE_URL] == null)
                {
                    this.ViewState[SEARCH_GO_PREVIOUS_IMAGE_URL] = "";
                }

                return (string)this.ViewState[SEARCH_GO_PREVIOUS_IMAGE_URL];
            }
            set
            {
                this.ViewState[SEARCH_GO_PREVIOUS_IMAGE_URL] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string SearchGoLastImageUrl
        {
            get
            {
                if (this.ViewState[SEARCH_GO_LAST_IMAGE_URL] == null)
                {
                    this.ViewState[SEARCH_GO_LAST_IMAGE_URL] = "";
                }

                return (string)this.ViewState[SEARCH_GO_LAST_IMAGE_URL];
            }
            set
            {
                this.ViewState[SEARCH_GO_LAST_IMAGE_URL] = value;
            }
        }


        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string SearchGoImageUrl
        {
            get
            {
                if (this.ViewState[SEARCH_GO_IMAGE_URL] == null)
                {
                    this.ViewState[SEARCH_GO_IMAGE_URL] = "";
                }

                return (string)this.ViewState[SEARCH_GO_IMAGE_URL];
            }
            set
            {
                this.ViewState[SEARCH_GO_IMAGE_URL] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string SearchGoText
        {
            get
            {

                return (string)this.ViewState[SEARCH_GO_IMAGE_URL];
            }
        }


        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string CancelSearchImageURL
        {
            get
            {
                if (this.ViewState[CANCEL_SEARCH_IMAGE_URL] == null)
                {
                    this.ViewState[CANCEL_SEARCH_IMAGE_URL] = "";
                }

                return (string)this.ViewState[CANCEL_SEARCH_IMAGE_URL];
            }
            set
            {
                this.ViewState[CANCEL_SEARCH_IMAGE_URL] = value;
            }
        }


        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string ExcelImageURL
        {
            get
            {
                if (this.ViewState[EXCEL_IMAGE_URL] == null)
                {
                    this.ViewState[EXCEL_IMAGE_URL] = "";
                }

                return (string)this.ViewState[EXCEL_IMAGE_URL];
            }
            set
            {
                this.ViewState[EXCEL_IMAGE_URL] = value;
            }
        }
        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(true)]
        public bool SelectableDataRow
        {
            get
            {
                if (this.ViewState[SELECTABLE_DATA_ROW] == null)
                {
                    this.ViewState[SELECTABLE_DATA_ROW] = true;
                }

                return (bool)this.ViewState[SELECTABLE_DATA_ROW];
            }
            set
            {
                this.ViewState[SELECTABLE_DATA_ROW] = value;
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        public DataTable SearchFilters
        {
            get
            {
                return (DataTable)this.ViewState[SEARCH_FILTERS];
            }
        }

        [Category("Appearance")]
        [Bindable(BindableSupport.No)]
        [DefaultValue(false)]
        public bool SearchFilterChanged
        {
            get
            {
                if (this.ViewState[SEARCH_FILTER_CHANGED] == null)
                {
                    this.ViewState[SEARCH_FILTER_CHANGED] = false;
                }

                return (bool)this.ViewState[SEARCH_FILTER_CHANGED];
            }
            set
            {
                this.ViewState[SEARCH_FILTER_CHANGED] = value;
            }
        }

        [Category("Behaviour")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string CurrentSortExpression
        {
            get
            {
                if (this.ViewState[CURRENT_SORT_EXPRESSION] == null)
                {
                    this.ViewState[CURRENT_SORT_EXPRESSION] = "";
                }

                return (string)this.ViewState[CURRENT_SORT_EXPRESSION];
            }
            set
            {
                this.ViewState[CURRENT_SORT_EXPRESSION] = value;
            }
        }
        #endregion

        [Category("Behaviour")]
        [Bindable(BindableSupport.No)]
        [DefaultValue("")]
        public string CurrentSortDirection
        {
            get
            {
                if (this.ViewState[CURRENT_SORT_DIRECTION] == null)
                {
                    this.ViewState[CURRENT_SORT_DIRECTION] = "ASC";
                }

                return (string)this.ViewState[CURRENT_SORT_DIRECTION];
            }
            set
            {
                if (value != "ASC" && value != "DESC")
                {
                    throw new Exception("Invalid sort direction, it should be ASC or DESC");
                }
                this.ViewState[CURRENT_SORT_DIRECTION] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Compare old filter data and new textbox data
            CompareCurrentSearchFilterWithOldData();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (IsDesign()) { return; }

            _pnlSearchFooter = new Panel();
            TemplateField tempFieldSearch = new TemplateField();
            tempFieldSearch.ItemStyle.Width = 55;
            // Insert this as the last column for holding the search and search cancel buttons
            this.Columns.Insert(this.Columns.Count, tempFieldSearch);
            //Set the width of the grid by calculating the each columns width
            int grdWidth = 0;
            for (int indexCol = 0; indexCol < Columns.Count; indexCol++)
            {
                if (this.Columns[indexCol].Visible == true)
                {
                    grdWidth = grdWidth + (int)Columns[indexCol].ItemStyle.Width.Value;
                }
            }
            this.Width = Unit.Pixel(grdWidth + 60);

            //Store hidden column list in to local variable _lstHiddenColumnList
            _lstHiddenColumns = new ArrayList();
            for (int indexCol = 0; indexCol < this.Columns.Count - 1; indexCol++)
            {
                if (this.Columns[indexCol].Visible == false)
                {
                    _lstHiddenColumns.Add(indexCol);
                    this.Columns[indexCol].Visible = true;
                }
            }
            InitSearchFilterData();

        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);
            if (SelectableDataRow == true)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ceedfc'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=''");
                    e.Row.Attributes.Add("style", "cursor:pointer;");
                    e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackClientHyperlink(this, "Select$" + e.Row.RowIndex));
                }
            }
            //Initilize search filter data
            InitSearchFilterData();
        }

        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);
            if (IsDesign()) { return; } //During Runtime

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //If ShowFooter is set to true
                if (ShowFooter && e.Row.Cells.Count > 0)
                {
                    if (e.Row.Cells[0].Controls.Count == 0)
                    {
                        CreateFooterNaviControls();
                        e.Row.Cells[0].Controls.Add(_pnlSearchFooter);
                    }
                }
                //e.Row.Cells[0].Attributes.CssStyle.Add("display", "none");
            }

            //Hide hidden column cells
            if (e.Row.RowType != DataControlRowType.Footer)
            {
                for (int intCol = 0; intCol < _lstHiddenColumns.Count; intCol++)
                {
                    e.Row.Cells[(int)_lstHiddenColumns[intCol]].Attributes.CssStyle.Add("display", "none");
                }
            }
        }

        void InitSearchFilterData()
        {
            //Set Search Filter Data if the search filter is empty
            if (ViewState[SEARCH_FILTERS] != null)
            {
                if (((DataTable)ViewState[SEARCH_FILTERS]).Rows.Count > 0)
                {
                    return;
                }
            }
            //Clear datatable and store values
            dtSearchFilter.Rows.Clear();
            for (int indexCol = 0; indexCol < this.Columns.Count - 1; indexCol++)
            {
                DataControlField dcfFieldSearch = Columns[indexCol];
                SearchBoundField boundFieldSearch = dcfFieldSearch as SearchBoundField;
                if (boundFieldSearch != null)
                {
                    dtSearchFilter.Rows.Add("(" + boundFieldSearch.SearchExpression + ")", "");
                }
            }
            ViewState[SEARCH_FILTERS] = dtSearchFilter;
        }

        void CompareCurrentSearchFilterWithOldData()
        {
            //Compare the Search filter value with old filter data to check the filter is changed or not
            string strTextBox = "";
            SearchFilterChanged = false;
            if (this.Controls.Count < 1)
            {
                return;
            }
            if (ViewState[SEARCH_FILTERS] != null)
            {
                dtSearchFilter = ((DataTable)ViewState[SEARCH_FILTERS]);
                if (dtSearchFilter.Rows.Count > 0)
                {
                    int i = 0;
                    for (int indexSearch = 0; indexSearch < this.Columns.Count - 1; indexSearch++)
                    {
                        if (this.Columns[indexSearch].GetType() == typeof(SearchBoundField))
                        {
                            if (this.Controls[0].Controls[0].Controls[indexSearch].HasControls())
                            {
                                if (this.Controls[0].Controls[0].Controls[indexSearch].Controls[0].HasControls())
                                {
                                    if (this.Controls[0].Controls[0].Controls[indexSearch].Controls[0].Controls[0].HasControls())
                                    {
                                        strTextBox = ((TextBox)this.Controls[0].Controls[0].Controls[indexSearch].Controls[0].Controls[0]).Text;
                                        if (!strTextBox.Equals(dtSearchFilter.Rows[i]["SearchValue"]))
                                        {
                                            SearchFilterChanged = true;
                                        }
                                    }
                                }
                            }

                            i++;
                        }
                    }
                }
            }
        }

        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            int count = base.CreateChildControls(dataSource, dataBinding);
            if (IsDesign()) { return count; }

            //  no rows in grid. create header and footer in this case
            if (count == 0 && (ShowEmptyFooter || ShowEmptyHeader))
            {
                //  create the table
                Table table = this.CreateChildTable();

                DataControlField[] fields;
                if (this.AutoGenerateColumns)
                {
                    PagedDataSource source = new PagedDataSource();
                    source.DataSource = dataSource;

                    System.Collections.ICollection autoGeneratedColumns =
                                                   this.CreateColumns(source, true);
                    fields = new DataControlField[autoGeneratedColumns.Count];
                    autoGeneratedColumns.CopyTo(fields, 0);
                }
                else
                {
                    fields = new DataControlField[this.Columns.Count];
                    this.Columns.CopyTo(fields, 0);
                }

                if (ShowEmptyHeader)
                {
                    //create a new header row
                    GridViewRow headerRow = base.CreateRow(-1, -1, DataControlRowType.Header,
                                                           DataControlRowState.Normal);
                    this.InitializeRow(headerRow, fields);
                    // Fire the OnRowCreated event to handle showing row numbers
                    OnRowCreated(new GridViewRowEventArgs(headerRow));
                    //add the header row to the table
                    table.Rows.Add(headerRow);
                }

                //create the empty row
                GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow,
                                                       DataControlRowState.Normal);
                TableCell cell = new TableCell();
                cell.ColumnSpan = fields.Length;
                cell.Width = Unit.Percentage(100);

                //  respect the precedence order if both EmptyDataTemplate
                //  and EmptyDataText are both supplied ...
                if (this.EmptyDataTemplate != null)
                {
                    this.EmptyDataTemplate.InstantiateIn(cell);
                }
                else if (!string.IsNullOrEmpty(this.EmptyDataText))
                {
                    cell.Controls.Add(new LiteralControl(EmptyDataText));
                }

                emptyRow.Cells.Add(cell);
                table.Rows.Add(emptyRow);

                if (ShowEmptyFooter)
                {
                    //  create footer row
                    GridViewRow footerRow = base.CreateRow(-1, -1, DataControlRowType.Pager,
                                                           DataControlRowState.Normal);
                    this.InitializeRow(footerRow, fields);
                    // Fire the OnRowCreated event to handle showing
                    // search tool and total number of rows
                    OnRowCreated(new GridViewRowEventArgs(footerRow));

                    //  add the footer to the table
                    table.Rows.Add(footerRow);

                }

                this.Controls.Clear();
                this.Controls.Add(table);
                CreateFilterColumn();
            }
            else
            {
                CreateFilterColumn();
            }

            //Merge Footer columns 
            if (this.FooterRow != null)
            {
                int intCellCount = this.FooterRow.Cells.Count;
                this.FooterRow.Cells[0].ColumnSpan = this.FooterRow.Cells.Count;
                for (int indexCol = intCellCount - 1; indexCol > 0; indexCol--)
                {
                    //this.FooterRow.Cells.RemoveAt(indexCol);
                    this.FooterRow.Cells[indexCol].Visible = false;
                }
            }

            //Merge last two columns of Hedder Row
            if (this.HeaderRow != null)
            {
                int intCellCount = this.HeaderRow.Cells.Count;
                ((TableCell)this.Controls[0].Controls[1].Controls[intCellCount - 1]).Visible = false;
                if (intCellCount > 2) { 
               
                ((TableCell)this.Controls[0].Controls[1].Controls[intCellCount - 2]).ColumnSpan = 2;
                }
            }

            //Merge last two columns of data row , because the last column is used for Search/SearchCancel Buttons
            for (int indexRow = 0; indexRow < this.Rows.Count; indexRow++)
            {
                GridViewRow gvRow = this.Rows[indexRow];
                if (this.Columns.Count>2) { 
                gvRow.Cells[this.Columns.Count - 2].ColumnSpan = 2;
                  
                }
                gvRow.Cells.RemoveAt(this.Columns.Count - 1);
            }
            return count;
        }

        protected override void OnSorting(GridViewSortEventArgs e)
        {
            CurrentSortExpression = e.SortExpression;
            if (e.SortExpression == CurrentSortExpression)
            {
                if (CurrentSortDirection == "ASC")
                {
                    CurrentSortDirection = "DESC";
                }
                else
                {
                    CurrentSortDirection = "ASC";
                }
            }
            base.OnSorting(e);
        }

        void _btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            ApplySearchGrid();
            //((TextBox)this.Controls[0].Controls[0].Controls[0].Controls[0].Controls[0]).Focus();
        }

        void _btnCancelSearch_Click(object sender, ImageClickEventArgs e)
        {
            ClearSearchFilters();
        }

        void _btnExcel_Click(object sender, ImageClickEventArgs e)
        {
           // ExportToExcel();
            SearchGridEventArgs eventData = new SearchGridEventArgs(dtSearchFilter);
            OnExcelButtonClick(eventData);
            // ClearSearchFilters();
        }

        void _btnNavigationButtonGoFirst_Click(object sender, ImageClickEventArgs e)
        {
            NavigationButtonEventArgs eventData = new NavigationButtonEventArgs(NavigationButtonsTypes.GoFirst);
            OnNavigationButtonClick(eventData);
        }

        void _btnNavigationButtonGoPrevious_Click(object sender, ImageClickEventArgs e)
        {
            NavigationButtonEventArgs eventData = new NavigationButtonEventArgs(NavigationButtonsTypes.GoPrevious);
            OnNavigationButtonClick(eventData);
        }

        void _btnNavigationButtonGoLast_Click(object sender, ImageClickEventArgs e)
        {
            NavigationButtonEventArgs eventData = new NavigationButtonEventArgs(NavigationButtonsTypes.GoLast);
            OnNavigationButtonClick(eventData);
        }

        void _btnNavigationButtonGoNext_Click(object sender, ImageClickEventArgs e)
        {
            NavigationButtonEventArgs eventData = new NavigationButtonEventArgs(NavigationButtonsTypes.GoNext);
            OnNavigationButtonClick(eventData);
        }

        void _btnNavigationButtonGoToPage_Click(object sender, ImageClickEventArgs e)
        {
            string txtValue = ((TextBox)(this.FooterRow.Controls[0].Controls[0].Controls[0].Controls[0].Controls[0].Controls[0].Controls[0].Controls[2].Controls[0])).Text;
            Int32 TmpValue;
            int intSearchGoValue = 0;
            if (Int32.TryParse(txtValue, out TmpValue))
            {
                intSearchGoValue = TmpValue;
            }
            NavigationButtonEventArgs eventData = new NavigationButtonEventArgs(NavigationButtonsTypes.GoToPage, intSearchGoValue);
            OnNavigationButtonClick(eventData);
        }

        //Search function and fire SearchGrid Event to aspx page
        public void ApplySearchGrid()
        {
            if (IsDesign()) { return; }

            //Clear Search Filter Datatable
            dtSearchFilter.Rows.Clear();

            for (int indexCol = 0; indexCol < this.Columns.Count - 1; indexCol++)
            {
                
                string strSearchTextBox = ((TextBox)_pnlSearchFooter.Controls[0].Controls[indexCol].Controls[0].Controls[0]).Text;
                DataControlField dcfFieldSearch = Columns[indexCol];
                SearchBoundField boundFieldSearch = dcfFieldSearch as SearchBoundField;
                if (boundFieldSearch != null)
                {
                    dtSearchFilter.Rows.Add("(" + boundFieldSearch.SearchExpression + ")", strSearchTextBox.Replace("'", "''"));
                }
            }
            this.ViewState[SEARCH_FILTERS] = dtSearchFilter;
            SearchGridEventArgs eventData = new SearchGridEventArgs(dtSearchFilter);
            OnFilterButtonClick(eventData);
        }

        private void CreateFooterNaviControls()
        {
            short tcWidth = 25;
            short buttonWidth = 25;
            short buttonHeight = 25;
            short txtWidth = 40;


            //Create the search control


            Table tableNavButtons = new Table();
            //tableNavButtons.BorderWidth = 1;

            tableNavButtons.Width = Unit.Percentage(100);
            TableCell tc;
            TableRow tr;
            tr = new TableRow();

            //Go to First page Image Button
            tc = new TableCell();
            tc.Style.Add("width", tcWidth.ToString() + "px");
            if (btnSearchGoFirst == null) { btnSearchGoFirst = new ImageButton(); }
            btnSearchGoFirst.ID = "btnSearchGoFirst";
            btnSearchGoFirst.ImageUrl = SearchGoFirstImageUrl;
            btnSearchGoFirst.ImageAlign = ImageAlign.AbsMiddle;
            btnSearchGoFirst.AlternateText = "First";
            btnSearchGoFirst.ToolTip = "Go to first page";
            btnSearchGoFirst.Width = buttonWidth;
            btnSearchGoFirst.Height = buttonHeight;
            btnSearchGoFirst.Click += new ImageClickEventHandler(_btnNavigationButtonGoFirst_Click); ;
            tc.Controls.Add(btnSearchGoFirst);
            tr.Cells.Add(tc);

            //Go to previous page image button
            tc = new TableCell();
            tc.Style.Add("width", tcWidth.ToString() + "px");
            if (btnSearchGoPrevious == null) { btnSearchGoPrevious = new ImageButton(); }
            btnSearchGoPrevious.ID = "btnSearchGoPrevious";
            btnSearchGoPrevious.ImageUrl = SearchGoPreviousImageUrl;
            btnSearchGoPrevious.Width = buttonWidth;
            btnSearchGoPrevious.Height = buttonHeight;
            btnSearchGoPrevious.ImageAlign = ImageAlign.AbsMiddle;
            btnSearchGoPrevious.AlternateText = "Previous";
            btnSearchGoPrevious.ToolTip = "Go to previous page";
            btnSearchGoPrevious.Click += new ImageClickEventHandler(_btnNavigationButtonGoPrevious_Click); ;
            tc.Controls.Add(btnSearchGoPrevious);
            tr.Cells.Add(tc);

            //Go to Page Textbox
            tc = new TableCell();
            tc.Style.Add("width", txtWidth.ToString() + "px");
            if (txtSearchGo == null) { txtSearchGo = new TextBox(); }
            txtSearchGo.ID = "txtSearchGo";
            txtSearchGo.Width = txtWidth;
            txtSearchGo.MaxLength = 5;

            tc.Controls.Add(txtSearchGo);
            tr.Cells.Add(tc);

            //Go to Next Page Image Button
            tc = new TableCell();
            tc.Style.Add("width", tcWidth.ToString() + "px");
            if (btnSearchGoPage == null) { btnSearchGoPage = new ImageButton(); }
            btnSearchGoPage.ID = "btnSearchGoPage";
            btnSearchGoPage.ImageUrl = SearchGoImageUrl;

            btnSearchGoPage.ImageAlign = ImageAlign.AbsMiddle;
            btnSearchGoPage.AlternateText = "Go";
            btnSearchGoPage.ToolTip = "Go to page";
            btnSearchGoPage.Width = buttonWidth;
            btnSearchGoPage.Height = buttonHeight;
            btnSearchGoPage.Click += new ImageClickEventHandler(_btnNavigationButtonGoToPage_Click); ;
            tc.Controls.Add(btnSearchGoPage);
            tr.Cells.Add(tc);

            //Go to Next page Image Button
            tc = new TableCell();
            tc.Style.Add("width", tcWidth.ToString() + "px");
            if (btnSearchGoNext == null) { btnSearchGoNext = new ImageButton(); }
            btnSearchGoNext.ID = "btnSearchGoNext";
            btnSearchGoNext.ImageUrl = SearchGoNextImageUrl;
            btnSearchGoNext.Width = buttonWidth;
            btnSearchGoNext.Height = buttonHeight;
            btnSearchGoNext.ImageAlign = ImageAlign.AbsMiddle;
            btnSearchGoNext.AlternateText = "Next";
            btnSearchGoNext.ToolTip = "Go to next page";
            btnSearchGoNext.Click += new ImageClickEventHandler(_btnNavigationButtonGoNext_Click); ;
            tc.Controls.Add(btnSearchGoNext);
            tr.Cells.Add(tc);

            //Go to last page image button
            tc = new TableCell();
            tc.Style.Add("width", tcWidth.ToString() + "px");
            if (btnSearchGoLast == null) { btnSearchGoLast = new ImageButton(); }
            btnSearchGoLast.ID = "btnSearchGoLast";
            btnSearchGoLast.ImageUrl = SearchGoLastImageUrl;
            btnSearchGoLast.Width = buttonWidth;
            btnSearchGoLast.Height = buttonHeight;
            btnSearchGoLast.ImageAlign = ImageAlign.AbsMiddle;
            btnSearchGoLast.AlternateText = "Last";
            btnSearchGoLast.ToolTip = "Go to last page";
            btnSearchGoLast.Click += new ImageClickEventHandler(_btnNavigationButtonGoLast_Click); ;
            tc.Controls.Add(btnSearchGoLast);
            tr.Cells.Add(tc);

            //Add an extra cell
            tc = new TableCell();
            tr.Cells.Add(tc);

            tableNavButtons.Rows.Add(tr);
            ddlNumberOfPages.AutoPostBack = true;
           if (ddlNumberOfPages.Items.Count < 2) { 
                ddlNumberOfPages.Items.Add("3");
            for (int i = 5; i <= 20; i += 5)
                ddlNumberOfPages.Items.Add(i.ToString());
            }
            //PageSize = Convert.ToInt32(ddlNumberOfPages.SelectedItem.Value);
            ddlNumberOfPages.SelectedItem.Value = PageSize.ToString();
             ddlNumberOfPages.SelectedIndexChanged += new EventHandler(ddlNumberOfPages_SelectedIndexChanged);

            Table tableLabel = new Table();
            tableLabel.Width = Unit.Percentage(100);
            TableRow trLabel = new TableRow();
            trLabel.Width = Unit.Percentage(100);
            TableCell tcLabel = new TableCell();
            tcLabel.Width = Unit.Percentage(100);
            //Record Status Label
            if (lblRecStatus == null) { lblRecStatus = new Label(); }
            lblRecStatus.ID = "lblRecStatus";
            if (PageSize > 0)
            {
                int intTotalPages = (TotalSearchRecords / PageSize);
                if (TotalSearchRecords % PageSize > 0) { intTotalPages++; }
                if (CurrentSearchPageNo > 0 && intTotalPages > 0)
                {
                    lblRecStatus.Text = "Pagina : " + CurrentSearchPageNo.ToString() + "/" + intTotalPages.ToString() + " - Filas Totales : " + TotalSearchRecords.ToString();
                }
                else
                {
                    lblRecStatus.Text = "";
                }
            }
            tcLabel.Style.Add("text-align", "right");
            lblRecStatus.Font.Bold = true;
            tcLabel.Controls.Add(lblRecStatus);
            trLabel.Cells.Add(tcLabel);


            tableLabel.Rows.Add(trLabel);

            Table tableMain = new Table();

            tableMain.Width = Unit.Percentage(100);

            TableRow trMain = new TableRow();
            trMain.Width = Unit.Percentage(100);
            TableCell tcMain = new TableCell();
            tcMain.Width = Unit.Percentage(30);

            tcMain.Controls.Add(tableNavButtons);
            trMain.Controls.Add(tcMain);

            tcMain = new TableCell();
            tcMain.Width = Unit.Percentage(10);
            TableRow trPagging = new TableRow();
            TableCell tcPagging = new TableCell();
            Label lbPages = new Label();
            lbPages.Text = "Filas por pagina:";
            tcPagging.Controls.Add(lbPages);
            tcPagging.Controls.Add(ddlNumberOfPages);

            tcMain.Controls.Add(tcPagging);
            trMain.Controls.Add(tcMain); 

            tcMain = new TableCell();
            tcMain.Width = Unit.Percentage(33);
            tcMain.Controls.Add(tableLabel);
            trMain.Controls.Add(tcMain);

            tableMain.Controls.Add(trMain);

            _pnlSearchFooter.Controls.Clear();
            _pnlSearchFooter.DefaultButton = "btnSearchGoPage";
            _pnlSearchFooter.Controls.Add(tableMain);
        }

        private void CreateFilterColumn()
        {
            if (IsDesign()) { return; }

            GridViewRow SearchGridRow = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
            TableCell SearchCell;
            Panel pnltxt;

            if (ViewState[SEARCH_FILTERS] != null)
            {
                dtSearchFilter = ((DataTable)ViewState[SEARCH_FILTERS]);
            }
            int i = 0;
            for (int indexCol = 0; indexCol < this.Columns.Count; indexCol++)
            {
                _txtSearch = new TextBox();
                SearchCell = new TableCell();
                if (!(indexCol == this.Columns.Count - 1))
                {


                    //Crate search text boxes
                    _txtSearch.ID = "txtSearch" + indexCol.ToString();
                    if (indexCol == this.Columns.Count - 1)
                    {
                        _txtSearch.Width = Unit.Percentage(70);
                    }
                    else
                    {
                        _txtSearch.Width = Unit.Percentage(99);
                        this.Columns[indexCol].ControlStyle.Width = Unit.Pixel(100);
                    }


                    //Retain old search values
                    if (this.Columns[indexCol].GetType() != typeof(SearchBoundField))
                    {
                        _txtSearch.Attributes.CssStyle.Add("display", "none");
                    }
                    else
                    {
                        if (dtSearchFilter.Rows.Count > 0)
                        {
                            _txtSearch.Text = dtSearchFilter.Rows[i]["SearchValue"].ToString();
                        }
                        i++;
                    }
                    pnltxt = new Panel();
                    pnltxt.DefaultButton = "btnSearch";
                    pnltxt.Controls.Add(_txtSearch);
                    SearchCell.Controls.Add(pnltxt);
                }
                //Add Search Button
                else if (indexCol == this.Columns.Count - 1)
                {
                    int SearchWidth = 25;
                    int SearchHeight = 20;

                    _btnSearch = new ImageButton();
                    _btnSearch.ID = "btnSearch";
                    _btnSearch.Width = SearchWidth;
                    _btnSearch.Height = SearchHeight;
                    _btnSearch.ImageUrl = SearchImageURL;
                    _btnSearch.ToolTip = "Filter data";
                    //Assign the function that is called when search button is clicked
                    _btnSearch.Click += new ImageClickEventHandler(_btnSearch_Click);

                    _btnSearchCancel = new ImageButton();
                    _btnSearchCancel.ID = "btnSearchCancel";
                    _btnSearchCancel.Width = SearchWidth;
                    _btnSearchCancel.Height = SearchHeight;
                    _btnSearchCancel.ImageUrl = CancelSearchImageURL;
                    _btnSearchCancel.ToolTip = "Cancel filtered data";
                    //Assign the function that is called when search button is clicked
                    _btnSearchCancel.Click += new ImageClickEventHandler(_btnCancelSearch_Click);

                    _btnExcel = new ImageButton();
                    _btnExcel.ID = "btnExcel";
                    _btnExcel.Width = SearchWidth;
                    _btnExcel.Height = SearchHeight;
                    _btnExcel.ImageUrl = ExcelImageURL;
                    _btnExcel.ToolTip = "Export Excel";
                    //Assign the function that is called when search button is clicked
                    _btnExcel.Click += new ImageClickEventHandler(_btnExcel_Click);

                    Table tblFilter = new Table();
                    tblFilter.Width = 50;
                    //tblFilter.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
                    //tblFilter.BorderWidth = 1;

                    TableRow tr = new TableRow();
                    TableCell tc = new TableCell();
                    tc.Width = 25;
                    tc.Controls.Add(_btnSearch);
                    tr.Controls.Add(tc);
                    tc = new TableCell();
                    tc.Width = 25;
                    tc.Controls.Add(_btnSearchCancel);
                    tr.Controls.Add(tc);
                    tc = new TableCell();
                    tc.Width = 25;
                    tc.Controls.Add(_btnExcel);
                    tr.Controls.Add(tc);
                    tblFilter.Controls.Add(tr);
                    SearchCell.Controls.Add(tblFilter);
                }
                if (SearchInHiddenColIndex(indexCol))
                {
                    SearchCell.Attributes.CssStyle.Add("display", "none");
                }
                SearchGridRow.Cells.Add(SearchCell);
            }
            _pnlSearchFooter.Controls.AddAt(0,SearchGridRow);
         //   this.Controls[0].Controls.AddAt(0, SearchGridRow);
        }

        private bool SearchInHiddenColIndex(int indexCol)
        {
            for (int indexLst = 0; indexLst < _lstHiddenColumns.Count; indexLst++)
            {
                if ((int)_lstHiddenColumns[indexLst] == indexCol)
                {
                    return true;
                }
            }
            return false;
        }
       
        public void ClearSearchFilters()
        {

            if (IsDesign()) { return; }

            dtSearchFilter.Rows.Clear();
            ViewState[SEARCH_FILTERS] = dtSearchFilter;
            InitSearchFilterData();
            if (this.Controls.Count > 0)
            {
                for (int indexCol = 0; indexCol < this.Columns.Count - 1; indexCol++)
                {
                    if (this.Controls[0].Controls.Count > 0)
                        if (this.Controls[0].Controls[0].Controls.Count > 0)
                            if (this.Controls[0].Controls[0].Controls[0].Controls.Count > 0)
                                ((TextBox)this.Controls[0].Controls[0].Controls[indexCol].Controls[0].Controls[0]).Text = "";
                }
            }
            SearchGridEventArgs eventData = new SearchGridEventArgs(dtSearchFilter);
            OnCancelFilterButtonClick(eventData);
        }
        protected void ddlNumberOfPages_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int iPageSize = Convert.ToInt32(ddlNumberOfPages.SelectedItem.Value);

            OnPageSizeChanged( new PageSizeChangeEventArgs(iPageSize));
        }
        //Function will return if it is design mode or not
        private bool IsDesign()
        {
            if (this.Site != null)
                return this.Site.DesignMode;
            return false;
        }
    }
}
