using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreelanceTool.Models;

namespace FreelanceTool.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

	    Task SendNewApplicationEmailAsync(Applicant applicant);
    }
}
