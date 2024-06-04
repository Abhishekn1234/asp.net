using System.Data.Entity;
using MySql.Data.EntityFramework;
using Employee.Models; 

[DbConfigurationType(typeof(MySqlEFConfiguration))]
public class TaskContext : DbContext
{
    public TaskContext() : base("name=TaskContext")
    {
    }

    public DbSet<Employee.Models.Task> Tasks { get; set; } 
}
