using System;
using Newtonsoft.Json;

namespace InfinitioPCL
{
	public class RootNode
	{
		[JsonProperty("data")]
		public object data { get; set; }
		[JsonProperty("ZemoseStatus")]
		public IStatus status { get; set; }
	}

	public class IStatus
	{
		public string StatusCode { get; set; }
		public string Status { get; set; }
	}
}
