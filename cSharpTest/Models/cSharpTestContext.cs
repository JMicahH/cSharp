using Microsoft.EntityFrameworkCore;
 
	namespace cSharpTest.Models
	{
	    public class cSharpTestContext : DbContext
	    {
	        // base() calls the parent class' constructor passing the "options" parameter along
	        public cSharpTestContext(DbContextOptions<cSharpTestContext> options) : base(options) { }

				    public DbSet<User> Users { get; set; }
				    public DbSet<Participant> Participants { get; set; }
				    public DbSet<Activity> Activities { get; set; }

	    }
	}
