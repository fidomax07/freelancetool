using Microsoft.AspNetCore.Hosting;
using static System.IO.Path;
using static FreelanceTool.Helpers.Constants;

namespace FreelanceTool.Services
{
	public class PathHandler
    {
	    public static string GetUploadPath(IHostingEnvironment env)
		{
		    return Combine(env.ContentRootPath, UPLOAD_PATH);
	    }

	    public static string GetCsvPath(IHostingEnvironment env)
	    {
		    return Combine(GetUploadPath(env), CSV_PATH);
	    }
    }
}
