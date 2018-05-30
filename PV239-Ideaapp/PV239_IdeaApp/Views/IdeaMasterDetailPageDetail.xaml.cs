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
    public partial class IdeaMasterDetailPageDetail : ContentPage
    {
        public Ideas Idea { get; set; }
        private readonly IdeaManager _manager;

        public IdeaMasterDetailPageDetail()
        {

        }

        public IdeaMasterDetailPageDetail(Ideas idea)
        {
            InitializeComponent();

            _manager = IdeaManager.DefaultManager;
            Idea = idea;
            BindingContext = new IdeaMasterDetailPageDetailViewModel(idea);
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            var editPage = new EditPage(Idea);
            ((IdeaMasterDetailPage)App.Current.MainPage).NavigateTo(editPage);
        }

        private async Task DeleteButton_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete", "Do you wan't to delete this idea?", "Yes", "No");
            if (answer)
            {
                await _manager.DeleteIdeaAsync(Idea);
                await ((IdeaMasterDetailPage)App.Current.MainPage).NavigateHome();
            }
            
        }
    }

    class IdeaMasterDetailPageDetailViewModel : INotifyPropertyChanged
    {
        public Ideas Idea { get; set; }

        public IdeaMasterDetailPageDetailViewModel(Ideas idea)
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