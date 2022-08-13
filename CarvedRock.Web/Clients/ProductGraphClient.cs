using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CarvedRock.Api.GraphQL.Messaging;
using CarvedRock.Web.Models;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

namespace CarvedRock.Web.Clients
{
    public class ProductGraphClient
    {
        private readonly GraphQLHttpClient _client;

        public ProductGraphClient(GraphQLHttpClient client)
        {
            _client = client;
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var query = new GraphQLRequest
            {
                Query = @" 
                query productQuery($productId: ID!)
                { product(id: $productId) 
                    { id name price rating photoFileName description stock introducedAt 
                      reviews { title review }
                    }
                }",
                Variables = new {productId = id}
            };
            var response = await _client.SendQueryAsync<ProductModel>(query);
            //var response = await _client.PostAsync(query);
            return response.Data;//.GetDataFieldAs<ProductModel>("product");
        }

        public async Task<ProductReviewModel> AddReview(ProductReviewModel review)
        {
            var query = new GraphQLRequest
            {
                Query = @" 
                mutation($review: reviewInput!)
                {
                    createReview(review: $review)
                    {
                        id
                    }
                }",
                Variables = new { review }
            };
            //var response = await _client.PostAsync(query);
            //var reviewReturned = response.GetDataFieldAs<ProductReviewModel>("createReview");
            var response = await _client.SendMutationAsync<ProductReviewModel>(query);
            return response.Data;
        }

        public Task SubscribeToUpdates()
        {
	        var graphQlRequest = new GraphQLRequest("subscription { reviewAdded { title productId } }");

            var result = _client.CreateSubscriptionStream<ReviewAddedMessage>(graphQlRequest);
            result.Subscribe(Receive);
            return Task.CompletedTask;
            //result.FirstOrDefaultAsync().Subscribe(Receive);
        }

        private void Receive(GraphQLResponse<ReviewAddedMessage> resp)
        {
            var review = resp.Data;
        }
    }
}
