using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BarBot.Core.Model
{
    public class DrinkOrder : JsonModelObject, INotifyPropertyChanged
    {
        private string _id;
        private string _userId;
        private string _userName;
        private Recipe _recipe;
        private bool _ice;
        private bool _garnish;
        private string _timestamp;

        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged("UserId");
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public Recipe Recipe
        {
            get { return _recipe; }
            set
            {
                _recipe = value;
                OnPropertyChanged("Recipe");
            }
        }

        public bool Ice
        {
            get { return _ice; }
            set
            {
                _ice = value;
                OnPropertyChanged("Ice");
            }
        }

        public bool Garnish
        {
            get { return _garnish; }
            set
            {
                _garnish = value;
                OnPropertyChanged("Garnish");
            }
        }

        public string Timestamp
        {
            get { return _timestamp; }
            set
            {
                _timestamp = value;
                OnPropertyChanged("Timestamp");
            }
        }

        public DrinkOrder() { }

        public DrinkOrder(string json)
        {
            var d = (DrinkOrder)parseJSON(json, typeof(DrinkOrder));
            Id = d.Id;
            UserId = d.UserId;
            UserName = d.UserName;
            Recipe = d.Recipe;
            Ice = d.Ice;
            Garnish = d.Garnish;
            Timestamp = d.Timestamp;
        }
    }
}
