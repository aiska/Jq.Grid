using System;
namespace Jq.Grid
{
	internal static class JsonUtil
	{
		internal static string RenderClientSideEvent(string json, string jsName, string eventName)
		{
			string arg = (json.Length > 2) ? "," : "";
			if (!string.IsNullOrEmpty(eventName))
			{
				string value = string.Format("{0}{1}:{2}", arg, jsName, eventName);
				return json.Insert(json.Length - 1, value);
			}
			return json;
		}
	}
}
