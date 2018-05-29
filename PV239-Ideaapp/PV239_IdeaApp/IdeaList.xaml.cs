using PV239_IdeaApp.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PV239_IdeaApp
{
    public partial class IdeaList : ContentPage
    {
        private readonly IdeaManager _manager;
        // Track whether the user has authenticated.
        bool _authenticated = false;

        public IdeaList()
        {
            InitializeComponent();

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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (_authenticated == true)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                await RefreshItems(true, syncItems: false);

                // Hide the Sign-in button.
                this.LoginButton.IsVisible = false;
            }
        }

        // Data methods
        async Task AddItem(Ideas item)
        {
            await _manager.SaveTaskAsync(item);
            todoList.ItemsSource = await _manager.GetTodoItemsAsync();
        }

        async Task MakeFavoriteItem(Ideas item)
        {
            item.IsFavorite = true;
            await _manager.SaveTaskAsync(item);
            todoList.ItemsSource = await _manager.GetTodoItemsAsync();
        }

        public async void OnAdd(object sender, EventArgs e)
        {
            var todo = new Ideas { Name = newItemName.Text };
            await AddItem(todo);

            newItemName.Text = string.Empty;
            newItemName.Unfocus();
        }

        // Event handlers
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //if (Device.OS != TargetPlatform.iOS && e.SelectedItem is Ideas todo)
            //{
            //    // Not iOS - the swipe-to-delete is discoverable there
            //    if (Device.OS == TargetPlatform.Android)
            //    {
            //        await DisplayAlert(todo.Name, "Press-and-hold to make favorite this task " + todo.Name, "Got it!");
            //    }
            //    else
            //    {
            //        // Windows, not all platforms support the Context Actions yet
            //        if (await DisplayAlert("Mark favorite?", "Do you wish to complete " + todo.Name + "?", "Complete", "Cancel"))
            //        {
            //            await MakeFavoriteItem(todo);
            //        }
            //    }
            //}
            if (e.SelectedItem != null)
            {
                var idea = (Ideas)e.SelectedItem;
                NavigationPage detailPage = new NavigationPage(new ContentPage());
                //await Navigation.PushAsync(detailPage);
                
            }
            todoList.SelectedItem = null;
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
        public async void OnMakeFavorite(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var todo = mi.CommandParameter as Ideas;
            await MakeFavoriteItem(todo);
        }

        public void OnViewDetail(object sender, EventArgs e)
        {
            var idea = (Ideas) todoList.SelectedItem;
            NavigationPage detailPage = new NavigationPage(new ContentPage());
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
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

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                todoList.ItemsSource = await _manager.GetTodoItemsAsync(syncItems);
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

        private async void LoginButton_OnClicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                _authenticated = await App.Authenticator.Authenticate();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            if (_authenticated == true)
                await RefreshItems(true, syncItems: false);
        }
    }
}

