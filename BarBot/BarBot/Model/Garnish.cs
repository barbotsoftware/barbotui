using Newtonsoft.Json;
using System.ComponentModel;

namespace BarBot.Core.Model
{
    public class Garnish : JsonModelObject, INotifyPropertyChanged
    {
        string barbotId;
        string name;
        int optionNumber;
        int quantity;

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

        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [JsonProperty("option_number")]
        public int OptionNumber
        {
            get
            {
                return optionNumber;
            }
            set
            {
                optionNumber = value;
                OnPropertyChanged("OptionNumber");
            }
        }

        [JsonProperty("quantity")]
        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public Garnish()
        {

        }

        public Garnish(string barbotId, string name, int optionNumber, int quantity)
        {
            BarbotId = barbotId;
            Name = name;
            OptionNumber = optionNumber;
            Quantity = quantity;
        }

        public Garnish(string json)
        {
            var g = (Garnish)parseJSON(json, typeof(Garnish));
            BarbotId = g.barbotId;
            Name = g.Name;
            OptionNumber = g.OptionNumber;
            Quantity = g.Quantity;
        }
    }
}
