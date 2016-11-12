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
using InfinitioPCL.Helpers;

namespace Infinitio
{
    [Activity(Label = "InterestSelectActivity")]
    public class InterestSelectActivity : Activity
    {
		SeekBar SportsProgress, BusinessProgress, PoliticsProgress, TechnologyProgress, EntertainmentProgress;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.InterestSelectLayout);
			TextView txt = FindViewById<TextView>(Resource.Id.CatName);
			Button CatSelBtn = FindViewById<Button>(Resource.Id.CatSelBtn);
			TextView UserName = FindViewById<TextView>(Resource.Id.UserName);

			SportsProgress = FindViewById<SeekBar>(Resource.Id.SportsProgress);
			BusinessProgress = FindViewById<SeekBar>(Resource.Id.BusinessProgress);
			PoliticsProgress = FindViewById<SeekBar>(Resource.Id.PoliticsProgress);
			TechnologyProgress = FindViewById<SeekBar>(Resource.Id.TechnologyProgress);
			EntertainmentProgress = FindViewById<SeekBar>(Resource.Id.EntertainmentProgress);

			if (String.IsNullOrEmpty(Settings.Name))
				UserName.Text = Settings.Name;

			CatSelBtn.Click+=CatSelBtn_Click;
			LoadData();
		}
		public async void LoadData()
		{
			string userid;
			if (String.IsNullOrEmpty(Settings.UserID))
				userid = "6";
			else
				userid = Settings.UserID;
			var res = await DataServices.GetInterests("6");
			var result = res.data as SectionData;


		}
		void CatSelBtn_Click(object sender, EventArgs e)
		{
			StartActivity(typeof(ChatActivity));
		}
	}
}