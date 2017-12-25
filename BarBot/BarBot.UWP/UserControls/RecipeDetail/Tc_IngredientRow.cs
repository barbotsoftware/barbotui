using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.RecipeDetail
{
    public sealed class Tc_IngredientRow : Control, INotifyPropertyChanged
    {
        private Ingredient ingredient;
        private string volumeText;

        // Action Button Click Event Handlers
        private RoutedEventHandler DecrementVolumeButton_Click;
        private RoutedEventHandler IncrementVolumeButton_Click;
        private RoutedEventHandler RemoveIngredientButton_Click;

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

        public Tc_IngredientRow(Ingredient ingredient, 
                                RoutedEventHandler decrementVolumeButton_Click,
                                RoutedEventHandler incrementVolumeButton_Click,
                                RoutedEventHandler removeIngredientButton_Click)
        {
            this.DefaultStyleKey = typeof(Tc_IngredientRow);
            this.DataContext = this;

            Ingredient = ingredient;

            DecrementVolumeButton_Click += decrementVolumeButton_Click;
            IncrementVolumeButton_Click += incrementVolumeButton_Click;
            RemoveIngredientButton_Click += removeIngredientButton_Click;
        }

        protected override void OnApplyTemplate()
        {
            var decrementVolumeButton = GetTemplateChild("DecrementVolumeButton") as Button;
            var incrementVolumeButton = GetTemplateChild("IncrementVolumeButton") as Button;
            var removeIngredientButton = GetTemplateChild("RemoveIngredientButton") as Button;

            decrementVolumeButton.Click += DecrementVolumeButton_Click;
            incrementVolumeButton.Click += IncrementVolumeButton_Click;
            removeIngredientButton.Click += RemoveIngredientButton_Click;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
