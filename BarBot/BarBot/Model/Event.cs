/*
 * Event.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 9/22/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System;
namespace BarBot.Model
{
	public class Event
	{
		public string Name { get; set; }
		public string Data { get; set; }

		public Event()
		{
		}

		public Event(string name, string data)
		{
			Name = name;
			Data = data;
		}
	}
}
