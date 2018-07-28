namespace FreelanceTool.ViewModels
{
	public class ApplicantLanguageViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsChecked { get; set; }

		public ApplicantLanguageViewModel Check()
		{
			IsChecked = true;

			return this;
		}

		public ApplicantLanguageViewModel Uncheck()
		{
			IsChecked = false;

			return this;
		}
	}
}