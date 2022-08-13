using System;

namespace CarvedRock.Api.GraphQL;

public static class XServiceProvider
{
	public static T Resolve<T>(this IServiceProvider serviceProvider) => (T) serviceProvider.GetService(typeof(T));
}