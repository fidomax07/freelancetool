using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreelanceTool.Models
{
	public class Language
	{
		public int Id { get; set; }

		[StringLength(20)]
		public string Name { get; set; }


		// Relationships and Navigation Properties
		public ICollection<Applicant> Applicants { get; set; }
	}
}