using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace InfinitioPCL
{
public static class CommonService
	{
		public const string BaseUrl = "https://infinitio.azurewebsites.net/";
		public const string ikey = "cnl0RZjW4lBCoCRDxRUC4VueaX9ma1rdeD3WsdInoaPCDx60eRt9CNdDBdPB";
		public static async Task<RootNode> HttpGetOperation(string url)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(BaseUrl);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Add("ikey", ikey);
				var response = await client.GetAsync(url);
				if (response.IsSuccessStatusCode)
				{
					var json = await response.Content.ReadAsStringAsync();
					var result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<RootNode>(json));
					return result;
				}
				else
				{
					var status = new IStatus();
					status.Status = "NoInternet";
					status.StatusCode = response.StatusCode.ToString();
					var rootnode = new RootNode();
					rootnode.status = status;
					rootnode.data = null;
					return rootnode;
				}

			}


		}
		public static async Task<RootNode> HttpPostOperation(string url, MultipartFormDataContent content)
		{
			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					client.BaseAddress = new Uri(BaseUrl);
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					client.DefaultRequestHeaders.Add("ikey", ikey);
					var response = await client.PostAsync(url, content);
					var temp = await response.Content.ReadAsStringAsync();
					if (response.IsSuccessStatusCode)
					{
						var json = await response.Content.ReadAsStringAsync();
						var result = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<RootNode>(json));
						return result;
					}
					else
					{
						var status = new IStatus();
						status.Status = "NoInternet";
						status.StatusCode = response.StatusCode.ToString();
						var rootnode = new RootNode();
						rootnode.status = status;
						rootnode.data = null;
						return rootnode;
					}
				}
			}
			catch (Exception e)
			{
				var status = new IStatus();
				status.Status = "NoInternet";
				status.StatusCode = "NoInternet";
				var rootnode = new RootNode();
				rootnode.status = status;
				rootnode.data = null;
				return rootnode;
			}

		}
	}
}
