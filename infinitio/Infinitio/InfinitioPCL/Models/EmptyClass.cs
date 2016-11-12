using System;
using System.Collections.Generic;

namespace InfinitioPCL
{
	public class Article
	{
		public string articleID { get; set; }
		public string title { get; set; }
		public string articleURL { get; set; }
		public string thumbnail { get; set; }
		public string imgWeb { get; set; }
		public string imgMob { get; set; }
		public string lastModified { get; set; }
		public string otherImages { get; set; }
		public string video { get; set; }
	}

	public class ArticleData
	{
		public List<Article> articles { get; set; }
	}

	public class RootObject
	{
		public IStatus IStatus { get; set; }
		public object data { get; set; }
		public string type { get; set;}
	}
}
