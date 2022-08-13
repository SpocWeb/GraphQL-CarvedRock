using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CarvedRock.Web.Models;
using GraphQL;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace CarvedRock.Web
{
	public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        [TestCase(@"{
	""data"": {
		""product"": {
			""id"": 1,
			""name"": ""Mountain Walkers"",
			""price"": 219.5,
			""rating"": 4,
			""photoFileName"": ""shutterstock_66842440.jpg"",
			""description"": ""Use these sturdy shoes to pass any mountain range with ease."",
			""stock"": 12,
			""introducedAt"": ""2022-07-13T02:59:02.3182129+02:00"",
			""reviews"": [
				{
					""title"": ""Crossed the Himalayas"",
					""review"": ""First released almost 30 years ago, the Titan 26078 is a classic work boot. It’s also one of Timberland’s all time best sellers.""
				},
				{
					""title"": ""Comfortable"",
					""review"": ""One of the most comfortable hiking boots available, each pair comes complete with the Power Fit Comfort System, designed to offer maximum support at key areas of your feet.""
				}
			]
		}
	},
	""extensions"": {
		""tracing"": {
			""version"": 1,
			""startTime"": ""2022-08-13T15:10:13.4954361Z"",
			""endTime"": ""2022-08-13T15:10:16.3564361Z"",
			""duration"": 2860540900,
			""parsing"": {
				""startOffset"": 1521500,
				""duration"": 3792900
			},
			""validation"": {
				""startOffset"": 5315299,
				""duration"": 4419600
			},
			""execution"": {
				""resolvers"": []
			}
		}
	}
}")]
		public static void TestJsonDeSerialize(string json)
        {
	        var options = new JsonSerializerOptions();
	        options.PropertyNameCaseInsensitive = true;
	        options.Converters.Add(new JsonStringEnumConverter());

	        var response = JsonSerializer.Deserialize<GraphQLResponse<ProductModel>> (json, options);

	        Console.WriteLine($"Team.City={response.Data.Description}");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("https://localhost:5002")
                .UseStartup<Startup>();
    }
}
