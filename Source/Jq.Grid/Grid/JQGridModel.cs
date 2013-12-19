using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
namespace Jq.Grid
{
    public class JQGridModel
    {
        public JQGrid Grid { get; set; }
    }

    public class JQGridModel<T> where T : class
    {
        public JQGridModel() { Grid = JQGrid<T>.Create(); }
        public JQGridModel(string ID) { Grid = JQGrid<T>.Create(ID); }
        public JQGrid<T> Grid { get; set; }
    }

}
