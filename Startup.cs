using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace PaylocityAPI
{
	public class Startup
	{
		readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore()
				.AddApiExplorer();

			services
				.AddSwaggerGen(c =>
				{
//					c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaylocityAPI", Version = "v1.0" });
					var assemblyName = Assembly.GetExecutingAssembly().GetName();
//					var version = $"v{assemblyName.Version.Major}.{assemblyName.Version.MajorRevision}";
					c.SwaggerDoc("v1", new OpenApiInfo
					{
						Version = "v1",
						Title = "Paylocity API",
						Description = "An ASP.NET Core Web API for managing employee onboarding.",
						TermsOfService = new Uri("https://example.com/terms"),
						Contact = new OpenApiContact
						{
							Name = "Example Contact",
							Url = new Uri("https://example.com/contact")
						},
						License = new OpenApiLicense
						{
							Name = "Example License",
							Url = new Uri("https://example.com/license")
						}
					});
				})
				.AddCors(options =>
				{
					options.AddPolicy(MyAllowSpecificOrigins, builder =>
					{
						builder.WithOrigins("http://localhost:4200");
					});
				})
				.AddAnnualCostRepositoryServices()
				.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app
				.UseSwagger(c =>
				{
					c.SerializeAsV2 = true;
				})
				.UseSwaggerUI(c => {
/*					var assemblyName = Assembly.GetExecutingAssembly().GetName();
					var version = $"v{assemblyName.Version.Major}.{assemblyName.Version.MajorRevision}";

					c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{assemblyName.Name} {version}");	*/
					c.RoutePrefix = "swagger";
					c.SwaggerEndpoint("v1/swagger.json", "PaylocityAPI v1.0");
				});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseCors(MyAllowSpecificOrigins);

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
