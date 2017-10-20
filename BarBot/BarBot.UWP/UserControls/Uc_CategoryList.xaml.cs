using BarBot.Core;
using BarBot.Core.Model;
using BarBot.Core.WebSocket;
using BarBot.UWP.Websocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls
{
    public sealed partial class Uc_CategoryList : UserControl, INotifyPropertyChanged
    {
        private UWPWebSocketService webSocketService;

        private List<Category> categories;

        public List<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                List<Uc_CategoryTile> tiles = new List<Uc_CategoryTile>();
                for (int i = 0; i < Math.Min(8, categories.Count); i++)
                {
                    Uc_CategoryTile tile = new Uc_CategoryTile();
                    tile.SetValue(Grid.ColumnProperty, i % 4);
                    tile.SetValue(Grid.RowProperty, i / 4);
                    tile.Category = categories[i];
                    tiles.Add(tile);
                }
                icMenuItems.ItemsSource = tiles;
                OnPropertyChanged("Categories");
            }
        }

        public Uc_CategoryList()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
