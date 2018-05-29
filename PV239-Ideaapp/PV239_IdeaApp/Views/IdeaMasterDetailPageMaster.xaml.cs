    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PV239_IdeaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IdeaMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        private readonly IdeaManager _manager;
        private bool _IsAuthenticated = false;
        public readonly string NonFavIconPath = "NonFavorite.png";
        public readonly string FavIconPath = "Favorite.png";

        public IdeaMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new IdeaMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;

            _manager = IdeaManager.DefaultManager;

            // OnPlatform<T> doesn't currently support the "Windows" target platform, so we have this check here.
            if (_manager.IsOfflineEnabled && Device.OS == TargetPlatform.Windows)
            {
                var syncButton = new Button
                {
                    Text = "Sync items",
                    HeightRequest = 30
                };
                syncButton.Clicked += OnSyncItems;

                buttonsPanel.Children.Add(syncButton);
            }
        }

        async Task AddItem(Ideas item)
        {
            await _manager.SaveTaskAsync(item);
            MenuItemsListView.ItemsSource = await _manager.GetTodoItemsAsync();
        }

        async Task MakeFavoriteItem(Ideas item)
        {
            item.IsFavorite = true;
            await _manager.SaveTaskAsync(item);
            MenuItemsListView.ItemsSource = await _manager.GetTodoItemsAsync();
        }

        //public async void OnAdd(object sender, EventArgs e)
        //{
        //    var todo = new Ideas { Name = newItemName.Text };
        //    await AddItem(todo);

        //    newItemName.Text = string.Empty;
        //    newItemName.Unfocus();
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (_IsAuthenticated == true)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                await RefreshItems(true, syncItems: false);

                // Hide the Sign-in button.
                this.LoginButton.IsVisible = false;
            }
        }

        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true, true);
        }

        public async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                MenuItemsListView.ItemsSource = await _manager.GetTodoItemsAsync(syncItems);
            }
        }

        private async void LoginButton_OnClicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                _IsAuthenticated = await App.Authenticator.Authenticate();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (_IsAuthenticated == true)
                await RefreshItems(true, syncItems: false);
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            var addPage = new AddPage();
            ((IdeaMasterDetailPage)App.Current.MainPage).NavigateTo(addPage);
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private readonly bool _showIndicator;
            private readonly ActivityIndicator _indicator;
            private readonly Task _indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this._indicator = indicator;
                this._showIndicator = showIndicator;

                if (showIndicator)
                {
                    _indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    _indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this._indicator.IsVisible = isActive;
                this._indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (_showIndicator)
                {
                    _indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        public class IdeaMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<Ideas> MenuItems { get; set; }
            public ICommand IdeaPreferenceCommand { get; private set; }

            private readonly IdeaManager _manager;

            public IdeaMasterDetailPageMasterViewModel()
            {
                _manager = IdeaManager.DefaultManager;

                GetMenuItems();
                IdeaPreferenceCommand = new Command<Ideas>(ChangePreference);
            }

            public async Task GetMenuItems()
            {
                MenuItems = await _manager.GetTodoItemsAsync();
            }

            public void ChangePreference(Ideas idea)
            {
                idea.IsFavorite = !idea.IsFavorite;
                _manager.SaveTaskAsync(idea);
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