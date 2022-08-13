using System;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL
{
    public class CarvedRockSchema: Schema
    {
        public CarvedRockSchema(IServiceProvider resolver): base(resolver)
        {
            Query = resolver.Resolve<CarvedRockQuery>();
            Mutation = resolver.Resolve<CarvedRockMutation>();
            Subscription = resolver.Resolve<CarvedRockSubscription>();
        }
    }
}
