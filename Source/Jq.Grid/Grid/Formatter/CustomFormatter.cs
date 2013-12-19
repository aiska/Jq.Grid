using System;
namespace Jq.Grid
{
	public class CustomFormatter : JQGridColumnFormatter
	{
		public string FormatFunction { get; set; }
		public string UnFormatFunction { get; set; }
	}
}
