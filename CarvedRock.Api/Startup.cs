using System;
using System.Security.Claims;
using CarvedRock.Api.Data;
using CarvedRock.Api.GraphQL;
using CarvedRock.Api.Repositories;
using GraphQL;
using GraphQL.Execution;
using GraphQL.NewtonsoftJson;
using GraphQL.Server;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Server.Ui.Playground;
using GraphQL.SystemTextJson;
using GraphQL.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarvedRock.Api
{
	public class Startup
	{
		private readonly IConfiguration _config;
		private readonly IHostingEnvironment _env;

		public Startup(IConfiguration config, IHostingEnvironment env)
		{
			_config = config;
			_env = env;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<CarvedRockDbContext>(options =>
				options.UseSqlServer(_config["ConnectionStrings:CarvedRock"]));

			services.AddScoped<ProductRepository>();
			services.AddScoped<ProductReviewRepository>();

			services.AddScoped<IServiceProvider>(s => new FuncServiceProvider(s.GetRequiredService));
			services.AddScoped<CarvedRockSchema>();
			services.AddSingleton<ReviewMessageService>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddScoped<IGraphQLSerializer, GraphQLSerializer>();

			// https://stackoverflow.com/questions/53537521/how-to-implement-authorization-using-graphql-net-at-resolver-function-level
			services.AddTransient<IValidationRule, AuthorizationValidationRule>()
				.AddAuthorization(options => { options.AddPolicy("LoggedIn", p => p.RequireAuthenticatedUser()); });

			// Extensions from graphql.server.core\5.2.2
			services.AddGraphQL(o => o.UnhandledExceptionDelegate = HandleException)
				; /*
				.AddGraphTypes(ServiceLifetime.Scoped)
				//.AddUserContextBuilder(ContextCreator)
				.AddDataLoader();
				//.AddWebSockets();
*/
		}

		static ClaimsPrincipal ContextCreator(HttpContext httpContext)
		{
			return httpContext.User;
		}

		static void HandleException(UnhandledExceptionContext ex)
		{
			Console.Error.WriteLine(ex.ErrorMessage);
			ex.Exception = null!;
		}

		public void Configure(IApplicationBuilder app, CarvedRockDbContext dbContext)
		{
			app.UseCors(builder =>
				builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
			app.UseWebSockets();
			//app.UseGraphQLWebSockets<CarvedRockSchema>("/graphql");
			//app.UseGraphQL<CarvedRockSchema>();
			app.UseGraphQLPlayground(new PlaygroundOptions());
			dbContext.Seed();
		}
	}
}