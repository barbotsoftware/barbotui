using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BarBot.UWP.UserControls.RecipeDetail.IngredientList.Dialogs
{
    public sealed partial class IngredientChangeContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private List<Ingredient> ingredients;               // Ingredients not already used in Recipe
        private List<Button> buttons;
        public Ingredient SelectedIngredient { get; set; }  // Refer to this Ingredient in Recipe Detail

        public List<Ingredient> Ingredients
        {
            get { return ingredients; }
            set
            {
                ingredients = value;

                // Sort Ingredients by Name
                ingredients.Sort((x, y) => x.Name.CompareTo(y.Name));

                for (int i = 0; i < ingredients.Count; i++)
                {
                    var ingredient = ingredients[i];

                    Button button = new Button()
                    {
                        Content = Helpers.UppercaseWords(ingredient.Name),
                        Name = ingredient.IngredientId,                     // set button Name to IngredientId
                        Width = 275,
                        Height = 80,
                        FontSize = 34,
                        FontFamily = new FontFamily("Microsoft Yi Baiti"),
                        Margin = new Thickness(0, 0, 0, 30),
                        Background = Barbot_Colors.BarbotBlue
                    };
                    button.SetValue(Grid.ColumnProperty, i % 4);
                    button.SetValue(Grid.RowProperty, i / 4);
                    button.Click += IngredientButton_Click;

                    buttons.Add(button);
                }
                icIngredientItems.ItemsSource = buttons;

                OnPropertyChanged("Ingredients");
            }
        }

        public IngredientChangeContentDialog(List<Ingredient> availableIngredients)
        {
            this.InitializeComponent();

            this.buttons = new List<Button>();
            this.Ingredients = availableIngredients;
        }

        private void IngredientButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            SelectedIngredient = Ingredients.Where(i => i.IngredientId == btn.Name).First();
            SelectedIngredient.Amount = 0.5;
            ContentDialog.Hide();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
