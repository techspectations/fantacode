using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InfinitioPCL
{
	public class DataServices
	{
		public const string BaseUrl = "https://infinitio.azurewebsites.net/";
		public async static Task<RootNode> Register(string id_token)
		{
			MultipartFormDataContent content = new MultipartFormDataContent();
			content.Add(new StringContent(id_token), "token");
			var result = await CommonService.HttpPostOperation("v1/Users/FbLogin", content);
			if (result.data != null)
			{
				result.data = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<UserData>(result.data.ToString()));
			}
			return result;
		}

		public async static Task<RootNode> GetInterests(string userId)
		{
			MultipartFormDataContent content = new MultipartFormDataContent();
			content.Add(new StringContent(userId), "userId");
			var result = await CommonService.HttpPostOperation("v1/Interest/GetInterest", content);
			if (result.data != null)
			{
				result.data = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<SectionData>(result.data.ToString()));
			}

			return result;
		}

		public async static Task<RootNode> PostInterests(string userId, string sports, string business, string politics, string tech, string entertainment)
		{
			MultipartFormDataContent content = new MultipartFormDataContent();
			content.Add(new StringContent(userId), "userId");
			content.Add(new StringContent(sports), "sports");
			content.Add(new StringContent(business), "business");
			content.Add(new StringContent(politics), "politics");
			content.Add(new StringContent(tech), "tech");
			content.Add(new StringContent(entertainment), "entertainment");
			var result = await CommonService.HttpPostOperation("v1/Interest/PostInterest", content);
			return result;
		}

		public async static Task<RootObject> GetResponse(string userId, string term)
		{
			MultipartFormDataContent content = new MultipartFormDataContent();
			content.Add(new StringContent(userId), "userId");
			content.Add(new StringContent(term), "term");
			var result = await CommonService.HttpPostOperation("v1/Users/search", content);
			if (result.data != null)
			{
				result.data = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<RootNode>(result.data.ToString()));
			}

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(BaseUrl);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await client.PostAsync("v1/Users/search", content);
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var res = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<RootObject>(json));
					return res;
				}
				else
				{
					return null;
				}
			}
		}
	}
}