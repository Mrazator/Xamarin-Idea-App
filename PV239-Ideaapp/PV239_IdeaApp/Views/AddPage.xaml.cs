using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private async Task AddButton_Clicked(object sender, EventArgs e)
        {
            var idea = new Ideas();

            idea.Name = newIdeaName.Text;
            idea.Description = newIdeaDescription.Text;
            idea.IsFavorite = false;

            await AddIdea(idea);

            ((IdeaMasterDetailPage)App.Current.MainPage).NavigateHome();

        }

        private async Task AddIdea(Ideas idea)
        {
            await _manager.SaveTaskAsync(idea);
        }
    }
}