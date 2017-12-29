using BarBot.Core.Model;
using BarBot.UWP.IO;
using BarBot.UWP.Pages;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static BarBot.Core.Constants;

namespace BarBot.UWP.UserControls.RecipeDetail.Dialogs
{
    public sealed partial class PouringContentDialog : ContentDialog, INotifyPropertyChanged
    {
        private BarbotIOController barbotIOController;
        private bool ice;
        private GarnishType garnish;
        private string text;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public PouringContentDialog(Recipe orderedRecipe, bool ice, GarnishType garnish)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.barbotIOController = (Application.Current as App).barbotIOController;
            this.ice = ice;
            this.garnish = garnish;

            this.Opened += (s, a) => Dialog_Opened(s, a, orderedRecipe);

            this.Text = string.Format("Your {0} is pouring!", orderedRecipe.Name);
        }

        private async void Dialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args, Recipe recipe)
        {
            if (barbotIOController != null)
            {
                Dictionary<IO.Devices.IContainer, double> ingredients = Utils.Helpers.GetContainersFromRecipe(recipe, barbotIOController.Containers);

                barbotIOController.PourDrinkSync(ingredients, ice, garnish);
            }

            // TODO: Update with how many seconds it takes to pour a drink
            await Task.Delay(3000);

            sender.Hide();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
