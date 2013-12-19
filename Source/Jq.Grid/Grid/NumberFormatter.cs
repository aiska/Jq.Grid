using System;
namespace Jq.Grid
{
	public class NumberFormatter : JQGridColumnFormatter
	{
		public string ThousandsSeparator { get; set; }
		public string DefaultValue { get; set; }
		public string DecimalSeparator { get; set; }
		public int DecimalPlaces { get; set; }
	}
}
