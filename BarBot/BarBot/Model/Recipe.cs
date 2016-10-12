using System.ComponentModel;
/*
 * Recipe.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.Model
{
	public class Recipe : JsonModelObject, INotifyPropertyChanged
	{
        private string _id;
        private string _name;
        private string _img;
        private Step[] _steps;

		public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

		public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

		public string Img
        {
            get { return _img; }
            set
            {
                _img = value;
                OnPropertyChanged("Img");
            }
        }
		public Step[] Steps
        {
            get { return _steps; }
            set
            {
                _steps = value;
                OnPropertyChanged("Steps");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

		public Recipe()
		{
		}

		public Recipe(string id, string name, string img, Step[] steps)
		{
			Id = id;
			Name = name;
			Img = img;
			Steps = steps;
		}

		public Recipe(string json)
		{
			var r = (Recipe)parseJSON(json, typeof(Recipe));
			Id = r.Id;
			Name = r.Name;
			Img = r.Img;
			Steps = r.Steps;

			// To-do: query available ingredients to match IngredientId, add to Ingredients array
		}

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
