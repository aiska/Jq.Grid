using Newtonsoft.Json;
using System;
using System.Collections;
namespace Jq.Grid
{
	internal class JsonSearchToolBar
	{
		private Hashtable _jsonValues;
		private JQGrid _grid;
		public JsonSearchToolBar(JQGrid grid)
		{
			this._jsonValues = new Hashtable();
			this._grid = grid;
		}
		public string Process()
		{
			SearchToolBarSettings searchToolBarSettings = this._grid.SearchToolBarSettings;
			if (searchToolBarSettings.SearchToolBarAction == SearchToolBarAction.SearchOnKeyPress)
			{
				this._jsonValues["searchOnEnter"] = false;
			}
            return JsonConvert.SerializeObject(this._jsonValues);
		}
	}
}
