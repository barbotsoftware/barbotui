using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls
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

                foreach (Container c in containers)
                {
                    Console.WriteLine("Container Number: " + c.Number + 
                                      ", Current Volume: " + c.CurrentVolume +
                                      ", Max Volume: " + c.MaxVolume +
                                      ", Ingredient: " + Helpers.GetIngredientByIngredientId(ingredients, c.IngredientId).Name);
                }

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
