using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;

using BarBot.Core.ViewModel;

namespace BarBot.Droid.View.Container
{
    public class ContainerAdapter : ArrayAdapter<Core.Model.Container>
    {
        ContainersViewModel ViewModel => App.Locator.Containers;

        public ContainerAdapter(Context context, List<Core.Model.Container> containers) : base(context, 0, containers)
        {
        }

        public override int Count
        {
            get
            {
                return ViewModel.Containers.Count;
            }
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            var container = ViewModel.Containers[position];

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.ContainerRow, null);
            }

            var containerTextView = convertView.FindViewById<TextView>(Resource.Id.container_textview);
            containerTextView.Text = string.Format("Container {0}", container.Number);

            var ingredient = App.IngredientsInBarBot.First(i => i.IngredientId == container.IngredientId);
            var ingredientTextView = convertView.FindViewById<TextView>(Resource.Id.ingredient_textview);
            ingredientTextView.Text = ingredient.Name;

            var currentVolume = new SpannableString(container.CurrentVolume.ToString());
            currentVolume.SetSpan(new ForegroundColorSpan(GetTextViewColor(container)), 0, currentVolume.ToString().Length, SpanTypes.ExclusiveExclusive);

            var volumeTextView = convertView.FindViewById<TextView>(Resource.Id.volume_textview);
            volumeTextView.TextFormatted = currentVolume;
            volumeTextView.Append(string.Format("/{0} oz", container.MaxVolume));

            return convertView;
        }

        private Color GetTextViewColor(Core.Model.Container container)
        {
            if (container.CurrentVolume > (container.MaxVolume * .8))
            {
                return Color.Green;
            }
            else if (container.CurrentVolume > (container.MaxVolume * .6))
            {
                return Color.YellowGreen;
            }
            else if (container.CurrentVolume > (container.MaxVolume * .4))
            {
                return Color.DarkOrange;
            }
            else if (container.CurrentVolume > (container.MaxVolume * .2))
            {
                return Color.OrangeRed;
            }
            else
            {
                return Color.Red;
            }
        }
    }
}
