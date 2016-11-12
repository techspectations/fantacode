using System;
namespace InfinitioPCL
{
	public class Data
	{
		public bool is_silhouette { get; set; }
		public string url { get; set; }
	}

	public class Picture
	{
		public Data data { get; set; }
	}

	public class FacebookResult
	{
		public string id { get; set; }
		public string name { get; set; }
		public Picture picture { get; set; }
	}
}
