using System;
using System.Collections.Generic;

namespace Jq.Grid.Demo.Models
{
    public class JsonExampleGridModel : JQGridModel
    {
        public JsonExampleGridModel()
        {
            Grid = new JQGrid
            {
                ID = "JsonExample",
                Columns = new List<JQGridColumn>
                {
                    new JQGridColumn 
                    {
                        DataField = "id",
                        HeaderText = "Inv No",
                        PrimaryKey = true,
                        DataType = typeof(int),
                    },
                    new JQGridColumn 
                    {
                        DataField = "invdate",
                        HeaderText = "Date",
                        DataType = typeof(DateTime)
                    },
                    new JQGridColumn 
                    {
                        DataField = "name",
                        HeaderText = "Client",
                        DataType = typeof(string)
                    },
                    new JQGridColumn 
                    {
                        DataField = "amount",
                        HeaderText = "Amount",
                        DataType = typeof(double)
                    },
                    new JQGridColumn 
                    {
                        DataField = "tax",
                        HeaderText = "Tax",
                        DataType = typeof(double)
                    },
                    new JQGridColumn 
                    {
                        DataField = "total",
                        HeaderText = "Total",
                        DataType = typeof(double)
                    },
                    new JQGridColumn 
                    {
                        DataField = "note",
                        HeaderText = "Notes",
                        DataType = typeof(string)
                    }
                }
            };
        }
    }
}