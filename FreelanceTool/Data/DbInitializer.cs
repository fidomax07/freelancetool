using System.Linq;
using FreelanceTool.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceTool.Data
{
	public static class DbInitializer
    {
	    public static void Seed(ApplicationDataContext context)
	    {
		    if (context.Languages.Any()) return;

		    context.Database.Migrate();

		    var languages = new[]
		    {
			    new Language { Name = "German"},
			    new Language { Name = "French"},
			    new Language { Name = "Italian"}
		    };

		    context.Languages.AddRange(languages);
		    context.SaveChanges();
	    }
	}
}
