using BarBot.Core.Model;
using BarBot.UWP.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BarBot.UWP.UserControls.AppBar.Filter
{
    public sealed partial class FilterContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private List<Ingredient> ingredients;

        public List<Ingredient> Ingredients
        {
            get { return ingredients; }
            set
            {
                ingredients = value;

                // Sort Ingredients by Name
                ingredients.Sort((x, y) => x.Name.CompareTo(y.Name));

                List<Button> buttons = new List<Button>();
                for (int i = 0; i < ingredients.Count; i++)
                {
                    Thickness margin = new Thickness(0, 0, 0, 30);
                    Button button = new Button()
                    {
                        Content = Helpers.UppercaseWords(ingredients[i].Name),
                        Width = 275,
                        Height = 80,
                        FontSize = 34,
                        FontFamily = new FontFamily("Microsoft Yi Baiti"),
                        Margin = margin
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

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            
            // Toggle background color
            // Set Ingredient Filter on in App.cs
        }

        public FilterContentDialog()
        {
            this.InitializeComponent();

            var ingredientDictionary = (Application.Current as App).IngredientsInBarbot;

            this.Ingredients = ingredientDictionary.Values.ToList();
        }

        private void FilterDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void FilterDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
