using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.RecipeDetail
{
    public sealed partial class Uc_IngredientElement : UserControl, INotifyPropertyChanged
    {
        public Ingredient ingredient;
        private string volumeText;
        private double volumeAvailable;
        public bool VolumeChangeInProgress = false;

        public Ingredient Ingredient
        {
            get
            {
                return ingredient;
            }

            set
            {
                ingredient = value;
                ingredient.Name = Helpers.UppercaseWords(ingredient.Name);
                VolumeText = ingredient.Amount.ToString() + " oz";
                OnPropertyChanged("Ingredient");
            }
        }

        public string VolumeText
        {
            get
            {
                return volumeText;
            }

            set
            {
                volumeText = value;
                OnPropertyChanged("VolumeText");
            }
        }

        public double VolumeAvailable
        {
            get
            {
                return volumeAvailable;
            }
            set
            {
                volumeAvailable = value;
                OnPropertyChanged("VolumeAvailable");
                PopulateVolumeSelector();
            }
        }

        public Uc_IngredientElement(Ingredient ingredient, double volumeAvailable)
        {
            this.InitializeComponent();
            this.DataContext = this;

            Ingredient = ingredient;
            VolumeAvailable = volumeAvailable;
        }

        private void PopulateVolumeSelector()
        {
            VolumeChangeInProgress = true;
            //ingredientVolume.Items.Clear();


            //if (Ingredient.Amount >= 0.5)
            //{
            //    for (var i = 0.5; i <= VolumeAvailable + Ingredient.Amount; i += 0.5)
            //    {
            //        ingredientVolume.Items.Add(i);
            //    }
            //}
            //else
            //{
            //    ingredientVolume.Items.Add((double)0);
            //    ingredientVolume.SelectedIndex = 0;

            //    for (double i = 0; i <= VolumeAvailable + Ingredient.Amount; i += 0.5)
            //    {
            //        ingredientVolume.Items.Add(i);
            //    }
            //}

            //if (ingredientVolume.Items.IndexOf(Ingredient.Amount) > -1)
            //{
            //    ingredientVolume.SelectedIndex = ingredientVolume.Items.IndexOf(Ingredient.Amount);
            //}
            //else
            //{
            //    if(ingredientVolume.Items.Count > 0)
            //    {
            //        ingredientVolume.SelectedIndex = 0;
            //    }
            //}

            VolumeChangeInProgress = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
