using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Jq.Grid
{
    //internal class JQGridRenderer<T> : JQGridRenderer where T : class
    //{
    //    public string RenderHtml(JQGrid grid, JQGridRenderMode mode)
    //    {
    //    }
    //}
    internal class JQGridRenderer
    {
        public static string RenderHtml<T>(JQGrid grid, JQGridRenderMode mode) where T : class
        {
            JQGrid gridT = grid as JQGrid;
            return RenderHtml(gridT, mode);
        }
        public static string RenderHtml(JQGrid grid, JQGridRenderMode mode)
        {
            if (string.IsNullOrEmpty(grid.ID))
            {
                throw new Exception("You need to set ID for this grid.");
            }
            string text = "";
            if (mode == JQGridRenderMode.Full || mode == JQGridRenderMode.Partial)
            {
                text = "<table id=\"{0}\"></table>";
                if (grid.ToolBarSettings.ToolBarPosition != ToolBarPosition.Hidden)
                {
                    text += "<div id=\"{0}_pager\"></div>";
                }
                text = string.Format(text, grid.ID);
            }
            if (mode == JQGridRenderMode.Full || mode == JQGridRenderMode.Script)
            {
                if (grid.HierarchySettings.HierarchyMode == HierarchyMode.Child || grid.HierarchySettings.HierarchyMode == HierarchyMode.ParentAndChild)
                {
                    text += GetChildSubGridJavaScript(grid);
                }
                else
                {
                    text += GetStartupJavascript(grid, false);
                }
            }
            return text;
        }
        private static string GetStartupJavascript(JQGrid grid, bool subgrid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<script type=\"text/javascript\">\n");
            stringBuilder.Append("$(document).ready(function() {");
            stringBuilder.Append(GetStartupOptions(grid, subgrid));
            stringBuilder.Append("});");
            stringBuilder.Append("</script>");
            return stringBuilder.ToString();
        }
        private static string GetChildSubGridJavaScript(JQGrid grid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<script type=\"text/javascript\">\n");
            stringBuilder.AppendFormat("function showSubGrid_{0}(subgrid_id, row_id, message, suffix) {{", grid.ID);
            stringBuilder.Append("var subgrid_table_id, pager_id;\r\n\t\t                subgrid_table_id = subgrid_id+'_t';\r\n\t\t                pager_id = 'p_'+ subgrid_table_id;\r\n                        if (suffix) { subgrid_table_id += suffix; pager_id += suffix;  }\r\n                        if (message) $('#'+subgrid_id).append(message);                        \r\n\t\t                $('#'+subgrid_id).append('<table id=\" + subgrid_table_id + \" class=scroll></table><div id=\" + pager_id + \" class=scroll></div>');\r\n                ");
            stringBuilder.Append(GetStartupOptions(grid, true));
            stringBuilder.Append("}");
            stringBuilder.Append("</script>");
            return stringBuilder.ToString();
        }
        private static string GetStartupOptions(JQGrid grid, bool subGrid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string initGrid = subGrid ? "$('#' + subgrid_table_id)" : string.Format("$('#{0}')", grid.ID);
            string initPager = subGrid ? "$('#' + pager_id)" : string.Format("$('#{0}')", grid.ID + "_pager");
            string pagerSelectorID = subGrid ? "'#' + pager_id" : string.Format("'#{0}'", grid.ID + "_pager");
            string initParent = subGrid ? "&parentRowID=' + row_id + '" : string.Empty;
            string initDataUrl = (grid.DataUrl.IndexOf("?") > 0) ? "&" : "?";
            string initEditUrl = (grid.EditUrl.IndexOf("?") > 0) ? "&" : "?";
            string initJs = string.Format("{0}{1}jqGridID={2}{3}", grid.DataUrl, initDataUrl, grid.ID, initParent);
            string initJsEdit = string.Format("{0}{1}jqGridID={2}&editMode=1{3}", grid.EditUrl, initEditUrl, grid.ID, initParent);

            if (grid.Columns.Count > 0 && grid.Columns[0].Frozen)
            {
                grid.AppearanceSettings.ShrinkToFit = false;
            }
            stringBuilder.AppendFormat("{0}.jqGrid({{", initGrid);
            stringBuilder.AppendFormat("url: '{0}'", initJs);
            stringBuilder.AppendFormatIfFalse(string.IsNullOrWhiteSpace(grid.EditUrl), ",editurl: '{0}'", initJsEdit);
            stringBuilder.Append(",mtype: 'POST'");
            stringBuilder.Append(",datatype: 'json'");
            stringBuilder.AppendFormat(",page: {0}", grid.PagerSettings.CurrentPage);
            stringBuilder.AppendFormat(",colNames: {0}", GetColNames(grid));
            stringBuilder.AppendFormat(",colModel: {0}", GetColModel(grid));
            stringBuilder.AppendFormat(",viewrecords: true", new object[0]);
            stringBuilder.AppendFormat(",scrollrows: false", new object[0]);
            if (grid.Columns.Count > 0)
            {
                stringBuilder.AppendFormat(",prmNames: {{ id: \"{0}\" }}", Util.GetPrimaryKeyField(grid));
            }
            if (grid.AppearanceSettings.ShowFooter)
            {
                stringBuilder.Append(",footerrow: true");
                stringBuilder.Append(",userDataOnFooter: true");
            }
            if (!grid.AppearanceSettings.ShrinkToFit)
            {
                stringBuilder.Append(",shrinkToFit: false");
            }
            stringBuilder.Append(",headertitles: true");
            if (grid.ColumnReordering)
            {
                stringBuilder.Append(",sortable: true");
            }
            if (grid.AppearanceSettings.ScrollBarOffset != 18)
            {
                stringBuilder.AppendFormat(",scrollOffset: {0}", grid.AppearanceSettings.ScrollBarOffset);
            }
            if (grid.AppearanceSettings.RightToLeft)
            {
                stringBuilder.Append(",direction: 'rtl'");
            }
            if (grid.AutoWidth)
            {
                stringBuilder.Append(",autowidth: true");
            }
            if (grid.ForceFit)
            {
                stringBuilder.Append(",forceFit: true");
            }
            if (!grid.ShrinkToFit)
            {
                stringBuilder.Append(",shrinkToFit: false");
            }
            if (grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.Bottom || grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.TopAndBottom)
            {
                stringBuilder.AppendFormat(",pager: {0}", initPager);
            }
            if (grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.Top || grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.TopAndBottom)
            {
                stringBuilder.Append(",toppager: true");
            }
            if (grid.RenderingMode == RenderingMode.Optimized)
            {
                if (grid.HierarchySettings.HierarchyMode != HierarchyMode.None)
                {
                    throw new Exception("Optimized rendering is not compatible with hierarchy.");
                }
                stringBuilder.Append(",gridview: true");
            }
            if (grid.HierarchySettings.HierarchyMode == HierarchyMode.Parent || grid.HierarchySettings.HierarchyMode == HierarchyMode.ParentAndChild)
            {
                stringBuilder.Append(",subGrid: true");
                stringBuilder.AppendFormat(",subGridOptions: {0}", grid.HierarchySettings.ToJSON());
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.SubGridRowExpanded))
            {
                stringBuilder.AppendFormat(",subGridRowExpanded: {0}", grid.ClientSideEvents.SubGridRowExpanded);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.ServerError))
            {
                stringBuilder.AppendFormat(",errorCell: {0}", grid.ClientSideEvents.ServerError);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowSelect))
            {
                stringBuilder.AppendFormat(",onSelectRow: {0}", grid.ClientSideEvents.RowSelect);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.ColumnSort))
            {
                stringBuilder.AppendFormat(",onSortCol: {0}", grid.ClientSideEvents.ColumnSort);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowDoubleClick))
            {
                stringBuilder.AppendFormat(",ondblClickRow: {0}", grid.ClientSideEvents.RowDoubleClick);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.RowRightClick))
            {
                stringBuilder.AppendFormat(",onRightClickRow: {0}", grid.ClientSideEvents.RowRightClick);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.LoadDataError))
            {
                stringBuilder.AppendFormat(",loadError: {0}", grid.ClientSideEvents.LoadDataError);
            }
            else
            {
                stringBuilder.AppendFormat(",loadError: {0}", "jqGrid_aspnet_loadErrorHandler");
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.GridInitialized))
            {
                stringBuilder.AppendFormat(",gridComplete: {0}", grid.ClientSideEvents.GridInitialized);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.BeforeAjaxRequest))
            {
                stringBuilder.AppendFormat(",beforeRequest: {0}", grid.ClientSideEvents.BeforeAjaxRequest);
            }
            if (!string.IsNullOrEmpty(grid.ClientSideEvents.AfterAjaxRequest))
            {
                stringBuilder.AppendFormat(",loadComplete: {0}", grid.ClientSideEvents.AfterAjaxRequest);
            }
            if (grid.TreeGridSettings.Enabled)
            {
                stringBuilder.AppendFormat(",treeGrid: true", new object[0]);
                stringBuilder.AppendFormat(",treedatatype: 'json'", new object[0]);
                stringBuilder.AppendFormat(",treeGridModel: 'adjacency'", new object[0]);
                string arg5 = "{ level_field: 'tree_level', parent_id_field: 'tree_parent', leaf_field: 'tree_leaf', expanded_field: 'tree_expanded', loaded: 'tree_loaded', icon_field: 'tree_icon' }";
                stringBuilder.AppendFormat(",treeReader: {0}", arg5);
                stringBuilder.AppendFormat(",ExpandColumn: '{0}'", GetFirstVisibleDataField(grid));
                Hashtable hashtable = new Hashtable();
                if (!string.IsNullOrEmpty(grid.TreeGridSettings.CollapsedIcon))
                {
                    hashtable.Add("plus", grid.TreeGridSettings.CollapsedIcon);
                }
                if (!string.IsNullOrEmpty(grid.TreeGridSettings.ExpandedIcon))
                {
                    hashtable.Add("minus", grid.TreeGridSettings.ExpandedIcon);
                }
                if (!string.IsNullOrEmpty(grid.TreeGridSettings.LeafIcon))
                {
                    hashtable.Add("leaf", grid.TreeGridSettings.LeafIcon);
                }
                if (hashtable.Count > 0)
                {
                    stringBuilder.AppendFormat(",treeIcons: {0}", JsonConvert.SerializeObject(hashtable));
                }
            }
            if (!grid.AppearanceSettings.HighlightRowsOnHover)
            {
                stringBuilder.Append(",hoverrows: false");
            }
            if (grid.AppearanceSettings.AlternateRowBackground)
            {
                stringBuilder.Append(",altRows: true");
            }
            if (grid.AppearanceSettings.ShowRowNumbers)
            {
                stringBuilder.Append(",rownumbers: true");
            }
            if (grid.AppearanceSettings.RowNumbersColumnWidth != 25)
            {
                stringBuilder.AppendFormat(",rownumWidth: {0}", grid.AppearanceSettings.RowNumbersColumnWidth.ToString());
            }
            if (grid.PagerSettings.ScrollBarPaging)
            {
                stringBuilder.AppendFormat(",scroll: 1", new object[0]);
            }
            stringBuilder.AppendFormat(",rowNum: {0}", grid.PagerSettings.PageSize.ToString());
            stringBuilder.AppendFormat(",rowList: {0}", grid.PagerSettings.PageSizeOptions.ToString());
            if (!string.IsNullOrEmpty(grid.PagerSettings.NoRowsMessage))
            {
                stringBuilder.AppendFormat(",emptyrecords: '{0}'", grid.PagerSettings.NoRowsMessage.ToString());
            }
            stringBuilder.AppendFormat(",editDialogOptions: {0}", GetEditOptions(grid));
            stringBuilder.AppendFormat(",addDialogOptions: {0}", GetAddOptions(grid));
            stringBuilder.AppendFormat(",delDialogOptions: {0}", GetDelOptions(grid));
            stringBuilder.AppendFormat(",searchDialogOptions: {0}", GetSearchOptions(grid));
            string format;
            if (grid.TreeGridSettings.Enabled)
            {
                format = ",jsonReader: {{ id: \"{0}\", repeatitems:false,subgrid:{{repeatitems:false}} }}";
            }
            else
            {
                format = ",jsonReader: {{ id: \"{0}\" }}";
            }
            stringBuilder.AppendFormat(format, grid.Columns[Util.GetPrimaryKeyIndex(grid)].DataField);
            if (!string.IsNullOrEmpty(grid.SortSettings.InitialSortColumn))
            {
                stringBuilder.AppendFormat(",sortname: '{0}'", grid.SortSettings.InitialSortColumn);
            }
            stringBuilder.AppendFormat(",sortorder: '{0}'", grid.SortSettings.InitialSortDirection.ToString().ToLower());
            if (grid.MultiSelect)
            {
                stringBuilder.Append(",multiselect: true");
                if (grid.MultiSelectMode == MultiSelectMode.SelectOnCheckBoxClickOnly)
                {
                    stringBuilder.AppendFormat(",multiboxonly: true", grid.MultiSelect.ToString().ToLower());
                }
                if (grid.MultiSelectKey != MultiSelectKey.None)
                {
                    stringBuilder.AppendFormat(",multikey: '{0}'", GetMultiKeyString(grid.MultiSelectKey));
                }
            }
            if (!string.IsNullOrEmpty(grid.AppearanceSettings.Caption))
            {
                stringBuilder.AppendFormat(",caption: '{0}'", grid.AppearanceSettings.Caption);
            }
            if (!grid.Width.IsEmpty)
            {
                stringBuilder.AppendFormat(",width: '{0}'", grid.Width.ToString().Replace("px", ""));
            }
            if (!grid.Height.IsEmpty)
            {
                stringBuilder.AppendFormat(",height: '{0}'", grid.Height.ToString().Replace("px", ""));
            }
            if (grid.GroupSettings.GroupFields.Count > 0)
            {
                stringBuilder.Append(grid.GroupSettings.ToJSON());
            }
            stringBuilder.AppendFormat(",viewsortcols: [{0},'{1}',{2}]", "false", grid.SortSettings.SortIconsPosition.ToString().ToLower(), (grid.SortSettings.SortAction == SortAction.ClickOnHeader) ? "true" : "false");
            stringBuilder.AppendFormat("}})\r", new object[0]);
            stringBuilder.Append(GetToolBarOptions(grid, subGrid, pagerSelectorID));
            if (!grid.PagerSettings.ScrollBarPaging)
            {
                stringBuilder.AppendFormat(".bindKeys()", new object[0]);
            }
            stringBuilder.Append(";");
            stringBuilder.Append(GetLoadErrorHandler());
            stringBuilder.Append(";");
            if (grid.HeaderGroups.Count > 0)
            {
                List<Hashtable> list = new List<Hashtable>();
                foreach (JQGridHeaderGroup current in grid.HeaderGroups)
                {
                    list.Add(current.ToHashtable());
                }
                stringBuilder.AppendFormat("{0}.setGroupHeaders( {{ useColSpanStyle:true,groupHeaders:{1} }});", initGrid, JsonConvert.SerializeObject(list));
            }
            if (grid.ToolBarSettings.ShowSearchToolBar)
            {
                stringBuilder.AppendFormat("{0}.filterToolbar({1});", initGrid, new JsonSearchToolBar(grid).Process());
            }
            if (grid.Columns.Count > 0 && grid.Columns[0].Frozen)
            {
                stringBuilder.AppendFormat("{0}.setFrozenColumns();", initGrid);
            }
            return stringBuilder.ToString();
        }
        private static string GetEditOptions(JQGrid grid)
        {
            JsonEditDialog jsonEditDialog = new JsonEditDialog(grid);
            return jsonEditDialog.Process();
        }
        private static string GetAddOptions(JQGrid grid)
        {
            JsonAddDialog jsonAddDialog = new JsonAddDialog(grid);
            return jsonAddDialog.Process();
        }
        private static string GetDelOptions(JQGrid grid)
        {
            JsonDelDialog jsonDelDialog = new JsonDelDialog(grid);
            return jsonDelDialog.Process();
        }
        private static string GetSearchOptions(JQGrid grid)
        {
            JsonSearchDialog jsonSearchDialog = new JsonSearchDialog(grid);
            return jsonSearchDialog.Process();
        }
        private static string GetColNames(JQGrid grid)
        {
            string[] array = new string[grid.Columns.Count];
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                JQGridColumn jQGridColumn = grid.Columns[i];
                array[i] = (string.IsNullOrEmpty(jQGridColumn.HeaderText) ? jQGridColumn.DataField : jQGridColumn.HeaderText);
            }
            return JsonConvert.SerializeObject(array);
        }
        private static string GetColModel(JQGrid grid)
        {
            Hashtable[] array = new Hashtable[grid.Columns.Count];
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                JsonColModel jsonColModel = new JsonColModel(grid.Columns[i], grid);
                array[i] = jsonColModel.JsonValues;
            }
            string input = JsonConvert.SerializeObject(array);
            return JsonColModel.RemoveQuotesForJavaScriptMethods(input, grid);
        }
        private static string GetMultiKeyString(MultiSelectKey key)
        {
            switch (key)
            {
                case MultiSelectKey.Shift:
                    return "shiftKey";

                case MultiSelectKey.Ctrl:
                    return "ctrlKey";

                case MultiSelectKey.Alt:
                    return "altKey";

                default:
                    throw new Exception("Should not be here.");
            }
        }
        private static string GetToolBarOptions(JQGrid grid, bool subGrid, string pagerSelectorID)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (grid.ShowToolBar)
            {
                JsonToolBar obj = new JsonToolBar(grid.ToolBarSettings);
                if (!subGrid)
                {
                    stringBuilder.AppendFormat(".navGrid('#{0}',{1},{2},{3},{4},{5} )", new object[]
					{
						grid.ID + "_pager",
						JsonConvert.SerializeObject(obj),
						string.Format("$('#{0}').getGridParam('editDialogOptions')", grid.ID),
						string.Format("$('#{0}').getGridParam('addDialogOptions')", grid.ID),
						string.Format("$('#{0}').getGridParam('delDialogOptions')", grid.ID),
						string.Format("$('#{0}').getGridParam('searchDialogOptions')", grid.ID)
					});
                }
                else
                {
                    stringBuilder.AppendFormat(".navGrid('#' + pager_id,{0},{1},{2},{3},{4} )", new object[]
					{
						JsonConvert.SerializeObject(obj),
						"$('#' + subgrid_table_id).getGridParam('editDialogOptions')",
						"$('#' + subgrid_table_id).getGridParam('addDialogOptions')",
						"$('#' + subgrid_table_id).getGridParam('delDialogOptions')",
						"$('#' + subgrid_table_id).getGridParam('searchDialogOptions')"
					});
                }
                foreach (JQGridToolBarButton current in grid.ToolBarSettings.CustomButtons)
                {
                    if (grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.Bottom || grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.TopAndBottom)
                    {
                        JsonCustomButton jsonCustomButton = new JsonCustomButton(current);
                        stringBuilder.AppendFormat(".navButtonAdd({0},{1})", pagerSelectorID, jsonCustomButton.Process());
                    }
                    if (grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.TopAndBottom || grid.ToolBarSettings.ToolBarPosition == ToolBarPosition.Top)
                    {
                        JsonCustomButton jsonCustomButton2 = new JsonCustomButton(current);
                        stringBuilder.AppendFormat(".navButtonAdd({0},{1})", pagerSelectorID.Replace("_pager", "_toppager"), jsonCustomButton2.Process());
                    }
                }
                return stringBuilder.ToString();
            }
            return string.Empty;
        }
        private static string GetLoadErrorHandler()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("\n");
            stringBuilder.Append("function jqGrid_aspnet_loadErrorHandler(xht, st, handler) {");
            stringBuilder.Append("$(document.body).css('font-size','100%'); $(document.body).html(xht.responseText);");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
        private string GetJQuerySubmit(JQGrid grid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("\r\n                        var _theForm = document.getElementsByTagName('FORM')[0];\r\n                        $(_theForm).submit( function() \r\n                        {{  \r\n                            $('#{0}').attr('value', $('#{1}').getGridParam('selrow'));                            \r\n                        }});\r\n                       ", grid.ID + "_SelectedRow", grid.ID, grid.ID + "_CurrentPage");
            return stringBuilder.ToString();
        }
        private static string GetFirstVisibleDataField(JQGrid grid)
        {
            foreach (JQGridColumn current in grid.Columns)
            {
                if (current.Visible)
                {
                    return current.DataField;
                }
            }
            return grid.Columns[0].DataField;
        }
    }
}
