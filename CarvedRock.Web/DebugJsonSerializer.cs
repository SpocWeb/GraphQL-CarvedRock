using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions.Websocket;

namespace CarvedRock.Web;

public class DebugJsonSerializer : IGraphQLWebsocketJsonSerializer
{
	readonly IGraphQLWebsocketJsonSerializer _WebsocketJsonSerializer;

	public DebugJsonSerializer(IGraphQLWebsocketJsonSerializer websocketJsonSerializer)
	{
		_WebsocketJsonSerializer = websocketJsonSerializer;
	}

	public string SerializeToString(GraphQLRequest request)
	{
		return _WebsocketJsonSerializer.SerializeToString(request);
	}

	public async Task<GraphQLResponse<TResponse>> DeserializeFromUtf8StreamAsync<TResponse>(Stream stream,
		CancellationToken cancellationToken)
	{
		var str = await new StreamReader(stream).ReadToEndAsync();
		try
		{
			stream.Position = 0;
		}
		catch
		{
			stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
		}

		var ret = await _WebsocketJsonSerializer
			.DeserializeFromUtf8StreamAsync<TResponse>(stream, cancellationToken);
		return ret;
	}

	public byte[] SerializeToBytes(GraphQLWebSocketRequest request)
	{
		return _WebsocketJsonSerializer.SerializeToBytes(request);
	}

	public Task<WebsocketMessageWrapper> DeserializeToWebsocketResponseWrapperAsync(Stream stream)
	{
		return _WebsocketJsonSerializer.DeserializeToWebsocketResponseWrapperAsync(stream);
	}

	public GraphQLWebSocketResponse<GraphQLResponse<TResponse>> DeserializeToWebsocketResponse<TResponse>(byte[] bytes)
	{
		return _WebsocketJsonSerializer.DeserializeToWebsocketResponse<TResponse>(bytes);
	}
}