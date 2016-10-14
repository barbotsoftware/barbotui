using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.Model
{
    class Container : JsonModelObject
    {
        private int number;
        private int ingredient_id;
        private int size;
        private int fluid_level;

        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public int Ingredient_id
        {
            get
            {
                return ingredient_id;
            }

            set
            {
                ingredient_id = value;
                OnPropertyChanged("Ingredient_id");
            }
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

        public int Fluid_level
        {
            get
            {
                return fluid_level;
            }

            set
            {
                fluid_level = value;
                OnPropertyChanged("Fluid_level");
            }
        }

        public Container() { }

        public Container(int number, int ingredient_id, int size, int fluid_level)
        {
            Number = number;
            Ingredient_id = ingredient_id;
            Size = size;
            Fluid_level = fluid_level;
        }

        public Container(string json)
        {
            var c = (Container)parseJSON(json, typeof(Container));
            Number = c.Number;
            Ingredient_id = c.Ingredient_id;
            Size = c.Size;
            Fluid_level = c.Fluid_level;    
        }
    }
}
