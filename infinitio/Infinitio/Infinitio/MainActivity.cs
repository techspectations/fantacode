using Android.App;
using Android.Widget;
using Android.OS;
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
using Android.Util;
using Android.Views.Animations;
using Android.Graphics;
using System.Threading;

namespace Infinitio
{
	[Activity(Label = "Infinitio", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.Splash")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView (Resource.Layout.Splash);

			Thread.Sleep(2000);
			StartActivity(typeof(LoginActivity));
            
        }

    }
}

