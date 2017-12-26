using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace BarBot.UWP.UserControls.RecipeDetail
{
    public sealed class Tc_IngredientRow : Control, INotifyPropertyChanged
    {
        private Ingredient ingredient;
        private string volumeText;

        public List<Ingredient> AvailableIngredients;

        private ComboBox IngredientComboBox;

        // Combo Box Selection Changed Event Handler
        private SelectionChangedEventHandler IngredientComboBox_SelectionChanged;

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
                                List<Ingredient> availableIngredients,
                                SelectionChangedEventHandler ingredientComboBox_SelectionChanged,
                                RoutedEventHandler decrementVolumeButton_Click,
                                RoutedEventHandler incrementVolumeButton_Click,
                                RoutedEventHandler removeIngredientButton_Click)
        {
            this.DefaultStyleKey = typeof(Tc_IngredientRow);
            this.DataContext = this;

            Ingredient = ingredient;
            AvailableIngredients = availableIngredients;

            IngredientComboBox_SelectionChanged += ingredientComboBox_SelectionChanged;
            DecrementVolumeButton_Click += decrementVolumeButton_Click;
            IncrementVolumeButton_Click += incrementVolumeButton_Click;
            RemoveIngredientButton_Click += removeIngredientButton_Click;
        }

        protected override void OnApplyTemplate()
        {
            IngredientComboBox = GetTemplateChild("IngredientComboBox") as ComboBox;
            var decrementVolumeButton = GetTemplateChild("DecrementVolumeButton") as Button;
            var incrementVolumeButton = GetTemplateChild("IncrementVolumeButton") as Button;
            var removeIngredientButton = GetTemplateChild("RemoveIngredientButton") as Button;

            IngredientComboBox.SelectionChanged += IngredientComboBox_SelectionChanged;
            PopulateComboBox();

            decrementVolumeButton.Click += DecrementVolumeButton_Click;
            incrementVolumeButton.Click += IncrementVolumeButton_Click;
            removeIngredientButton.Click += RemoveIngredientButton_Click;
        }

        public void PopulateComboBox()
        {
            IngredientComboBox.Items.Clear();
            IngredientComboBox.Items.Add(Ingredient.Name);
            IngredientComboBox.SelectedIndex = 0;

            foreach (Ingredient i in AvailableIngredients)
            {
                i.Name = Helpers.UppercaseWords(i.Name);
                IngredientComboBox.Items.Add(i);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
