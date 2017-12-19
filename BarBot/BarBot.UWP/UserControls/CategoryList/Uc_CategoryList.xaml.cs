using BarBot.Core.Model;
using BarBot.UWP.Websocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UWP.UserControls.CategoryList
{
    public sealed partial class Uc_CategoryList : UserControl, INotifyPropertyChanged
    {
        private List<Category> categories;

        public List<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                List<Uc_CategoryTileRect> tiles = new List<Uc_CategoryTileRect>();
                for (int i = 0; i < Math.Min(8, categories.Count); i++)
                {
                    Uc_CategoryTileRect tile = new Uc_CategoryTileRect();
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
