using CarvedRock.Api.Data;
using GraphQL.Types;

namespace CarvedRock.Api.GraphQL.Types
{
    /// <summary> Need to define a <see cref="EnumerationGraphType{TEnum}"/> Subtype to use Enums in GraphQL Queries </summary>
    public class ProductTypeEnumType: EnumerationGraphType<ProductTypeEnum> { }
}
