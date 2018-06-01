using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PV239_IdeaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_OnClicked(object sender, EventArgs args)
        {
            if (App.Authenticator != null)
            {
                App.IsAuthenticated = await App.Authenticator.Authenticate();

                if (App.IsAuthenticated)
                    await Navigation.PushModalAsync(new IdeaList());
            }
        }

    }
}
