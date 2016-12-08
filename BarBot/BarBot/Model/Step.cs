/*
 * Step.cs
 * BarBot
 * 
 * Created by Naveen Yadav on 10/5/16.
 * Copyright © 2016 BarBot. All rights reserved.
 */

using System.ComponentModel;

namespace BarBot.Core.Model
{
	public class Step : JsonModelObject, INotifyPropertyChanged
	{


		private int _stepNumber;
		private int _type;
		private string _ingredientId;
		private double? _quantity;
		private string _measurement;

		public int StepNumber
		{
			get { return _stepNumber; }
			set
			{
				_stepNumber = value;
				OnPropertyChanged("StepNumber");
			}
		}

		public int Type
		{
			get { return _type; }
			set
			{
				_type = value;
				OnPropertyChanged("Type");
			}
		}

		public string IngredientId
		{
			get { return _ingredientId; }
			set
			{
				_ingredientId = value;
				OnPropertyChanged("IngredientId");
			}
		}

		public double? Quantity
		{
			get { return _quantity; }
			set
			{
				_quantity = value;
				OnPropertyChanged("Quantity");
			}
		}

		public string Measurement
		{
			get { return _measurement; }
			set
			{
				_measurement = value;
				OnPropertyChanged("Measurement");
			}
		}

		public Step()
		{
		}

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
			var s = (Step)parseJSON(json, typeof(Step));
			StepNumber = s.StepNumber;
			Type = s.Type;
			IngredientId = s.IngredientId;
			Quantity = s.Quantity;
			Measurement = s.Measurement;
		}

		public override string ToString()
		{
			return string.Format("[Step: StepNumber={0}, Type={1}, IngredientId={2}, Quantity={3}, Measurement={4}]", StepNumber, Type, IngredientId, Quantity, Measurement);
		}
	}
}
