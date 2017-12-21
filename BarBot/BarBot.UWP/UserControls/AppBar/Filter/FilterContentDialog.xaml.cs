using BarBot.Core.Model;
using BarBot.UWP.UserControls.RecipeList;
using BarBot.UWP.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace BarBot.UWP.UserControls.AppBar.Filter
{
    public sealed partial class FilterContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private App app;                                // Contains Global Filter List
        private List<Ingredient> ingredients;           // All Ingredients in Barbot
        private List<Ingredient> filterIngredients;     // Filter List local to ContentDialog
        private List<Button> buttons;

        private SolidColorBrush BarbotBlue = new SolidColorBrush(Color.FromArgb(255, 0, 75, 153));
        private Brush DarkGray = new SolidColorBrush(Color.FromArgb(255, 68, 68, 68));

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
                        Background = app.FilterIngredients.Contains(ingredient) ? BarbotBlue : DarkGray
                    };
                    button.SetValue(Grid.ColumnProperty, i % 4);
                    button.SetValue(Grid.RowProperty, i / 4);
                    button.Click += FilterButton_Click;
                    
                    buttons.Add(button);
                }
                icFilterItems.ItemsSource = buttons;

                OnPropertyChanged("Ingredients");
            }
        }

        public FilterContentDialog()
        {
            this.InitializeComponent();
            this.app = Application.Current as App;
            this.buttons = new List<Button>();
            this.filterIngredients = new List<Ingredient>(app.FilterIngredients);
            this.Ingredients = app.IngredientsInBarbot.Values.ToList();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            // Add Ingredient to filterIngredients local list
            var ingredient = app.IngredientsInBarbot[button.Name];
            if (this.filterIngredients.Contains(ingredient))
            {
                this.filterIngredients.Remove(ingredient);
                button.Background = DarkGray;
            }
            else
            {
                this.filterIngredients.Add(ingredient);
                button.Background = BarbotBlue;
            }
        }

        private void FilterDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            app.ClearFilters();
            app.ApplyFilters(this.filterIngredients);

            // Dismiss Modal
            sender.Hide();
        }

        // Clear Action
        private void FilterDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Clear both filter lists
            this.filterIngredients.Clear();
            app.ClearFilters();

            sender.Hide();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
