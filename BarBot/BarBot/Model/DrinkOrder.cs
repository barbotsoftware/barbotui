using System.ComponentModel;

using Newtonsoft.Json;

namespace BarBot.Core.Model
{
    public class DrinkOrder : JsonModelObject, INotifyPropertyChanged
    {
        string drinkOrderId;
        string userId;
        string userName;
        Recipe recipe;
        bool ice;
        bool garnish;
        string timestamp;

        [JsonProperty("drink_order_id")]
        public string DrinkOrderId
        {
            get
            {
                return drinkOrderId;
            }

            set
            {
                drinkOrderId = value;
                OnPropertyChanged("DrinkOrderId");
            }
        }

        public string UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public Recipe Recipe
        {
            get { return recipe; }
            set
            {
                recipe = value;
                OnPropertyChanged("Recipe");
            }
        }

        [JsonProperty("ice")]
        public bool Ice
        {
            get { return ice; }
            set
            {
                ice = value;
                OnPropertyChanged("Ice");
            }
        }

        [JsonProperty("garnish")]
        public bool Garnish
        {
            get { return garnish; }
            set
            {
                garnish = value;
                OnPropertyChanged("Garnish");
            }
        }

        public string Timestamp
        {
            get { return timestamp; }
            set
            {
                timestamp = value;
                OnPropertyChanged("Timestamp");
            }
        }

        public DrinkOrder() { }

        public DrinkOrder(string json)
        {
            var d = (DrinkOrder)parseJSON(json, typeof(DrinkOrder));
            DrinkOrderId = d.DrinkOrderId;
            UserId = d.UserId;
            UserName = d.UserName;
            Recipe = d.Recipe;
            Ice = d.Ice;
            Garnish = d.Garnish;
            Timestamp = d.Timestamp;
        }
    }
}
