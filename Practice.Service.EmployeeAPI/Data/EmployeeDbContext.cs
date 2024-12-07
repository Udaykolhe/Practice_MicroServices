using Microsoft.EntityFrameworkCore;
using Practice.Service.EmployeeAPI.Model;

namespace Practice.Service.EmployeeAPI.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> employees { get; set; }

    }
}
