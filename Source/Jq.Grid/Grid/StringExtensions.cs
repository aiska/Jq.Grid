using System;
namespace Jq.Grid
{
	internal static class StringExtensions
	{
		internal static string RemoveQuotes(this string buffer, string expression)
		{
			return buffer.Replace("\\\"" + expression + "\\\"", expression);
		}
	}
}
