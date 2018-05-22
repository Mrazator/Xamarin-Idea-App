using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Facebook;

namespace IdeaApp.Droid
{
    [Activity(
        Label = "IdeaApp",
        Icon = "@drawable/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        Name = "com.companyname.IdeaApp.MainActivity",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation),
    ]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static ICallbackManager CallbackManager;

        protected override void OnCreate(Bundle bundle)
        {
            FacebookSdk.SdkInitialize(this);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CallbackManager = CallbackManagerFactory.Create();

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        protected override void OnActivityResult(
            int requestCode,
            Result resultCode,
            Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            CallbackManager.OnActivityResult(requestCode, Convert.ToInt32(resultCode),
                data);
        }
    }
}

