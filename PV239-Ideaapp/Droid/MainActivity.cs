﻿using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Microsoft.WindowsAzure.MobileServices;
using PV239_IdeaApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace PV239_IdeaApp.Droid
{
	[Activity (
	    Label = "PV239_IdeaApp.Droid",
		Icon = "@drawable/icon",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : FormsApplicationActivity, IAuthenticate
    {
        // Define a authenticated user.

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

		    // Initialize the authenticator before loading the app.
		    App.Init((IAuthenticate)this);

            // Initialize Azure Mobile Apps
            CurrentPlatform.Init();

			// Initialize Xamarin Forms
			Forms.Init (this, bundle);

            // Load the main application
            LoadApplication (new App ());
		}

        public async Task<bool> Authenticate()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                IdeaManager.User = await IdeaManager.DefaultManager.CurrentClient.LoginAsync(this,
                    MobileServiceAuthenticationProvider.Facebook, "pv239-ideaapp");
                if (IdeaManager.User != null)
                {
                    message = $"You are now successfully signed-in";
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }
    }
}

