
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using InfinitioPCL;

namespace Infinitio
{
	public class ChatByAdapter : BaseAdapter<string>
	{
		private List<string> mItems;
		private Context mContext;
		private List<string> temp;
		string type, reply;
		List<Article> News;

		public ChatByAdapter(Context context, List<string> items, string type, List<Article> news, string reply)
		{
			mItems = items;
			mContext = context;
			this.type = type;
			this.News = news;
			this.reply = reply;
		}
		public override int Count
		{
			get
			{
				return mItems.Count;
			}
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override string this[int position]
		{
			get
			{
				return mItems[position];
			}
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = convertView;
			if (type == "me")
			{
				if (row == null)
				{
					row = LayoutInflater.From(mContext).Inflate(Resource.Layout.ChatOneLayout, null, false);
				}
				TextView lblMsgFrom = row.FindViewById<TextView>(Resource.Id.lblMsgFrom);
				TextView txtMsg = row.FindViewById<TextView>(Resource.Id.txtMsg);


				txtMsg.Text = mItems[position];
			}
			else if (type == "they")
			{
				if (row == null)
				{
					row = LayoutInflater.From(mContext).Inflate(Resource.Layout.ChatOneLayoutLeft, null, false);
				}
				TextView lblMsgFrom = row.FindViewById<TextView>(Resource.Id.lblMsgFrom);
				TextView txtMsg = row.FindViewById<TextView>(Resource.Id.txtMsg);

				lblMsgFrom.Text = "Bot";
				txtMsg.Text = reply;
			}
			else
			{
				if (row == null)
				{
					row = LayoutInflater.From(mContext).Inflate(Resource.Layout.ChatOneLayoutLeftPosts, null, false);
				}
				TextView PostName = row.FindViewById<TextView>(Resource.Id.PostName);
				TextView PostContent = row.FindViewById<TextView>(Resource.Id.PostContent);

				PostName.Text = News[0].title;
				PostContent.Text = News[0].articleURL;
				        
			}
			return row;
		}
	}
}