using System;
namespace Jq.Grid
{
	public class PagerSettings
	{
		public int PageSize { get; set; }
		public int CurrentPage { get; set; }
		public string PageSizeOptions { get; set; }
		public string NoRowsMessage { get; set; }
		public bool ScrollBarPaging { get; set; }
		public string PagingMessage { get; set; }
		public PagerSettings()
		{
			this.PageSize = 10;
			this.CurrentPage = 1;
			this.PageSizeOptions = "[10,20,30]";
			this.NoRowsMessage = "";
			this.ScrollBarPaging = false;
			this.PagingMessage = "";
		}
	}
}
