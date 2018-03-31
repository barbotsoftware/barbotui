using System.ComponentModel;

using Newtonsoft.Json;

namespace BarBot.Core.Model
{
    public class Container : JsonModelObject, INotifyPropertyChanged
    {
        string barbotId;
        string ingredientId;
        int number;
        double currentVolume;
        double maxVolume;

        [JsonProperty("barbot_id")]
        public string BarbotId
        {
            get
            {
                return barbotId;
            }

            set
            {
                barbotId = value;
                OnPropertyChanged("BarbotId");
            }
        }

        [JsonProperty("ingredient_id")]
        public string IngredientId
        {
            get
            {
                return ingredientId;
            }

            set
            {
                ingredientId = value;
                OnPropertyChanged("IngredientId");
            }
        }

        [JsonProperty("number")]
        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        [JsonProperty("current_volume")]
        public double CurrentVolume
        {
            get
            {
                return currentVolume;
            }

            set
            {
                currentVolume = value;
                OnPropertyChanged("CurrentVolume");
            }
        }

        [JsonProperty("max_volume")]
        public double MaxVolume
        {
            get
            {
                return maxVolume;
            }

            set
            {
                maxVolume = value;
                OnPropertyChanged("MaxVolume");
            }
        }

        public Container()
        {
        }

        public Container(string barbotId, string ingredientId, int number,
                         double currentVolume, double maxVolume)
        {
            BarbotId = barbotId;
            IngredientId = ingredientId;
            Number = number;
            CurrentVolume = currentVolume;
            MaxVolume = maxVolume;
        }

        public Container(string json)
        {
            var c = (Container)parseJSON(json, typeof(Container));
            BarbotId = c.BarbotId;
            IngredientId = c.IngredientId;
            Number = c.Number;
            CurrentVolume = c.CurrentVolume;
            MaxVolume = c.MaxVolume;
        }
    }
}
