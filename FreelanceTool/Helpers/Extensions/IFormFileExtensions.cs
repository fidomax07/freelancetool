using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FreelanceTool.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.AspNetCore.Http
{
	public static class IFormFileExtensions
    {
	    public static string GetExtension(this IFormFile file)
	    {
		    return Path.GetExtension(file.FileName);
	    }

		public static string GetUniqueFileName(this IFormFile file)
	    {
		    var currentTimestamp = DateTime.UtcNow.ToString("yyyMMddHHmmss");
		    var uniqueFileName = Path.GetRandomFileName();
		    return new StringBuilder($"{currentTimestamp}_{uniqueFileName}")
			    .Replace(".", string.Empty)
			    .Append(file.GetExtension())
			    .ToString();
	    }

	    public static async Task<string> TryUploadFile(
		    this IFormFile file, IHostingEnvironment host)
	    {
		    var uniqueFileName = file.GetUniqueFileName();
		    var filePath = Path.Combine(
			    host.ContentRootPath, Constants.UPLOAD_PATH, uniqueFileName);
		    try
		    {
			    using (var stream = new FileStream(filePath, FileMode.Create))
				    await file.CopyToAsync(stream);

			    // Verify if upload was successful
			    if (!File.Exists(filePath)) return null;
		    }
		    catch (Exception)
		    {
			    return null;
		    }

		    return uniqueFileName;
	    }
	}
}
