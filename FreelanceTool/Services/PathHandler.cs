using Microsoft.AspNetCore.Hosting;
using static System.IO.Path;
using static FreelanceTool.Helpers.Constants;

namespace FreelanceTool.Services
{
	public class PathHandler
    {
	    private readonly IHostingEnvironment _env;

	    public PathHandler(IHostingEnvironment env)
	    {
		    _env = env;
	    }

	    public string GetUploadPath()
	    {
		    return Combine(_env.ContentRootPath, UPLOAD_PATH);
	    }

	    public string GetCsvPath()
	    {
		    return Combine(GetUploadPath(), CSV_PATH);
	    }
    }
}
