using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using IdeaApp.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
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
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        public static ICallbackManager CallbackManager;
        private MobileServiceUser user;


        protected override void OnCreate(Bundle bundle)
        {
            FacebookSdk.SdkInitialize(this.ApplicationContext);

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

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Facebook, "{url_scheme_of_your_app}");
                if (user != null)
                {
                    message = string.Format("you are now signed-in as {0}.",
                        user.UserId);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }
    }
}

