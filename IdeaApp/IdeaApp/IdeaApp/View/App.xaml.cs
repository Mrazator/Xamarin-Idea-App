﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdeaApp.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace IdeaApp
{
	public partial class App : Application
	{
	    public static MobileServiceClient Client;
	    public static IAuthenticate Authenticator { get; private set; }

        static App()
	    {
            Client = new MobileServiceClient("https://pv239-ideaapp.azurewebsites.net");
        }

        public App ()
		{
            InitializeComponent();

			MainPage = new LoginPage();
		}
	    public static void Init(IAuthenticate authenticator)
	    {
	        Authenticator = authenticator;
	    }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
