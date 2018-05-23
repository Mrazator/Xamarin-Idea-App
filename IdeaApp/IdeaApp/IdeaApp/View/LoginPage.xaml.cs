using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IdeaApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private bool authenticated = false;

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (authenticated == true)
                //await RefreshItems(true, syncItems: false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                //await RefreshItems(true, syncItems: false);

                // Hide the Sign-in button.
                this.LoginButton.IsVisible = false;
            }
        }
    }
}