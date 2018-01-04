using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace BarBot.UWP.UserControls.RecipeDetail
{
    public sealed partial class Uc_AddIngredientButton : UserControl
    {
        public Uc_AddIngredientButton()
        {
            this.InitializeComponent();
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(this, "PointerDown", true);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PointerUp", true);
            this.ReleasePointerCapture(e.Pointer);
        }
    }
}
