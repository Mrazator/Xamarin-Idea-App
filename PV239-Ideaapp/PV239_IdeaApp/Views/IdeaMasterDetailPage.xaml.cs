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
    public partial class IdeaMasterDetailPage : MasterDetailPage
    {
        public IdeaMasterDetailPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            IsPresented = true;
        }

        public void NavigateTo(ContentPage page)
        {
            Detail = new NavigationPage(page);
            IsPresented = false;
        }

        public async Task NavigateHome()
        {
            IsPresented = true;
            var masterPage = (IdeaMasterDetailPageMaster) Master;
            await masterPage.RefreshItems(true, true);
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Ideas;
            if (item == null)
                return;

            //var page = (Page)Activator.CreateInstance(item.TargetType);
            var page = new IdeaMasterDetailPageDetail(item);
            page.Title = item.Name;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}