using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Jq.Grid
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class GridAttribute : Attribute
    {
        public GridAttribute()
        {
            this.ID = "";
            this.AutoWidth = false;
            this.ShrinkToFit = true;
            this.DataUrl = "";
            this.EditUrl = "";
            this.ColumnReordering = false;
            this.RenderingMode = RenderingMode.Default;
            this.MultiSelect = false;
            this.MultiSelectMode = MultiSelectMode.SelectOnRowClick;
            this.MultiSelectKey = MultiSelectKey.None;
            this.Width = 0;
            this.Height = 0;
        }
        public bool AutoWidth { get; set; }
        public bool ForceFit { get; set; }
        public bool ShrinkToFit { get; set; }
        public string ID { get; set; }
        public string DataUrl { get; set; }
        public string EditUrl { get; set; }
        public bool ColumnReordering { get; set; }
        public RenderingMode RenderingMode { get; set; }
        public bool MultiSelect { get; set; }
        public MultiSelectMode MultiSelectMode { get; set; }
        public MultiSelectKey MultiSelectKey { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        internal static JQGrid<T> Parse<T>(string ID) where T : class
        {
            JQGrid<T> result = GridAttribute.Parse<T>();
            result.ID = ID;
            return result;
        }
        internal static JQGrid<T> Parse<T>() where T : class
        {
            JQGrid<T> result = new JQGrid<T>();
            result.Columns = new List<JQGridColumn>();
            GridAttribute gridAttr = typeof(T).GetCustomAttributes(typeof(GridAttribute), true).FirstOrDefault() as GridAttribute;
            if (gridAttr != null)
            {
                result.ID = gridAttr.ID;
                result.AutoWidth = gridAttr.AutoWidth;
                result.ShrinkToFit = gridAttr.ShrinkToFit;
                result.DataUrl = gridAttr.DataUrl;
                result.EditUrl = gridAttr.EditUrl;
                result.ColumnReordering = gridAttr.ColumnReordering;
                result.RenderingMode = gridAttr.RenderingMode;
                result.MultiSelect = gridAttr.MultiSelect;
                result.MultiSelectMode = gridAttr.MultiSelectMode;
                result.MultiSelectKey = gridAttr.MultiSelectKey;
                result.Width = (gridAttr.Width == 0) ? Unit.Empty : Unit.Pixel(gridAttr.Width);
                result.Height = (gridAttr.Height == 0) ? Unit.Empty : Unit.Pixel(gridAttr.Height);
            }

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
                        IsNew = true;
                        GridColumnAttribute GridAttr = (attr as GridColumnAttribute);
                        col = new JQGridColumn();
                        col.DataField = (string.IsNullOrEmpty(GridAttr.DataField)) ? prop.Name : GridAttr.DataField;
                        col.Visible = GridAttr.Visible;
                        col.HeaderText = (string.IsNullOrEmpty(GridAttr.HeaderText)) ? prop.Name : GridAttr.HeaderText;
                        col.DataType = prop.DeclaringType;
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
                        col.DataType = GridAttr.DataType;
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
                if (IsNew) result.Columns.Add(col);
            }
            //return cols;
            return result;
        }

    }
}
