
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Calligraphy;

using BarBot.Core.Service.WebSocket;
using BarBot.Core.ViewModel;
using BarBot.Core.WebSocket;
using BarBot.Droid.Utils;

namespace BarBot.Droid.View.Container
{
    [Activity(Label = "ContainerActivity")]
    public class ContainerActivity : BaseActivity
    {
        WebSocketService WebSocketService => App.WebSocketService;
        ContainersViewModel ViewModel => App.Locator.Containers;

        TextView TitleTextView;
        ListView ListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Prevent Rotation
            RequestedOrientation = Android.Content.PM.ScreenOrientation.Nosensor;

            SetContentView(Resource.Layout.ContainersLayout);

            ConfigureAppBar();
            ConfigureListView();

            WebSocketService.Socket.GetContainersEvent += Socket_GetContainersEvent;
            WebSocketService.GetContainers();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                // Respond to the action bar's Up/Home button
                case Android.Resource.Id.Home:
                    ViewModel.ShowMenuCommand();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void AttachBaseContext(Context @base)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(@base));
        }

        void ConfigureAppBar()
        {
            var appBar = (Toolbar)FindViewById(Resource.Id.toolbar); // Attaching the layout to the toolbar object
            SetActionBar(appBar);

            TitleTextView = (TextView)appBar.FindViewById(Resource.Id.toolbar_textview);
            TitleTextView.Text = Helpers.UppercaseWords(ViewModel.Title);

            ActionBar.SetDisplayShowTitleEnabled(false);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        void ConfigureListView()
        {
            ListView = (ListView)FindViewById(Resource.Id.container_listview);
            ListView.Adapter = new ContainerAdapter(this, ViewModel.Containers);
        }

        private async void Socket_GetContainersEvent(object sender, WebSocketEvents.GetContainersEventArgs args)
        {
            await Task.Run(() => RunOnUiThread(() =>
            {
                ViewModel.Containers = args.Containers;
                ViewModel.Containers.Sort((x, y) => x.Number.CompareTo(y.Number));
                (ListView.Adapter as ContainerAdapter).NotifyDataSetChanged();
            }));

            WebSocketService.Socket.GetContainersEvent -= Socket_GetContainersEvent;
        }
    }
}
