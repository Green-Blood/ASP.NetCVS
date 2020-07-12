using System.Data.Entity;

namespace ASPDotNetMVC.Models
{
    public class DemoContext : DbContext
    {
        public DemoContext() : base("ConString")
        {
 
        }
        public DbSet<Employee> Employees { get; set; }
    }
}