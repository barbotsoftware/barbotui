/*
 * Step.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/5/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

namespace BarBot.Model
{
	public class Step : JsonModelObject
	{
		public int StepNumber { get; set; }
		public int Type { get; set; }
		public string IngredientId { get; set; }
		public double? Quantity { get; set; }
		public string Measurement { get; set; }

		public Step(int stepNumber, int type, string ingredientId, double quantity, string measurement) 
		{
			StepNumber = stepNumber;
			Type = type;
			IngredientId = ingredientId;
			Quantity = quantity;
			Measurement = measurement;
		}

		public Step(string json)
		{
			var s = (Step)parseJSON(json);
			StepNumber = s.StepNumber;
			Type = s.Type;
			IngredientId = s.IngredientId;
			Quantity = s.Quantity;
			Measurement = s.Measurement;
		}
	}
}
