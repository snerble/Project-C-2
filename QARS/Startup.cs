using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using QARS.Data;
using QARS.Data.Services;

using System.Reflection;

using Blazored.Modal;

namespace QARS
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();
			services.AddServerSideBlazor();
			services.AddBlazoredModal();

			// When in Debug mode and EntityFramework isn't running, use the connection string specified in UseConnection. Otherwise use DefaultConnection
			services.AddDbContext<AppDbContext>(options => options.UseSqlite(Configuration.GetConnectionString(
#if DEBUG
				Assembly.GetEntryAssembly().GetName().Name != "ef" ?
					Configuration.GetValue<string>("UseConnection") :
#endif
					"DefaultConnection"
			)));

			services.AddScoped<CarModelServices>();
			services.AddScoped<CarServices>();
			services.AddScoped<AddlocationServices>();
			services.AddScoped<StoreServices>();
			services.AddScoped<FranchiseeServices>();
			services.AddScoped<EmployeeServices>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/_Host");
			});
		}
	}
}
