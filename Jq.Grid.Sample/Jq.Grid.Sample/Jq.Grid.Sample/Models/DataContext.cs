using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Jq.Grid.Sample.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<invheader> InvoiceHeader { get; set; }
    }

    public class DataContextInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        public DataContextInitializer()
        {
            Database.SetInitializer<DataContext>(null);

            try
            {
                using (var context = new DataContext())
                {
                    if (!context.Database.Exists())
                    {
                        try
                        {
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                        catch (Exception)
                        {
                        }
                        Seed(context);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Database could not be initialized.", ex);
            }
        }
        protected override void Seed(DataContext context)
        {
            context.InvoiceHeader.Add(new invheader { id = 13, invdate = DateTime.Parse("2007-10-06"), name = "Client 3", amount = 1000, tax = 0, total = 1000, IsPaid = true });
            context.InvoiceHeader.Add(new invheader { id = 12, invdate = DateTime.Parse("2007-10-06"), name = "Client 2", amount = 700, tax = 140, total = 840, IsPaid = true });
            context.InvoiceHeader.Add(new invheader { id = 11, invdate = DateTime.Parse("2007-10-06"), name = "Client 1", amount = 600, tax = 120, total = 720 });
            context.InvoiceHeader.Add(new invheader { id = 10, invdate = DateTime.Parse("2007-10-06"), name = "Client 2", amount = 100, tax = 20, total = 120 });
            context.InvoiceHeader.Add(new invheader { id = 9, invdate = DateTime.Parse("2007-10-06"), name = "Client 1", amount = 200, tax = 40, total = 240 });
            context.InvoiceHeader.Add(new invheader { id = 8, invdate = DateTime.Parse("2007-10-06"), name = "Client 3", amount = 200, tax = 0, total = 200, IsPaid = true });
            context.InvoiceHeader.Add(new invheader { id = 7, invdate = DateTime.Parse("2007-10-05"), name = "Client 2", amount = 120, tax = 12, total = 134 });
            context.InvoiceHeader.Add(new invheader { id = 6, invdate = DateTime.Parse("2007-10-05"), name = "Client 1", amount = 50, tax = 10, total = 60, note = "" });
            context.InvoiceHeader.Add(new invheader { id = 5, invdate = DateTime.Parse("2007-10-05"), name = "Client 3", amount = 100, tax = 0, total = 100, note = "no tax at all" });
            context.InvoiceHeader.Add(new invheader { id = 4, invdate = DateTime.Parse("2007-10-04"), name = "Client 3", amount = 150, tax = 0, total = 150, note = "no tax" });
            context.InvoiceHeader.Add(new invheader { id = 3, invdate = DateTime.Parse("2007-10-02"), name = "Client 2", amount = 300, tax = 60, total = 360, note = "note invoice 3 &amp; test" });
            context.InvoiceHeader.Add(new invheader { id = 2, invdate = DateTime.Parse("2007-10-03"), name = "Client 1", amount = 200, tax = 40, total = 240, note = "note 2" });
            context.InvoiceHeader.Add(new invheader { id = 1, invdate = DateTime.Parse("2007-10-01"), name = "Client 1", amount = 100, tax = 20, total = 120, note = "note 1" });
            context.SaveChanges();
        }
    }
}