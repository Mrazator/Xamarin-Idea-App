using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace IdeaApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async Task Insert()
        {
            var userTable = App.Client.GetTable<Users>();
            var itemTable = App.Client.GetTable<Ideas>();

            var user = new Users { NickName = "UserName3", Email = "Email12" };
            await userTable.InsertAsync(user);

            var query = userTable.Where(x => x.NickName == "UserName3");
            var id = (await userTable.ReadAsync(query)).FirstOrDefault()?.Id;

            var item = new Ideas { IsFavorite = false, Description = "Description", Name = "Name", UserId = id};
            await itemTable.InsertAsync(item);
        }

        public async void AddItemButton_OnClicked(object sender, EventArgs e)
        {
            await Insert();
        }
    }
}
