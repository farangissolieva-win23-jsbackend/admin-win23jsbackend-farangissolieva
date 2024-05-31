
using System.Text.Json;

namespace adminWebApp.Services
{
	public class GraphQLService(HttpClient httpClient)
	{
		private readonly HttpClient _httpClient = httpClient;

		public async Task<T> ExecuteQuery<T>(string query, object variables = null!)
		{
			var request = new GraphQLRequest
			{
				Query = query,
				Variables = variables
			};

			var response = await _httpClient.PostAsJsonAsync("https://courseprovidersilicon.azurewebsites.net/api/GraphQL", request);
			var responseContent = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"Error executing query: {responseContent}");
			}

			var result = JsonSerializer.Deserialize<GraphQLResponse<T>>(responseContent, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			return result.Data;
		}




	}

	public class GraphQLRequest
	{
		public string Query { get; set; }
		public object Variables { get; set; }
	}

	public class GraphQLResponse<T>
	{
		public T Data { get; set; }
	}
}
	
