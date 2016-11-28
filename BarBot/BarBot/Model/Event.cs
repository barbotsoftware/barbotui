/*
 * Event.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.Model
{
	public class Event : JsonModelObject
	{
		private string _name;
		private string _data;

		public Event()
		{
		}

		public Event(string name, string data)
		{
			Name = name;
			Data = data;
		}

		public Event(string json)
		{
			var e = (Event)parseJSON(json, typeof(Event));
			Name = e.Name;
			Data = e.Data;
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

		public string Data
		{
			get { return _data; }
			set
			{
				_data = value;
				OnPropertyChanged("Data");
			}
		}
	}
}
