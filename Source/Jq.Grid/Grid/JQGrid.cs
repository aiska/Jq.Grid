using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Jq.Grid
{
    public class JQGrid<T> : JQGrid where T : class
    {
        internal static JQGrid<T> Create()
        {
            return GridAttribute.Parse<T>();
        }

        internal static JQGrid<T> Create(string ID)
        {
            return GridAttribute.Parse<T>(ID);
        }

        public JQGrid()
            : base()
        {
            //this.Columns = GridColumnAttribute.Parse<T>(this.Columns);
        }

        public JQGrid(string ID)
        //: this()
        {
            this.ID = ID;
        }

        public JsonResult DataBind(IQueryable<T> dataSource)
        {
            AjaxCallBackMode ajaxCallBackMode = this.AjaxCallBackMode;
            if (ajaxCallBackMode != AjaxCallBackMode.RequestData) return null;

            HttpRequest request = HttpContext.Current.Request;
            if (request == null) throw new Exception("Cannot Get Request Variable");
            NameValueCollection queryString = request.HttpMethod == "POST" ? request.Form : request.QueryString;

            IQueryable<T> iqueryable = dataSource;
            Guard.IsNotNull(iqueryable, "DataSource", "should implement the IQueryable interface.");
            int pageIndex = this.GetPageIndex(queryString["page"]);
            int num = this.GetNum(queryString["rows"]);
            string text = queryString["sidx"];
            string sortDirection = queryString["sord"];
            string parentRowID = queryString["parentRowID"];
            bool search = (!string.IsNullOrEmpty(queryString["_search"]) && queryString["_search"] != "false");
            string filters = queryString["filters"];
            string searchField = queryString["searchField"];
            string searchString = queryString["searchString"];
            string searchOper = queryString["searchOper"];
            this.PagerSettings.CurrentPage = pageIndex;
            this.PagerSettings.PageSize = num;
            if (search)
            {
                try
                {
                    if (string.IsNullOrEmpty(filters) && !string.IsNullOrEmpty(searchField))
                    {
                        iqueryable = iqueryable.Where(Util.GetWhereClause(this, searchField, searchString, searchOper));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(filters))
                        {
                            iqueryable = iqueryable.Where(Util.GetWhereClause(this, filters));
                        }
                        else
                        {
                            if (this.ToolBarSettings.ShowSearchToolBar || search)
                            {
                                iqueryable = iqueryable.Where(Util.GetWhereClause(this, queryString));
                            }
                        }
                    }
                }
                catch (DataTypeNotSetException ex)
                {
                    throw ex;
                }
                catch (Exception)
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
                    jsonResult.Data = new object();
                    return jsonResult;
                }
            }
            int num2 = iqueryable.Count();
            int totalPagesCount = (int)Math.Ceiling((double)((float)num2 / (float)num));
            if (string.IsNullOrEmpty(text) && this.SortSettings.AutoSortByPrimaryKey)
            {
                if (this.Columns.Count == 0)
                {
                    throw new Exception("JQGrid must have at least one column defined.");
                }
                text = Util.GetPrimaryKeyField(this);
                sortDirection = "asc";
            }
            if (!string.IsNullOrEmpty(text))
            {
                iqueryable = iqueryable.OrderBy(this.GetSortExpression(text, sortDirection));
            }
            iqueryable = iqueryable.Skip((pageIndex - 1) * num).Take(num);
            this.OnDataResolved(new JQGridDataResolvedEventArgs(this, iqueryable, this.DataSource as IQueryable));
            if (this.TreeGridSettings.Enabled)
            {
                JsonTreeResponse response = new JsonTreeResponse(pageIndex, totalPagesCount, num2, num, iqueryable.Count(), Util.GetFooterInfo(this));
                return Util.ConvertToTreeJson(response, this, iqueryable);
            }
            JsonResponse response2 = new JsonResponse(pageIndex, totalPagesCount, num2, num, iqueryable.Count(), Util.GetFooterInfo(this));
            return Util.ConvertToJson(response2, this, iqueryable);
        }

    }

    public class JQGrid
    {
        private EventHandlerList _events;
        private static readonly object EventDataResolved;
        public event JQGridDataResolvedEventHandler DataResolved
        {
            add
            {
                this.Events.AddHandler(JQGrid.EventDataResolved, value);
            }
            remove
            {
                this.Events.RemoveHandler(JQGrid.EventDataResolved, value);
            }
        }
        public bool AutoWidth { get; set; }
        public bool ForceFit { get; set; }
        public bool ShrinkToFit { get; set; }
        public List<JQGridColumn> Columns { get; set; }
        public List<JQGridHeaderGroup> HeaderGroups { get; set; }
        public EditDialogSettings EditDialogSettings { get; set; }
        public AddDialogSettings AddDialogSettings { get; set; }
        public DeleteDialogSettings DeleteDialogSettings { get; set; }
        public SearchDialogSettings SearchDialogSettings { get; set; }
        public SearchToolBarSettings SearchToolBarSettings { get; set; }
        public PagerSettings PagerSettings { get; set; }
        public ToolBarSettings ToolBarSettings { get; set; }
        public SortSettings SortSettings { get; set; }
        public AppearanceSettings AppearanceSettings { get; set; }
        public HierarchySettings HierarchySettings { get; set; }
        public GroupSettings GroupSettings { get; set; }
        public TreeGridSettings TreeGridSettings { get; set; }
        public GridExportSettings ExportSettings { get; set; }
        public ClientSideEvents ClientSideEvents { get; set; }
        public string ID { get; set; }
        public string DataUrl { get; set; }
        public string EditUrl { get; set; }
        public bool ColumnReordering { get; set; }
        public RenderingMode RenderingMode { get; set; }
        public bool MultiSelect { get; set; }
        public MultiSelectMode MultiSelectMode { get; set; }
        public MultiSelectKey MultiSelectKey { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public object DataSource { get; set; }
        internal bool ShowToolBar
        {
            get
            {
                return this.ToolBarSettings.ShowAddButton || this.ToolBarSettings.ShowDeleteButton || this.ToolBarSettings.ShowEditButton || this.ToolBarSettings.ShowRefreshButton || this.ToolBarSettings.ShowSearchButton || this.ToolBarSettings.ShowViewRowDetailsButton || this.ToolBarSettings.CustomButtons.Count > 0;
            }
        }
        public AjaxCallBackMode AjaxCallBackMode
        {
            get
            {
                string text = HttpContext.Current.Request.Form["oper"];
                string value = HttpContext.Current.Request.QueryString["editMode"];
                string value2 = HttpContext.Current.Request.QueryString["_search"];
                AjaxCallBackMode result = AjaxCallBackMode.RequestData;
                string a;
                if (!string.IsNullOrEmpty(text) && (a = text) != null)
                {
                    if (a == "add")
                    {
                        return AjaxCallBackMode.AddRow;
                    }
                    if (a == "edit")
                    {
                        return AjaxCallBackMode.EditRow;
                    }
                    if (a == "del")
                    {
                        return AjaxCallBackMode.DeleteRow;
                    }
                }
                if (!string.IsNullOrEmpty(value))
                {
                    result = AjaxCallBackMode.EditRow;
                }
                if (!string.IsNullOrEmpty(value2) && Convert.ToBoolean(value2))
                {
                    result = AjaxCallBackMode.Search;
                }
                return result;
            }
        }
        private EventHandlerList Events
        {
            get
            {
                if (this._events == null)
                {
                    this._events = new EventHandlerList();
                }
                return this._events;
            }
        }
        static JQGrid()
        {
            JQGrid.EventDataResolved = new object();
        }

        #region Constructor ...
        public JQGrid()
        {
            this.AutoWidth = false;
            this.ShrinkToFit = true;
            this.EditDialogSettings = new EditDialogSettings();
            this.AddDialogSettings = new AddDialogSettings();
            this.DeleteDialogSettings = new DeleteDialogSettings();
            this.SearchDialogSettings = new SearchDialogSettings();
            this.SearchToolBarSettings = new SearchToolBarSettings();
            this.PagerSettings = new PagerSettings();
            this.ToolBarSettings = new ToolBarSettings();
            this.SortSettings = new SortSettings();
            this.AppearanceSettings = new AppearanceSettings();
            this.HierarchySettings = new HierarchySettings();
            this.GroupSettings = new GroupSettings();
            this.TreeGridSettings = new TreeGridSettings();
            this.ExportSettings = new GridExportSettings();
            this.ClientSideEvents = new ClientSideEvents();
            this.Columns = new List<JQGridColumn>();
            this.HeaderGroups = new List<JQGridHeaderGroup>();
            this.DataUrl = "";
            this.EditUrl = "";
            this.ColumnReordering = false;
            this.RenderingMode = RenderingMode.Default;
            this.MultiSelect = false;
            this.MultiSelectMode = MultiSelectMode.SelectOnRowClick;
            this.MultiSelectKey = MultiSelectKey.None;
            this.Width = Unit.Empty;
            this.Height = Unit.Empty;
        }
        public JQGrid(string ID)
            : this()
        {
            this.ID = ID;
        }
        #endregion

        public JsonResult DataBind(object dataSource)
        {
            this.DataSource = dataSource;
            return this.DataBind();
        }
        public JsonResult DataBind()
        {
            AjaxCallBackMode ajaxCallBackMode = this.AjaxCallBackMode;
            if (ajaxCallBackMode == AjaxCallBackMode.RequestData)
            {
                return this.GetJsonResponse();
            }
            return null;
        }
        public ActionResult ShowEditValidationMessage(string errorMessage)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.StatusCode = 500;
            HttpContext.Current.Response.StatusDescription = errorMessage;
            ContentResult contentResult = new ContentResult();
            contentResult.Content = errorMessage;
            return contentResult;
        }

        private JsonResult FilterDataSource(object dataSource, NameValueCollection queryString, out IQueryable iqueryable)
        {
            iqueryable = (dataSource as IQueryable);
            Guard.IsNotNull(iqueryable, "DataSource", "should implement the IQueryable interface.");
            int pageIndex = this.GetPageIndex(queryString["page"]);
            int num = Convert.ToInt32(queryString["rows"]);
            string text = queryString["sidx"];
            string sortDirection = queryString["sord"];
            string parentRowID = queryString["parentRowID"];
            string search = queryString["_search"];
            string filters = queryString["filters"];
            string searchField = queryString["searchField"];
            string searchString = queryString["searchString"];
            string searchOper = queryString["searchOper"];
            this.PagerSettings.CurrentPage = pageIndex;
            this.PagerSettings.PageSize = num;
            if (!string.IsNullOrEmpty(search) && search != "false")
            {
                try
                {
                    if (string.IsNullOrEmpty(filters) && !string.IsNullOrEmpty(searchField))
                    {
                        iqueryable = iqueryable.Where(Util.GetWhereClause(this, searchField, searchString, searchOper), new object[0]);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(filters))
                        {
                            iqueryable = iqueryable.Where(Util.GetWhereClause(this, filters), new object[0]);
                        }
                        else
                        {
                            if (this.ToolBarSettings.ShowSearchToolBar || search == "true")
                            {
                                iqueryable = iqueryable.Where(Util.GetWhereClause(this, queryString));
                            }
                        }
                    }
                }
                catch (DataTypeNotSetException ex)
                {
                    throw ex;
                }
                catch (Exception)
                {
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
                    jsonResult.Data = new object();
                    return jsonResult;
                }
            }
            int num2 = iqueryable.Count();
            int totalPagesCount = (int)Math.Ceiling((double)((float)num2 / (float)num));
            if (string.IsNullOrEmpty(text) && this.SortSettings.AutoSortByPrimaryKey)
            {
                if (this.Columns.Count == 0)
                {
                    throw new Exception("JQGrid must have at least one column defined.");
                }
                text = Util.GetPrimaryKeyField(this);
                sortDirection = "asc";
            }
            if (!string.IsNullOrEmpty(text))
            {
                iqueryable = iqueryable.OrderBy(this.GetSortExpression(text, sortDirection), new object[0]);
            }
            iqueryable = iqueryable.Skip((pageIndex - 1) * num).Take(num);
            //DataTable dataTable = iqueryable.ToDataTable(this);
            this.OnDataResolved(new JQGridDataResolvedEventArgs(this, iqueryable, this.DataSource as IQueryable));
            if (this.TreeGridSettings.Enabled)
            {
                JsonTreeResponse response = new JsonTreeResponse(pageIndex, totalPagesCount, num2, num, iqueryable.Count(), Util.GetFooterInfo(this));
                //return Util.ConvertToTreeJson(response, this, dataTable);
                return Util.ConvertToTreeJson(response, this, iqueryable);
            }
            JsonResponse response2 = new JsonResponse(pageIndex, totalPagesCount, num2, num, iqueryable.Count(), Util.GetFooterInfo(this));
            return Util.ConvertToJson(response2, this, iqueryable);
            //return Util.ConvertToJson(response2, this, dataTable);
        }
        protected string GetSortExpression(string sortExpression, string sortDirection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<string> list = sortExpression.Split(new char[]
			{
				','
			}).ToList<string>();
            foreach (string current in list)
            {
                if (current.Trim().Length == 0)
                {
                    break;
                }
                List<string> list2 = current.Trim().Split(new char[]
				{
					' '
				}).ToList<string>();
                string arg = list2[0];
                string arg2 = sortDirection;
                if (list2.Count > 1)
                {
                    arg2 = list2[1];
                }
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(",");
                }
                stringBuilder.AppendFormat("{0} {1}", arg, arg2);
            }
            return stringBuilder.ToString();
        }
        private JsonResult GetJsonResponse()
        {
            Guard.IsNotNull(this.DataSource, "DataSource");
            IQueryable queryable;
            HttpRequest request = HttpContext.Current.Request;
            if (request == null) throw new Exception("Cannot Get Request Variable");
            NameValueCollection queryString = request.HttpMethod == "POST" ? request.Form : request.QueryString;
            return this.FilterDataSource(this.DataSource, queryString, out queryable);
        }
        public JQGridEditData GetEditData()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            foreach (string text in HttpContext.Current.Request.Form.Keys)
            {
                if (text != "oper")
                {
                    nameValueCollection[text] = HttpContext.Current.Request.Form[text];
                }
            }
            string text2 = string.Empty;
            foreach (JQGridColumn current in this.Columns)
            {
                if (current.PrimaryKey)
                {
                    text2 = current.DataField;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(nameValueCollection["id"]))
            {
                nameValueCollection[text2] = nameValueCollection["id"];
            }
            JQGridEditData jQGridEditData = new JQGridEditData();
            jQGridEditData.RowData = nameValueCollection;
            jQGridEditData.RowKey = nameValueCollection["id"];
            string text3 = HttpContext.Current.Request.QueryString["parentRowID"];
            if (!string.IsNullOrEmpty(text3))
            {
                jQGridEditData.ParentRowKey = text3;
            }
            return jQGridEditData;
        }
        public JQGridTreeExpandData GetTreeExpandData()
        {
            JQGridTreeExpandData jQGridTreeExpandData = new JQGridTreeExpandData();
            if (HttpContext.Current.Request["nodeid"] != null)
            {
                jQGridTreeExpandData.ParentID = HttpContext.Current.Request["nodeid"];
            }
            if (HttpContext.Current.Request["n_level"] != null)
            {
                jQGridTreeExpandData.ParentLevel = Convert.ToInt32(HttpContext.Current.Request["n_level"]);
            }
            return jQGridTreeExpandData;
        }
        protected int GetPageIndex(string value)
        {
            int result = 1;
            try
            {
                result = int.Parse(value);
            }
            catch (Exception)
            {
            }
            return result;
        }
        protected int GetNum(string value)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(value);
            }
            catch (Exception)
            {
            }
            return result;
        }
        private DataGrid GetExportGrid()
        {
            DataGrid dataGrid = new DataGrid();
            dataGrid.AutoGenerateColumns = false;
            dataGrid.ID = this.ID + "_exportGrid";
            foreach (JQGridColumn current in this.Columns)
            {
                if (current.Visible)
                {
                    BoundColumn boundColumn = new BoundColumn();
                    boundColumn.DataField = current.DataField;
                    string headerText = string.IsNullOrEmpty(current.HeaderText) ? current.DataField : current.HeaderText;
                    boundColumn.HeaderText = headerText;
                    boundColumn.DataFormatString = current.DataFormatString;
                    boundColumn.FooterText = current.FooterValue;
                    dataGrid.Columns.Add(boundColumn);
                }
            }
            return dataGrid;
        }
        private IQueryable GetFilteredDataSource(object dataSource, JQGridState gridState)
        {
            if (this.ExportSettings.ExportDataRange != ExportDataRange.FilteredAndPaged)
            {
                gridState.QueryString["page"] = "1";
                gridState.QueryString["rows"] = "1000000";
            }
            IQueryable result;
            this.FilterDataSource(dataSource, gridState.QueryString, out result);
            return result;
        }
        public JQGridState GetState()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            foreach (string name in HttpContext.Current.Request.QueryString.Keys)
            {
                nameValueCollection.Add(name, HttpContext.Current.Request.QueryString[name]);
            }
            return new JQGridState
            {
                QueryString = nameValueCollection
            };
        }
        public void ExportToCSV(object dataSource, string fileName)
        {
            DataGrid exportGrid = this.GetExportGrid();
            exportGrid.DataSource = dataSource;
            exportGrid.DataBind();
            this.RenderCSVToStream(exportGrid, fileName);
        }
        public void ExportToCSV(object dataSource, string fileName, JQGridState gridState)
        {
            IQueryable filteredDataSource = this.GetFilteredDataSource(dataSource, gridState);
            this.ExportToCSV(filteredDataSource, fileName);
        }
        private void RenderCSVToStream(DataGrid grid, string fileName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.ExportSettings.ExportHeaders)
            {
                foreach (BoundColumn boundColumn in grid.Columns)
                {
                    stringBuilder.AppendFormat("{0}{1}", this.QuoteText(boundColumn.HeaderText), this.ExportSettings.CSVSeparator);
                }
            }
            stringBuilder.Append("\n");
            foreach (DataGridItem dataGridItem in grid.Items)
            {
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (this.Columns[i].Visible)
                    {
                        stringBuilder.AppendFormat("{0}{1}", this.QuoteText(dataGridItem.Cells[i].Text), this.ExportSettings.CSVSeparator);
                    }
                }
                stringBuilder.Append("\n");
            }
            HttpResponse response = HttpContext.Current.Response;
            response.ClearContent();
            response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            response.ContentType = "application/excel";
            response.ContentEncoding = Encoding.Default;
            response.Clear();
            response.Write(stringBuilder.ToString());
            response.Flush();
            response.SuppressContent = true;
        }
        private string QuoteText(string input)
        {
            return string.Format("\"{0}\"", input.Replace("\"", "\"\""));
        }
        protected internal virtual void OnDataResolved(JQGridDataResolvedEventArgs e)
        {
            JQGridDataResolvedEventHandler jQGridDataResolvedEventHandler = (JQGridDataResolvedEventHandler)this.Events[JQGrid.EventDataResolved];
            if (jQGridDataResolvedEventHandler != null)
            {
                jQGridDataResolvedEventHandler(this, e);
            }
        }
        public void ExportToExcel(object dataSource, string fileName)
        {
            DataGrid exportGrid = this.GetExportGrid();
            IQueryable queryable = dataSource as IQueryable;
            if (queryable != null)
            {
                exportGrid.DataSource = queryable.ToDataTable(this);
            }
            else
            {
                exportGrid.DataSource = dataSource;
            }
            exportGrid.DataBind();
            this.RenderExcelToStream(exportGrid, fileName);
        }
        public void ExportToExcel(object dataSource, string fileName, JQGridState gridState)
        {
            IQueryable filteredDataSource = this.GetFilteredDataSource(dataSource, gridState);
            this.ExportToExcel(filteredDataSource, fileName);
        }
        private void RenderExcelToStream(DataGrid grid, string fileName)
        {
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
            grid.RenderControl(writer);
            HttpResponse response = HttpContext.Current.Response;
            response.ClearContent();
            response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            response.ContentType = "application/excel";
            response.Clear();
            response.Write(stringWriter.ToString());
            response.Flush();
            response.SuppressContent = true;
        }
        public DataTable GetExportData(object dataSource)
        {
            DataGrid exportGrid = this.GetExportGrid();
            exportGrid.DataSource = dataSource;
            exportGrid.DataBind();
            return this.ConvertDataGridToDataTable(exportGrid);
        }
        public DataTable GetExportData(object dataSource, JQGridState gridState)
        {
            IQueryable filteredDataSource = this.GetFilteredDataSource(dataSource, gridState);
            return this.GetExportData(filteredDataSource);
        }
        private DataTable ConvertDataGridToDataTable(DataGrid grid)
        {
            DataTable dataTable = new DataTable();
            foreach (DataGridColumn dataGridColumn in grid.Columns)
            {
                dataTable.Columns.Add(dataGridColumn.HeaderText);
            }
            foreach (DataGridItem dataGridItem in grid.Items)
            {
                DataRow dataRow = dataTable.NewRow();
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    dataRow[i] = dataGridItem.Cells[i].Text;
                }
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
    }
}
