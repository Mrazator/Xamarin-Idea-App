using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using PV239_IdeaApp.ViewModel;
using Xamarin.Forms;

namespace PV239_IdeaApp.Views
{
    public partial class IdeaList : ContentPage
    {
        private readonly IdeaManager _manager;

        // Track whether the user has authenticated.

        public IdeaList()
        {
            InitializeComponent();

            _manager = IdeaManager.DefaultManager;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (App.IsAuthenticated == true)
            {
                await RefreshItems(true, syncItems: false);
            }
        }

        // Data methods

        async Task DeleteItem(Ideas item)
        {
            await _manager.DeleteIdeaAsync(item);
            Ideas.ItemsSource = await _manager.GetTodoItemsAsync();
        }

        async Task MakeFavoriteItem(Ideas item)
        {
            item.IsFavorite = !item.IsFavorite;
            await _manager.UpdateIdeaAsync(item);
            Ideas.ItemsSource = await _manager.GetTodoItemsAsync();
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddPage());
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            await DeleteItem(((MenuItem) sender).CommandParameter as Ideas);
        }

        // Event handlers
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (Device.OS != TargetPlatform.iOS && e.SelectedItem is Ideas todo)
            {
                if (Device.OS == TargetPlatform.Android)
                {
                    await DisplayAlert(todo.Name, "Press-and-hold to interact " + todo.Name, "Got it!");
                }
            }

            Ideas.SelectedItem = null;
        }

        public async void OnEdit(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditPage(((MenuItem) sender).CommandParameter as Ideas));
        }

        public async void OnMakeFavorite(object sender, EventArgs e)
        {
            await MakeFavoriteItem(((MenuItem)sender).CommandParameter as Ideas);
        }

        public void OnViewDetail(object sender, EventArgs e)
        {
            var idea = (Ideas) Ideas.SelectedItem;
            NavigationPage detailPage = new NavigationPage(new ContentPage());
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
                Ideas.ItemsSource = await _manager.GetTodoItemsAsync(syncItems);
            }
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
    }
}

