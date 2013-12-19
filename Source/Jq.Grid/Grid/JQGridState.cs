using System;
using System.Collections.Specialized;
namespace Jq.Grid
{
	public class JQGridState
	{
		public NameValueCollection QueryString { get; set; }
		public JQGridState()
		{
			this.QueryString = new NameValueCollection();
		}
	}
}
