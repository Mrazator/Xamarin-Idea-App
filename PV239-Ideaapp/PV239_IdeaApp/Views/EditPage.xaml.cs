using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PV239_IdeaApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PV239_IdeaApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPage : ContentPage
	{
        private readonly IdeaManager _manager;
	    private readonly Ideas _idea;

        public EditPage(Ideas idea)
		{
            InitializeComponent();

            _manager = IdeaManager.DefaultManager;

		    _idea = idea;
		    newIdeaName.Text = idea.Name;
		    newIdeaDescription.Text = idea.Description;
            BindingContext = new EditPageViewModel(idea);
		}

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            _idea.Name = newIdeaName.Text;
            _idea.Description = newIdeaDescription.Text;

            await _manager.UpdateIdeaAsync(_idea);
            await Navigation.PopModalAsync();
        }


        class EditPageViewModel : INotifyPropertyChanged
        {
            private Ideas Idea { get; set; }

            public EditPageViewModel(Ideas idea)
            {
                Idea = idea;
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}