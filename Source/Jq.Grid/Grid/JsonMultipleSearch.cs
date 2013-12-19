using System;
using System.Collections.Generic;
namespace Jq.Grid
{
	internal class JsonMultipleSearch
	{
		public string groupOp { get; set; }
		public List<MultipleSearchRule> rules { get; set; }
	}
}
