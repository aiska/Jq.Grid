using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jq.Grid.Sample.Models
{
    public class invheader
    {
        [Key, GridColumn(Visible = false)]
        public int id { get; set; }
        [GridColumn(HeaderText = "Invoice Date", DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime invdate { get; set; }
        [GridColumn(HeaderText = "Customer Name")]
        public string name { get; set; }
        [GridColumn(DataFormatString = "{0:C}")]
        public double amount { get; set; }
        [GridColumn(DataFormatString = "{0:0.00\\%}")]
        public double tax { get; set; }
        [GridColumn(DataFormatString = "{0:C}")]
        public double total { get; set; }
        [GridColumn(DataFormatString = "{0:C}")]
        public string note { get; set; }
        [GridColumn(DataFormatString = "{0:Yes;0;No}")]
        public bool IsPaid { get; set; }
    }
}