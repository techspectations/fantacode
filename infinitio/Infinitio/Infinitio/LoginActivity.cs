using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using InfinitioPCL;
using InfinitioPCL.Helpers;
using Newtonsoft.Json;
using Org.Json;
using Xamarin.Facebook;
using Xamarin.Facebook.AppEvents;
using Xamarin.Facebook.Login;

namespace Infinitio
{
	[Activity(Label = "LoginActivity")]
	public class LoginActivity : AppCompatActivity, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
	{
		#region Facebook 
		Button fbloginButton;

		ProgressDialog dialog;
		LinearLayout Whiter;
		RelativeLayout MainLinear;
		AccessToken accessToken;
		ProgressBar progressBar;


		EditText countrycode;

		private ICallbackManager mCallBackManager;
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		void CreateProgressBar()
		{
			dialog = new ProgressDialog(this);
			dialog.SetMessage(GetString(Resource.String.loading));
			dialog.SetProgressStyle(ProgressDialogStyle.Spinner);
			dialog.Indeterminate = true;
		}

		public async void OnCompleted(JSONObject p0, GraphResponse p1)
		{

			string data = p0.ToString();
			FacebookResult result = JsonConvert.DeserializeObject<FacebookResult>(data);
			if (accessToken != null)
			{
				dialog.Show();
				var res = await DataServices.Register(accessToken.Token);
				dialog.Dismiss();
				if (res != null)
				{
					Settings.UserID = result.id;
					Settings.Name = result.name;
					//	ProcessAccountData(res);
					StartActivity(typeof(InterestSelectActivity));
				}
			}

		}
		public void OnCancel()
		{
			Snackbar.Make(fbloginButton, Resource.String.actioncancelled, Snackbar.LengthShort).Show();
		}

		public void OnError(FacebookException p0)
		{
			Snackbar.Make(fbloginButton, Resource.String.cantconnectfb, Snackbar.LengthShort).Show();
		}

		public void OnSuccess(Java.Lang.Object p0)
		{
			LoginResult loginResult = p0 as LoginResult;
			accessToken = loginResult.AccessToken;
			var request = GraphRequest.NewMeRequest(loginResult.AccessToken, this);
			Bundle parameters = new Bundle();
			parameters.PutString("fields", "id,name,picture,email");
			request.Parameters = parameters;
			request.ExecuteAsync();
		}

		#endregion


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);


			// Create your application here

			#region Fb Implementation
			FacebookSdk.SdkInitialize(ApplicationContext);
			AppEventsLogger.ActivateApp(Application);

			fbloginButton = (Button)FindViewById(Resource.Id.LoginBtn);
			progressBar = FindViewById<ProgressBar>(Resource.Id.progressbar);

			/*UpBger = (LinearLayout)FindViewById(Resource.Id.UpBger);
			Whiter = (LinearLayout)FindViewById(Resource.Id.Whiter);
			MainLinear = (RelativeLayout)FindViewById(Resource.Id.MainLinear);
			animated_android = (ImageView)FindViewById(Resource.Id.LoadImg);*/

			/*AnimationDrawable animation = (AnimationDrawable)animated_android.Drawable;
			animation.Start();*/

			mCallBackManager = CallbackManagerFactory.Create();
			LoginManager.Instance.RegisterCallback(mCallBackManager, this);
			fbloginButton.Click += delegate
			 {
				 //AnimateIris(UpBger);
				 //AnimateIris2(Whiter, MainLinear);
				 StartActivity(typeof(InterestSelectActivity));
				 var list = new List<string>();
				 list.Add("public_profile");
				 list.Add("email");
				 list.Add("user_birthday");
				 list.Add("user_likes");
					
				 //LoginManager.Instance.LogInWithReadPermissions(this, list);

			 };
			#endregion

		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			mCallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
		}
		public void AnimateIris(LinearLayout iris)
		{
			Rect corners = new Rect();
			bool i = iris.GetLocalVisibleRect(corners);
			Animation anim = new TranslateAnimation(10, 10, corners.Top, -2550);
			anim.Duration = 2000;
			iris.StartAnimation(anim);
		}
		public void AnimateIris2(LinearLayout iris, RelativeLayout iris2)
		{
			Rect corners = new Rect();
			bool i = iris.GetLocalVisibleRect(corners);
			Rect corners2 = new Rect();
			bool j = iris2.GetLocalVisibleRect(corners2);
			Animation anim = new TranslateAnimation(10, 10, corners.Top, corners2.Top);
			anim.Duration = 2000;
			iris.Visibility = ViewStates.Visible;
			iris.StartAnimation(anim);
		}
	}

}
