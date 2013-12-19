using System;
namespace Jq.Grid
{
	public class MultiSelectClientSideEvents
	{
		public string Show { get; set; }
		public string Hide { get; set; }
		public string Select { get; set; }
		public string MouseOver { get; set; }
		public string MouseOut { get; set; }
		public string Initialized { get; set; }
		public string KeyDown { get; set; }
		public MultiSelectClientSideEvents()
		{
			this.Show = "";
			this.Hide = "";
			this.Select = "";
			this.MouseOver = "";
			this.MouseOut = "";
			this.Initialized = "";
			this.KeyDown = "";
		}
	}
}
