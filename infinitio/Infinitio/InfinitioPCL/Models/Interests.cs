using System;
using System.Collections.Generic;

namespace InfinitioPCL
{
	public class SectionData
	{
		public string sports { get; set; }
		public string business { get; set; }
		public string politics { get; set; }
		public string tech { get; set; }
		public string entertainment { get; set; }
	}

	public class News
	{
		public string id { get; set; }
		public string title { get; set; }
		public string shortarticle { get; set; }
		public string image { get; set; }
		public string article { get; set; }
	}

	public class NewsData
	{
		public List<News> news { get; set; }
	}


}
