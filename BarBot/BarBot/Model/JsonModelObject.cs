/*
 * JsonModelObject.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/4/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BarBot.Model
{
	public abstract class JsonModelObject
	{
		public JsonModelObject()
		{
		}

		public string toJSON()
		{
			return JsonConvert.SerializeObject(
				this, 
				Formatting.Indented, 
				new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
		}

		public object parseJSON(string json)
		{
			dynamic obj = JsonConvert.DeserializeObject(json);
			return obj;
		}
	}
}
