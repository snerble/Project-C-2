using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using QARS.Data.Models;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace QARS.Areas.Identity.Pages.Account.Manage
{
	public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<DownloadPersonalDataModel> _logger;

        public DownloadPersonalDataModel(
            UserManager<User> userManager,
			SignInManager<User> signInManager,
			IServiceProvider serviceProvider,
            ILogger<DownloadPersonalDataModel> logger)
        {
            _userManager = userManager;
			_signInManager = signInManager;
			_serviceProvider = serviceProvider;
			_logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
			User user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, object>();
			IEnumerable<PropertyInfo> personalDataProps = typeof(User).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
			
			// Include methods that return personal data
			IEnumerable<MethodInfo> personalDataMethods = typeof(User).GetMethods().Where(x => Attribute.IsDefined(x, typeof(PersonalDataAttribute)));

			foreach (PropertyInfo p in personalDataProps)
				personalData.Add(p.Name, p.GetValue(user));

			foreach (MethodInfo method in personalDataMethods)
			{
				// Assert that the method returns a task and that it returns an IDictionary
				if (!typeof(Task).IsAssignableFrom(method.ReturnType) ||
					!method.ReturnType.IsGenericType ||
					!typeof(IDictionary).IsAssignableFrom(method.ReturnType.GetGenericArguments()[0]))
					throw new InvalidOperationException("Personal data method must have a return type that inherits from Task<System.Collections.IDictionary>");

				// Resolve dependencies of the method
				var dependencies = new List<object>();
				foreach (ParameterInfo param in method.GetParameters())
					dependencies.Add(_serviceProvider.GetService(param.ParameterType));

				// Get the dictionary and add it to the personal data
				var task = method.Invoke(user, dependencies.ToArray()) as Task;
				await task.ConfigureAwait(false);
				var result = task.GetType().GetProperty(nameof(Task<object>.Result)).GetValue(task) as IDictionary;
				foreach (var key in result.Keys.Cast<object>())
					personalData.Add(key.ToString(), result[key]);
			}

			if ((await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList().Any())
			{
				IList<UserLoginInfo> logins = await _userManager.GetLoginsAsync(user);
				foreach (UserLoginInfo l in logins)
					personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
			}

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(
				JsonSerializer.SerializeToUtf8Bytes(personalData, new JsonSerializerOptions { WriteIndented = true }),
				"application/json"
			);
        }
    }
}
