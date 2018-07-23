using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelanceTool.Models
{
	public class Language
	{
		public int Id { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public DateTime CreatedAt { get; set; }
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime UpdatedAt { get; set; }

		[StringLength(20)]
		public string Name { get; set; }


		// Relationships and Navigation Properties
		public ICollection<Applicant> Applicants { get; set; }
	}
}