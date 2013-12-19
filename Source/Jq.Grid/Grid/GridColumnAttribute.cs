using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jq.Grid
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class GridColumnAttribute : Attribute
    {
        public GridColumnAttribute()
        {
            this.Visible = true;
            this.DataField = null;
            this.HeaderText = null;
            this.DataFormatString = null;
            this.Width = 150;
            this.Sortable = true;
            this.Frozen = false;
            this.Resizable = true;
            this.Editable = false;
            this.PrimaryKey = false;
            this.EditType = EditType.TextBox;
            this.EditTypeCustomCreateElement = "";
            this.EditTypeCustomGetValue = "";
            this.SearchType = SearchType.TextBox;
            this.SearchControlID = "";
            this.SearchToolBarOperation = SearchOperation.Contains;
            this.SearchCaseSensitive = false;
            this.EditDialogColumnPosition = 0;
            this.EditDialogRowPosition = 0;
            this.EditDialogLabel = "";
            this.EditDialogFieldPrefix = "";
            this.EditDialogFieldSuffix = "";
            this.EditActionIconsColumn = false;
            this.EditorControlID = "";
            this.DataField = "";
            this.DataFormatString = "";
            this.HeaderText = "";
            this.TextAlign = TextAlign.Left;
            this.Visible = true;
            this.Searchable = true;
            this.HtmlEncode = true;
            this.HtmlEncodeFormatString = true;
            this.ConvertEmptyStringToNull = true;
            this.NullDisplayText = "";
            this.FooterValue = "";
            this.CssClass = "";
            this.GroupTemplate = "";
            this.Fixed = false;
        }
        public bool Fixed { get; set; }
        public int Width { get; set; }
        public bool Sortable { get; set; }
        public bool Frozen { get; set; }
        public bool Resizable { get; set; }
        public bool Editable { get; set; }
        public bool PrimaryKey { get; set; }
        public EditType EditType { get; set; }
        public string EditTypeCustomCreateElement { get; set; }
        public string EditTypeCustomGetValue { get; set; }
        public string EditorControlID { get; set; }
        public SearchType SearchType { get; set; }
        public string SearchControlID { get; set; }
        public Type DataType { get; set; }
        public bool SearchCaseSensitive { get; set; }
        public int EditDialogColumnPosition { get; set; }
        public int EditDialogRowPosition { get; set; }
        public string EditDialogLabel { get; set; }
        public string EditDialogFieldPrefix { get; set; }
        public string EditDialogFieldSuffix { get; set; }
        public bool EditActionIconsColumn { get; set; }
        public string DataField { get; set; }
        public string DataFormatString { get; set; }
        public string HeaderText { get; set; }
        public TextAlign TextAlign { get; set; }
        public bool Visible { get; set; }
        public bool Searchable { get; set; }
        public bool HtmlEncode { get; set; }
        public bool HtmlEncodeFormatString { get; set; }
        public bool ConvertEmptyStringToNull { get; set; }
        public string NullDisplayText { get; set; }
        public SearchOperation SearchToolBarOperation { get; set; }
        public string FooterValue { get; set; }
        public string CssClass { get; set; }
        public string GroupTemplate { get; set; }

        public static JQGridColumn GetGridColumn(GridColumnAttribute attribute, PropertyInfo prop)
        {
            JQGridColumn result = new JQGridColumn();
            result.DataField = (string.IsNullOrEmpty(attribute.DataField)) ? prop.Name : attribute.DataField;
            result.Visible = attribute.Visible;
            result.HeaderText = (string.IsNullOrEmpty(attribute.HeaderText)) ? prop.Name : attribute.HeaderText;
            result.DataFormatString = attribute.DataFormatString;
            result.Fixed = attribute.Fixed;
            result.Width = attribute.Width;
            result.Sortable = attribute.Sortable;
            result.Frozen = attribute.Frozen;
            result.Resizable = attribute.Resizable;
            result.Editable = attribute.Editable;
            result.EditType = attribute.EditType;
            result.EditTypeCustomCreateElement = attribute.EditTypeCustomCreateElement;
            result.EditTypeCustomGetValue = attribute.EditTypeCustomGetValue;
            result.EditorControlID = attribute.EditorControlID;
            result.SearchType = attribute.SearchType;
            result.SearchControlID = attribute.SearchControlID;
            result.DataType = attribute.DataType ?? prop.PropertyType;
            result.SearchCaseSensitive = attribute.SearchCaseSensitive;
            result.EditDialogColumnPosition = attribute.EditDialogColumnPosition;
            result.EditDialogRowPosition = attribute.EditDialogRowPosition;
            result.EditDialogLabel = attribute.EditDialogLabel;
            result.EditDialogFieldPrefix = attribute.EditDialogFieldPrefix;
            result.EditDialogFieldSuffix = attribute.EditDialogFieldSuffix;
            result.EditActionIconsColumn = attribute.EditActionIconsColumn;
            result.TextAlign = attribute.TextAlign;
            result.Searchable = attribute.Searchable;
            result.HtmlEncode = attribute.HtmlEncode;
            result.HtmlEncodeFormatString = attribute.HtmlEncodeFormatString;
            result.ConvertEmptyStringToNull = attribute.ConvertEmptyStringToNull;
            result.NullDisplayText = attribute.NullDisplayText;
            result.SearchToolBarOperation = attribute.SearchToolBarOperation;
            result.FooterValue = attribute.FooterValue;
            result.CssClass = attribute.CssClass;
            result.Width = attribute.Width;
            result.GroupTemplate = attribute.GroupTemplate;
            return result;
        }
        internal static List<JQGridColumn> Parse<T>(List<JQGridColumn> cols) where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //Get Key Attribute
            foreach (PropertyInfo prop in properties)
            {
                bool key = false;
                bool IsNew = false;
                JQGridColumn col = null;

                System.Object[] attributes = prop.GetCustomAttributes(true);
                foreach (object attr in attributes)
                {
                    //if ((attr as Attribute).TypeId.ToString() == "System.Data.Objects.DataClasses.EdmScalarPropertyAttribute")
                    //{
                    //    if ((attr as System.Data.Objects.DataClasses.EdmScalarPropertyAttribute).EntityKeyProperty == true)
                    //        col.PrimaryKey = true;
                    //}
                    //else 


                    if ((attr as Attribute).TypeId.ToString() == "System.ComponentModel.DataAnnotations.KeyAttribute")
                    {
                        key = true;
                        if (col != null) col.PrimaryKey = key;
                    }

                    if (attr is GridColumnAttribute)
                    {
                        GridColumnAttribute GridAttr = (attr as GridColumnAttribute);
                        string field = (string.IsNullOrEmpty(GridAttr.DataField)) ? prop.Name : GridAttr.DataField;
                        col = cols.Where(m => m.DataField.Equals(field)).FirstOrDefault();
                        if (col == null)
                        {
                            col = new JQGridColumn();
                            col.DataField = (string.IsNullOrEmpty(GridAttr.DataField)) ? prop.Name : GridAttr.DataField;
                            IsNew = true;
                        }
                        col.Visible = GridAttr.Visible;
                        col.HeaderText = (string.IsNullOrEmpty(GridAttr.HeaderText)) ? prop.Name : GridAttr.HeaderText;
                        col.DataType = GridAttr.DataType ?? prop.PropertyType;
                        col.PrimaryKey = key;
                        col.DataFormatString = GridAttr.DataFormatString;
                        col.Fixed = GridAttr.Fixed;
                        col.Width = GridAttr.Width;
                        col.Sortable = GridAttr.Sortable;
                        col.Frozen = GridAttr.Frozen;
                        col.Resizable = GridAttr.Resizable;
                        col.Editable = GridAttr.Editable;
                        col.EditType = GridAttr.EditType;
                        col.EditTypeCustomCreateElement = GridAttr.EditTypeCustomCreateElement;
                        col.EditTypeCustomGetValue = GridAttr.EditTypeCustomGetValue;
                        col.EditorControlID = GridAttr.EditorControlID;
                        col.SearchType = GridAttr.SearchType;
                        col.SearchControlID = GridAttr.SearchControlID;
                        col.SearchCaseSensitive = GridAttr.SearchCaseSensitive;
                        col.EditDialogColumnPosition = GridAttr.EditDialogColumnPosition;
                        col.EditDialogRowPosition = GridAttr.EditDialogRowPosition;
                        col.EditDialogLabel = GridAttr.EditDialogLabel;
                        col.EditDialogFieldPrefix = GridAttr.EditDialogFieldPrefix;
                        col.EditDialogFieldSuffix = GridAttr.EditDialogFieldSuffix;
                        col.EditActionIconsColumn = GridAttr.EditActionIconsColumn;
                        col.TextAlign = GridAttr.TextAlign;
                        col.Searchable = GridAttr.Searchable;
                        col.HtmlEncode = GridAttr.HtmlEncode;
                        col.HtmlEncodeFormatString = GridAttr.HtmlEncodeFormatString;
                        col.ConvertEmptyStringToNull = GridAttr.ConvertEmptyStringToNull;
                        col.NullDisplayText = GridAttr.NullDisplayText;
                        col.SearchToolBarOperation = GridAttr.SearchToolBarOperation;
                        col.FooterValue = GridAttr.FooterValue;
                        col.CssClass = GridAttr.CssClass;
                        col.Width = GridAttr.Width;
                        col.GroupTemplate = GridAttr.GroupTemplate;
                    }
                }
                if (IsNew) cols.Add(col);
            }
            return cols;
        }
    }
}
