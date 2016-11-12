using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using InfinitioPCL;

namespace Infinitio
{
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : Activity
    {
		//List<NewsData> NewsList;
		Button ChatSubmit;
		List<string> items;
		EditText ChatTxt;
		ListView ChatList;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Chat);

			ChatList = FindViewById<ListView>(Resource.Id.ChatList);
			ChatSubmit = FindViewById<Button>(Resource.Id.ChatSubmit);
			ChatTxt = FindViewById<EditText>(Resource.Id.ChatTxt);

			items = new List<string>();
			ChatSubmit.Click+=ChatSubmit_Click;
        }

		async void ChatSubmit_Click(object sender, EventArgs e)
		{
			
			items.Add(ChatTxt.Text);
			ChatByAdapter adapter = new ChatByAdapter(this, items, "me", null,null);
			ChatList.Adapter = adapter;
			ChatList.NotifyAll();
			LoadReply(ChatTxt.Text);
			ChatTxt.Text = "";
		}

		async void LoadReply(string msg)
		{
			var res = await DataServices.GetResponse("6", msg);
			if (res.data != null)
			{
				if (res.type == "p")
				{
					var addc = (string)res.data;
					ChatByAdapter adapter = new ChatByAdapter(this, items, "they", null, addc);
					ChatList.Adapter = adapter;
					ChatList.NotifyAll();
				}
				else
				{
					var addc = (ArticleData)res.data;
					ChatByAdapter adapter = new ChatByAdapter(this, items, "news", addc.articles, null);
					ChatList.Adapter = adapter;
					ChatList.NotifyAll();
				}
			}

		}
	}
}