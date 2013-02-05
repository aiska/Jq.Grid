using System;

namespace Jq.Grid.Demo.Models
{
    public class invheader
    {
        public int id { get; set; }
        public DateTime invdate { get; set; }
        public string name { get; set; }
        public double amount { get; set; }
        public double tax { get; set; }
        public double total { get; set; }
        public string note { get; set; }
    }
}