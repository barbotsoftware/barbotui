/*
 * JsonModelObject.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/4/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BarBot.Model
{
	public abstract class JsonModelObject
	{
		JsonSerializerSettings JsonSettings { get; set; }

		/*
		 * Default Constructor. Initialize JsonSerializerSettings
		 * with SnakeCaseNamingStrategy, NullValueHandling
		 */ 
		protected JsonModelObject()
		{
			JsonSettings = new JsonSerializerSettings
			{
				ContractResolver = new DefaultContractResolver
				{
					NamingStrategy = new SnakeCaseNamingStrategy()
				},
				NullValueHandling = NullValueHandling.Ignore
			};
		}

		/*
		 * Serialize this object and return as a string.
		 */ 
		public string toJSON()
		{
			return JsonConvert.SerializeObject(
				this,
				Formatting.Indented,
				JsonSettings);
		}

		/*
		 * Deserialize a JSON string and create an object
		 * of the passed Type.
		 */
		public object parseJSON(string json, Type type)
		{
			var obj = JsonConvert.DeserializeObject(json, type, JsonSettings);
			return obj;
		}
	}
}
