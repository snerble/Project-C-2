using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	// Code from: https://github.com/aspnet/Entropy/blob/dev/samples/Mvc.RenderViewToString/RazorViewToStringRenderer.cs
	/// <summary>
	/// Provides functionalities to render razor views to string.
	/// </summary>
	// This code is not mine, but I do understand it. I simply can't be bothered to
	// write it myself just to satisfy one teeny tiny optional feature. Consider this
	// the same as installing a NuGet package. Thank you for reading and have a nice day.
	public interface IRazorViewToStringRenderer
	{
		Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
	}

	/// <inheritdoc cref="IRazorViewToStringRenderer"/>
	public class RazorViewRenderer : IRazorViewToStringRenderer
	{

		private readonly IRazorViewEngine _viewEngine;
		private readonly ITempDataProvider _tempDataProvider;
		private readonly IServiceProvider _serviceProvider;

		public RazorViewRenderer(
			IRazorViewEngine viewEngine,
			ITempDataProvider tempDataProvider,
			IServiceProvider serviceProvider)
		{
			_viewEngine = viewEngine;
			_tempDataProvider = tempDataProvider;
			_serviceProvider = serviceProvider;
		}

		public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
		{
			ActionContext actionContext = GetActionContext();
			IView view = FindView(actionContext, viewName);

			using var output = new StringWriter();
			var viewContext = new ViewContext(
				actionContext,
				view,
				new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
				{
					Model = model
				},
				new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
				output,
				new HtmlHelperOptions()
			);

			await view.RenderAsync(viewContext);

			return output.ToString();
		}

		private IView FindView(ActionContext actionContext, string viewName)
		{
			ViewEngineResult getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
			if (getViewResult.Success)
			{
				return getViewResult.View;
			}

			ViewEngineResult findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
			if (findViewResult.Success)
			{
				return findViewResult.View;
			}

			IEnumerable<string> searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);

			throw new InvalidOperationException(string.Join(
				Environment.NewLine,
				new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)
			));
		}

		private ActionContext GetActionContext()
		{
			var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
			return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
		}
	}
}
