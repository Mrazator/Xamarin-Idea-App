using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV239_IdeaApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PV239_IdeaApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPage : ContentPage
	{
        private readonly IdeaManager _manager;

        public AddPage()
        {
            InitializeComponent();

            _manager = IdeaManager.DefaultManager;            
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            var idea = new Ideas
            {
                Name = newIdeaName.Text,
                Description = newIdeaDescription.Text,
                UserId = IdeaManager.User.UserId
            };

            await _manager.SaveTaskAsync(idea);
            await Navigation.PopModalAsync();
        }
    }
}