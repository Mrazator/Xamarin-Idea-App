using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PV239_IdeaApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditPage : ContentPage
	{
        public Ideas Idea { get; private set; }

        private readonly IdeaManager _manager;

        public EditPage(Ideas idea)
		{
            InitializeComponent();

            _manager = IdeaManager.DefaultManager;

            Idea = idea;
            BindingContext = new EditPageViewModel(idea);
		}

        private async Task ConfirmButton_Clicked(object sender, EventArgs e)
        {
            Idea.Name = newIdeaName.Text;
            Idea.Description = newIdeaDescription.Text;

            await UpdateIdea(Idea);

            ((IdeaMasterDetailPage) App.Current.MainPage).NavigateHome();
        }

        private async Task UpdateIdea(Ideas idea)
        {
            await _manager.SaveTaskAsync(idea);
        }

        class EditPageViewModel : INotifyPropertyChanged
        {
            public Ideas Idea { get; set; }

            public EditPageViewModel(Ideas idea)
            {
                Idea = idea;
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}