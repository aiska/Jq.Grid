using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
namespace Jq.Grid
{
    internal static class Util
    {
        internal class SearchArguments
        {
            public string SearchColumn
            {
                get;
                set;
            }
            public string SearchString
            {
                get;
                set;
            }
            public SearchOperation SearchOperation
            {
                get;
                set;
            }
        }

        private static JsonResponse PrepareJsonResponse(JsonResponse response, JQGrid grid, IQueryable data)
        {
            if (response.records == 0 && !grid.AppearanceSettings.ShowFooter) return response;
            Type type;
            if (data.GetType().GenericTypeArguments == null)
            {
                DataTable dataTable = data.ToDataTable(grid);
                return PrepareJsonResponse(response, grid, dataTable);
            }
            else
            {
                type = data.GetType().GenericTypeArguments[0];
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            response.rows = FillColumn(data, properties, grid.Columns, Util.GetPrimaryKeyIndex(grid));
            return response;
        }

        private static JsonRow[] FillColumn(IQueryable data, PropertyInfo[] properties, List<JQGridColumn> columns, int id)
        {
            List<JsonRow> result = new List<JsonRow>();
            foreach (object c in data)
            {
                List<string> array = new List<string>();
                foreach (JQGridColumn column in columns)
                {
                    array.Add(BindValue(column.DataField, c, properties, column));
                }
                string RowId = array[id];
                JsonRow jsonRow = new JsonRow();
                jsonRow.id = RowId;
                jsonRow.cell = array.ToArray();
                result.Add(jsonRow);
            }
            return result.ToArray();
        }

        private static string BindValue(string DataField, object data, PropertyInfo[] properties, JQGridColumn column)
        {
            if (string.IsNullOrEmpty(DataField)) return "";
            PropertyInfo prop;
            if (DataField.IndexOf('.') > -1)
            {
                int index = DataField.IndexOf('.');
                string PropField = DataField.Substring(0, index);
                index++;
                string NewDataField = DataField.Substring(index, DataField.Length - index);
                prop = properties.Where(p => p.Name.ToLower().Equals(PropField.ToLower())).FirstOrDefault();
                PropertyInfo[] NewProperties = prop.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                object val = prop.GetValue(data, null);
                return BindValue(NewDataField, val, NewProperties, column);
            }
            else
            {
                prop = properties.Where(p => p.Name.ToLower().Equals(DataField.ToLower())).FirstOrDefault();
            }
            if (prop != null)
            {
                if (data == null) return "";
                if (prop.GetValue(data, null) != null)
                {
                    object val = prop.GetValue(data, null);
                    string text = (string.IsNullOrEmpty(column.DataFormatString) ? val.ToString() : column.FormatDataValue(val, column.HtmlEncode));
                    return text;
                }
                else
                {
                    return "";
                }
            }
            return "";
        }

        internal static JsonResponse PrepareJsonResponse<TEntity>(JsonResponse response, JQGrid grid, IQueryable<TEntity> data) where TEntity : class
        {
            if (response.records == 0 && !grid.AppearanceSettings.ShowFooter) return response;
            PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int i = 0;
            foreach (object c in data)
            {
                List<string> array = new List<string>();
                foreach (JQGridColumn t in grid.Columns)
                {
                    bool fieldFound = false;
                    if (!string.IsNullOrEmpty(t.DataField))
                    {
                        foreach (PropertyInfo prop in properties)
                        {
                            if (t.DataField.ToLower().Equals(prop.Name.ToLower()))
                            {
                                fieldFound = true;
                                if (prop.GetValue(c, null) != null)
                                {
                                    object val = prop.GetValue(c, null);
                                    string text = (string.IsNullOrEmpty(t.DataFormatString) ? val.ToString() : t.FormatDataValue(val, t.HtmlEncode));
                                    array.Add(text);
                                }
                                else
                                {
                                    array.Add("");
                                }
                                break;
                            }
                        }
                        if (!fieldFound) array.Add("");
                    }
                }
                string id = array[Util.GetPrimaryKeyIndex(grid)];
                JsonRow jsonRow = new JsonRow();
                jsonRow.id = id;
                jsonRow.cell = array.ToArray();
                response.rows[i] = jsonRow;
                i++;
            }
            return response;
        }
        internal static JsonResponse PrepareJsonResponse(JsonResponse response, JQGrid grid, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] array = new string[grid.Columns.Count];
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    JQGridColumn jQGridColumn = grid.Columns[j];
                    string text = "";
                    if (!string.IsNullOrEmpty(jQGridColumn.DataField))
                    {
                        int ordinal = dt.Columns[jQGridColumn.DataField].Ordinal;
                        text = (string.IsNullOrEmpty(jQGridColumn.DataFormatString) ? dt.Rows[i].ItemArray[ordinal].ToString() : jQGridColumn.FormatDataValue(dt.Rows[i].ItemArray[ordinal], jQGridColumn.HtmlEncode));
                    }
                    array[j] = text;
                }
                string id = array[Util.GetPrimaryKeyIndex(grid)];
                JsonRow jsonRow = new JsonRow();
                jsonRow.id = id;
                jsonRow.cell = array;
                response.rows[i] = jsonRow;
            }
            return response;
        }

        private static object PrepareJsonTreeResponse(JsonTreeResponse response, JQGrid grid, IQueryable data = null)
        {
            PropertyInfo[] properties = data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int i = 0;
            foreach (object c in data)
            {
                List<string> ar = new List<string>();
                foreach (JQGridColumn t in grid.Columns)
                {
                    foreach (PropertyInfo prop in properties)
                    {
                        if (t.DataField.ToLower().Equals(prop.Name.ToLower()))
                        {
                            response.rows[i].Add(t.DataField, prop.GetValue(c, null).ToString());
                            break;
                        }
                    }
                    response.rows[i].Add(t.DataField, "");
                }
                bool tree_levelExist = false;
                bool tree_parentExist = false;
                bool tree_leafExist = false;
                bool tree_expandedExist = false;
                bool tree_loadedExist = false;
                bool tree_iconExist = false;
                int tree_level = 0;
                string tree_parent = "null";
                bool tree_leaf = false;
                bool tree_expanded = false;
                bool tree_loaded = false;
                string tree_icon = "";
                foreach (PropertyInfo prop in properties)
                {
                    if (prop.Name.ToLower() == "tree_level")
                    {
                        tree_levelExist = true;
                        try
                        {
                            tree_level = int.Parse(prop.GetValue(c, null).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_level", tree_level);
                    }
                    if (prop.Name.ToLower() == "tree_parent")
                    {
                        tree_parentExist = true;
                        try
                        {
                            tree_parent = prop.GetValue(c, null).ToString();
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_parent", tree_parent);
                    }
                    if (prop.Name.ToLower() == "tree_leaf")
                    {
                        tree_leafExist = true;
                        try
                        {
                            tree_leaf = bool.Parse(prop.GetValue(c, null).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_leaf", tree_leaf);
                    }
                    if (prop.Name.ToLower() == "tree_expanded")
                    {
                        tree_expandedExist = true;
                        try
                        {
                            tree_expanded = bool.Parse(prop.GetValue(c, null).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_expanded", tree_expanded);
                    }
                    if (prop.Name.ToLower() == "tree_loaded")
                    {
                        tree_loadedExist = true;
                        try
                        {
                            tree_loaded = bool.Parse(prop.GetValue(c, null).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_loaded", tree_loaded);
                    }
                    if (prop.Name.ToLower() == "tree_icon")
                    {
                        tree_iconExist = true;
                        try
                        {
                            tree_icon = prop.GetValue(c, null).ToString();
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_icon", tree_icon);
                    }
                }
                if (!tree_levelExist) response.rows[i].Add("tree_level", tree_level);
                if (!tree_parentExist) response.rows[i].Add("tree_parent", tree_parent);
                if (!tree_leafExist) response.rows[i].Add("tree_leaf", tree_leaf);
                if (!tree_expandedExist) response.rows[i].Add("tree_expanded", tree_expanded);
                if (!tree_loadedExist) response.rows[i].Add("tree_loaded", tree_loaded);
                if (!tree_iconExist) response.rows[i].Add("tree_icon", tree_icon);
                i++;
            }
            return response;
        }

        internal static JsonTreeResponse PrepareJsonTreeResponse<TEntity>(JsonTreeResponse response, JQGrid grid, IQueryable<TEntity> data = null) where TEntity : class
        {
            PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            int i = 0;
            foreach (object c in data)
            {
                List<string> ar = new List<string>();
                foreach (JQGridColumn t in grid.Columns)
                {
                    foreach (PropertyInfo prop in properties)
                    {
                        if (t.DataField.ToLower().Equals(prop.Name.ToLower()))
                        {
                            response.rows[i].Add(t.DataField, prop.GetValue(c, null).ToString());
                            break;
                        }
                    }
                    response.rows[i].Add(t.DataField, "");
                }
                bool tree_levelExist = false;
                bool tree_parentExist = false;
                bool tree_leafExist = false;
                bool tree_expandedExist = false;
                bool tree_loadedExist = false;
                bool tree_iconExist = false;
                int tree_level = 0;
                string tree_parent = "null";
                bool tree_leaf = false;
                bool tree_expanded = false;
                bool tree_loaded = false;
                string tree_icon = "";
                foreach (PropertyInfo prop in properties)
                {
                    if (prop.Name.ToLower() == "tree_level")
                    {
                        tree_levelExist = true;
                        try
                        {
                            tree_level = int.Parse(prop.GetValue(c, null).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_level", tree_level);
                    }
                    if (prop.Name.ToLower() == "tree_parent")
                    {
                        tree_parentExist = true;
                        try
                        {
                            tree_parent = prop.GetValue(c, null).ToString();
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_parent", tree_parent);
                    }
                    if (prop.Name.ToLower() == "tree_leaf")
                    {
                        tree_leafExist = true;
                        try
                        {
                            tree_leaf = bool.Parse(prop.GetValue(c, null).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_leaf", tree_leaf);
                    }
                    if (prop.Name.ToLower() == "tree_expanded")
                    {
                        tree_expandedExist = true;
                        try
                        {
                            tree_expanded = bool.Parse(prop.GetValue(c, null).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_expanded", tree_expanded);
                    }
                    if (prop.Name.ToLower() == "tree_loaded")
                    {
                        tree_loadedExist = true;
                        try
                        {
                            tree_loaded = bool.Parse(prop.GetValue(c, null).ToString());
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_loaded", tree_loaded);
                    }
                    if (prop.Name.ToLower() == "tree_icon")
                    {
                        tree_iconExist = true;
                        try
                        {
                            tree_icon = prop.GetValue(c, null).ToString();
                        }
                        catch (Exception)
                        {
                        }
                        response.rows[i].Add("tree_icon", tree_icon);
                    }
                }
                if (!tree_levelExist) response.rows[i].Add("tree_level", tree_level);
                if (!tree_parentExist) response.rows[i].Add("tree_parent", tree_parent);
                if (!tree_leafExist) response.rows[i].Add("tree_leaf", tree_leaf);
                if (!tree_expandedExist) response.rows[i].Add("tree_expanded", tree_expanded);
                if (!tree_loadedExist) response.rows[i].Add("tree_loaded", tree_loaded);
                if (!tree_iconExist) response.rows[i].Add("tree_icon", tree_icon);
                i++;
            }
            return response;
        }

        internal static JsonTreeResponse PrepareJsonTreeResponse(JsonTreeResponse response, JQGrid grid, DataTable dt, IQueryable data = null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                response.rows[i] = new Hashtable();
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    JQGridColumn jQGridColumn = grid.Columns[j];
                    string value = "";
                    if (!string.IsNullOrEmpty(jQGridColumn.DataField))
                    {
                        int ordinal = dt.Columns[jQGridColumn.DataField].Ordinal;
                        value = (string.IsNullOrEmpty(jQGridColumn.DataFormatString) ? dt.Rows[i].ItemArray[ordinal].ToString() : jQGridColumn.FormatDataValue(dt.Rows[i].ItemArray[ordinal], jQGridColumn.HtmlEncode));
                    }
                    response.rows[i].Add(jQGridColumn.DataField, value);
                }
                try
                {
                    response.rows[i].Add("tree_level", dt.Rows[i]["tree_level"] as int?);
                }
                catch
                {
                }
                try
                {
                    object obj = dt.Rows[i]["tree_parent"];
                    string value2;
                    if (obj is DBNull)
                    {
                        value2 = "null";
                    }
                    else
                    {
                        value2 = Convert.ToString(dt.Rows[i]["tree_parent"]);
                    }
                    response.rows[i].Add("tree_parent", value2);
                }
                catch
                {
                }
                try
                {
                    response.rows[i].Add("tree_leaf", dt.Rows[i]["tree_leaf"] as bool?);
                }
                catch
                {
                }
                try
                {
                    response.rows[i].Add("tree_expanded", dt.Rows[i]["tree_expanded"] as bool?);
                }
                catch
                {
                }
                try
                {
                    response.rows[i].Add("tree_loaded", dt.Rows[i]["tree_loaded"] as bool?);
                }
                catch
                {
                }
                try
                {
                    response.rows[i].Add("tree_icon", dt.Rows[i]["tree_icon"] as string);
                }
                catch
                {
                }
            }
            return response;
        }

        internal static JsonResult ConvertToJson(JsonResponse response, JQGrid grid, IQueryable data)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            jsonResult.Data = Util.PrepareJsonResponse(response, grid, data);
            return jsonResult;
        }

        internal static JsonResult ConvertToJson<TEntity>(JsonResponse response, JQGrid grid, IQueryable<TEntity> data) where TEntity : class
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            jsonResult.Data = Util.PrepareJsonResponse(response, grid, data);
            return jsonResult;
        }
        internal static JsonResult ConvertToJson(JsonResponse response, JQGrid grid, DataTable dt)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            if (response.records == 0)
            {
                if (grid.AppearanceSettings.ShowFooter)
                {
                    jsonResult.Data = Util.PrepareJsonResponse(response, grid, dt);
                }
                else
                {
                    jsonResult.Data = new object[0];
                }
            }
            else
            {
                jsonResult.Data = Util.PrepareJsonResponse(response, grid, dt);
            }
            return jsonResult;
        }
        internal static JsonResult ConvertToTreeJson(JsonTreeResponse response, JQGrid grid, IQueryable Data)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            jsonResult.Data = PrepareJsonTreeResponse(response, grid, Data);
            return jsonResult;
        }

        internal static JsonResult ConvertToTreeJson<TEntity>(JsonTreeResponse response, JQGrid grid, IQueryable<TEntity> Data) where TEntity : class
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            jsonResult.Data = Util.PrepareJsonTreeResponse(response, grid, Data);
            return jsonResult;
        }
        internal static JsonResult ConvertToTreeJson(JsonTreeResponse response, JQGrid grid, DataTable dt)
        {
            JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            jsonResult.Data = Util.PrepareJsonTreeResponse(response, grid, dt);
            return jsonResult;
        }
        public static int GetPrimaryKeyIndex(JQGrid grid)
        {
            if (grid.Columns.Count == 0)
            {
                throw new Exception("JQGrid must have at least one column defined.");
            }
            foreach (JQGridColumn current in grid.Columns)
            {
                if (current.PrimaryKey)
                {
                    return grid.Columns.IndexOf(current);
                }
            }
            return 0;
        }
        public static string GetPrimaryKeyField(JQGrid grid)
        {
            int primaryKeyIndex = Util.GetPrimaryKeyIndex(grid);
            return grid.Columns[primaryKeyIndex].DataField;
        }
        public static Hashtable GetFooterInfo(JQGrid grid)
        {
            Hashtable hashtable = new Hashtable();
            if (grid.AppearanceSettings.ShowFooter)
            {
                foreach (JQGridColumn current in grid.Columns)
                {
                    hashtable[current.DataField] = current.FooterValue;
                }
            }
            return hashtable;
        }
        public static string GetWhereClause(JQGrid grid, NameValueCollection queryString)
        {
            string text = " && ";
            string text2 = "";
            new Hashtable();
            foreach (JQGridColumn current in grid.Columns)
            {
                string text3 = queryString[current.DataField];
                if (!string.IsNullOrEmpty(text3))
                {
                    Util.SearchArguments args = new Util.SearchArguments
                    {
                        SearchColumn = current.DataField,
                        SearchString = text3,
                        SearchOperation = current.SearchToolBarOperation
                    };
                    string str = (text2.Length > 0) ? text : "";
                    string str2 = Util.ConstructLinqFilterExpression(grid, args);
                    text2 = text2 + str + str2;
                }
            }
            return text2;
        }
        public static string GetWhereClause(JQGrid grid, string searchField, string searchString, string searchOper)
        {
            string text = " && ";
            string text2 = "";
            new Hashtable();
            Util.SearchArguments args = new Util.SearchArguments
            {
                SearchColumn = searchField,
                SearchString = searchString,
                SearchOperation = Util.GetSearchOperationFromString(searchOper)
            };
            string str = (text2.Length > 0) ? text : "";
            string str2 = Util.ConstructLinqFilterExpression(grid, args);
            return text2 + str + str2;
        }
        public static string GetWhereClause(JQGrid grid, string filters)
        {
            JsonMultipleSearch jsonMultipleSearch = JsonConvert.DeserializeObject<JsonMultipleSearch>(filters);
            string text = "";
            foreach (MultipleSearchRule current in jsonMultipleSearch.rules)
            {
                Util.SearchArguments args = new Util.SearchArguments
                {
                    SearchColumn = current.field,
                    SearchString = current.data,
                    SearchOperation = Util.GetSearchOperationFromString(current.op)
                };
                string str = (text.Length > 0) ? (" " + jsonMultipleSearch.groupOp + " ") : "";
                text = text + str + Util.ConstructLinqFilterExpression(grid, args);
            }
            return text;
        }
        private static string ConstructLinqFilterExpression(JQGrid grid, Util.SearchArguments args)
        {
            JQGridColumn jQGridColumn = grid.Columns.Find((JQGridColumn c) => c.DataField == args.SearchColumn);
            if (jQGridColumn.DataType == null)
            {
                throw new DataTypeNotSetException("JQGridColumn.DataType must be set in order to perform search operations.");
            }
            string filterExpressionCompare = (jQGridColumn.DataType == typeof(string)) ? "{0} {1} \"{2}\"" : "{0} {1} {2}";
            if (jQGridColumn.DataType == typeof(DateTime))
            {
                DateTime dateTime = DateTime.Parse(args.SearchString);
                string str = string.Format("({0},{1},{2})", dateTime.Year, dateTime.Month, dateTime.Day);
                filterExpressionCompare = "{0} {1} DateTime" + str;
            }
            string str2 = string.Format("{0} != null AND ", args.SearchColumn);
            return str2 + Util.GetLinqExpression(filterExpressionCompare, args, jQGridColumn.SearchCaseSensitive, jQGridColumn.DataType);
        }
        internal static string ConstructLinqFilterExpression(JQAutoComplete autoComplete, Util.SearchArguments args)
        {
            Guard.IsNotNull(autoComplete.DataField, "DataField", "must be set in order to perform search operations. If you get this error from search/export method, make sure you setup(initialize) the grid again prior to filtering/exporting.");
            string filterExpressionCompare = "{0} {1} \"{2}\"";
            return Util.GetLinqExpression(filterExpressionCompare, args, false, typeof(string));
        }
        private static string GetLinqExpression(string filterExpressionCompare, Util.SearchArguments args, bool caseSensitive, Type dataType)
        {
            string text = caseSensitive ? args.SearchString : args.SearchString.ToLower();
            string arg = args.SearchColumn;
            if (dataType != null && dataType == typeof(string) && !caseSensitive)
            {
                arg = string.Format("{0}.ToLower()", args.SearchColumn);
            }
            switch (args.SearchOperation)
            {
                case SearchOperation.IsEqualTo:
                    return string.Format(filterExpressionCompare, arg, "==", text);

                case SearchOperation.IsNotEqualTo:
                    return string.Format(filterExpressionCompare, arg, "<>", text);

                case SearchOperation.IsLessThan:
                    return string.Format(filterExpressionCompare, arg, "<", text);

                case SearchOperation.IsLessOrEqualTo:
                    return string.Format(filterExpressionCompare, arg, "<=", text);

                case SearchOperation.IsGreaterThan:
                    return string.Format(filterExpressionCompare, arg, ">", text);

                case SearchOperation.IsGreaterOrEqualTo:
                    return string.Format(filterExpressionCompare, arg, ">=", text);

                case SearchOperation.BeginsWith:
                    return string.Format("{0}.StartsWith(\"{1}\")", arg, text);

                case SearchOperation.DoesNotBeginWith:
                    return string.Format("!{0}.StartsWith(\"{1}\")", arg, text);

                case SearchOperation.EndsWith:
                    return string.Format("{0}.EndsWith(\"{1}\")", arg, text);

                case SearchOperation.DoesNotEndWith:
                    return string.Format("!{0}.EndsWith(\"{1}\")", arg, text);

                case SearchOperation.Contains:
                    return string.Format("{0}.Contains(\"{1}\")", arg, text);

                case SearchOperation.DoesNotContain:
                    return string.Format("!{0}.Contains(\"{1}\")", arg, text);
            }
            throw new Exception("Invalid search operation.");
        }
        private static SearchOperation GetSearchOperationFromString(string searchOperation)
        {
            switch (searchOperation)
            {
                case "eq":
                    return SearchOperation.IsEqualTo;

                case "ne":
                    return SearchOperation.IsNotEqualTo;

                case "lt":
                    return SearchOperation.IsLessThan;

                case "le":
                    return SearchOperation.IsLessOrEqualTo;

                case "gt":
                    return SearchOperation.IsGreaterThan;

                case "ge":
                    return SearchOperation.IsGreaterOrEqualTo;

                case "in":
                    return SearchOperation.IsIn;

                case "ni":
                    return SearchOperation.IsNotIn;

                case "bw":
                    return SearchOperation.BeginsWith;

                case "bn":
                    return SearchOperation.DoesNotBeginWith;

                case "ew":
                    return SearchOperation.EndsWith;

                case "en":
                    return SearchOperation.DoesNotEndWith;

                case "cn":
                    return SearchOperation.Contains;

                case "nc":
                    return SearchOperation.DoesNotContain;
            }
            throw new Exception("Search operation not known: " + searchOperation);
        }
    }
}
