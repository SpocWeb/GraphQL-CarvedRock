using System;
using CarvedRock.Web.Clients;
using CarvedRock.Web.HttpClients;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarvedRock.Web;

public class Startup
{
	private readonly IConfiguration _config;

	public Startup(IConfiguration config)
	{
		_config = config;
	}
	public void ConfigureServices(IServiceCollection services)
	{
		//services.AddMvcCore(options => options.EnableEndpointRouting = false);
		services.AddMvcCore(options => options.EnableEndpointRouting = false).AddRazorViewEngine();
		services.AddRazorPages().AddRazorRuntimeCompilation();
		services.AddSingleton(t => new GraphQLHttpClient(_config["CarvedRockApiUri"]
			//, new DebugJsonSerializer(new NewtonsoftJsonSerializer())));
			//, new NewtonsoftJsonSerializer()));
			, new SystemTextJsonSerializer()));
		//.SystemTextJson.SystemTextJsonSerializer())));
		services.AddSingleton<ProductGraphClient>();
		services.AddHttpClient<ProductHttpClient>(o => o.BaseAddress = new Uri(_config["CarvedRockApiUri"]));
	}

	public void Configure(IApplicationBuilder app, IHostingEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseStaticFiles();
		app.UseMvcWithDefaultRoute();
	}
}