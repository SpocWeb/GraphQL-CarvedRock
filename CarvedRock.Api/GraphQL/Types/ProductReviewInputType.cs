using GraphQL.Types;

namespace CarvedRock.Api.GraphQL.Types
{
    /// <summary> For Mutations, need to define separate Types <see cref="ProductReviewType"/> </summary>
    public class ProductReviewInputType: InputObjectGraphType
    {
        public ProductReviewInputType()
        {
            Name = "reviewInput";
            Field<IdGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("title");
            Field<StringGraphType>("review");
            Field<IntGraphType>("productId");
        }

    }
}
