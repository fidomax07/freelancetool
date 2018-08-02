using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using static System.IO.Path;
using static FreelanceTool.Helpers.Constants;

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
		    var uniqueFileName = GetRandomFileName();
		    return new StringBuilder($"{currentTimestamp}_{uniqueFileName}")
			    .Replace(".", string.Empty)
			    .Append(file.GetExtension())
			    .ToString();
	    }

	    public static async Task<string> TrySaveFile(
		    this IFormFile file, IHostingEnvironment env, string path = null)
	    {
		    var internalPath = Combine(UPLOAD_PATH, path ?? "");
			var uniqueFileName = file.GetUniqueFileName();
		    var filePath = Combine(
			    env.ContentRootPath, internalPath, uniqueFileName);
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
