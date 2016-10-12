using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BarBot.Model;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BarBot.UserControls
{
    public sealed partial class Uc_RecipeTile : UserControl
    {
        private Recipe recipe;

        public Uc_RecipeTile()
        {
            this.InitializeComponent();

            Recipe = new Recipe();
            this.DataContext = this;
        }

        public Recipe Recipe
        {
            get
            {
                return recipe;
            }

            set
            {
                recipe = value;
            }
        }
    }
}
