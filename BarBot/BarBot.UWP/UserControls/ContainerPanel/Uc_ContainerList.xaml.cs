using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.ContainerPanel
{
    public sealed partial class Uc_ContainerList : UserControl, INotifyPropertyChanged
    {
        private List<Container> containers;
        private List<Ingredient> ingredients;

        public List<Container> Containers
        {
            get { return containers; }
            set
            {
                containers = value;

                // Sort Containers by Number
                containers.Sort((x, y) => x.Number.CompareTo(y.Number));

                List<Uc_ContainerTile> tiles = new List<Uc_ContainerTile>();
                for (int i = 0; i < containers.Count; i++)
                {
                    Uc_ContainerTile tile = new Uc_ContainerTile();
                    tile.SetValue(Grid.ColumnProperty, i % 4);
                    tile.SetValue(Grid.RowProperty, i / 4);
                    tile.Container = containers[i];
                    tile.Ingredient = Helpers.GetIngredientByIngredientId(ingredients, containers[i].IngredientId);
                    tiles.Add(tile);
                }
                icContainerItems.ItemsSource = tiles;

                OnPropertyChanged("Containers");
            }
        }

        public List<Ingredient> Ingredients
        {
            get { return ingredients; }
            set
            {
                ingredients = value;
                OnPropertyChanged("Ingredients");
            }
        }

        public Uc_ContainerList()
        {
            this.InitializeComponent();
            this.Ingredients = (Application.Current as App).IngredientsInBarbot;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
