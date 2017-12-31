using System;
using System.Collections.Generic;

namespace BarBot.Core.Model
{
    class GarnishList : JsonModelObject
    {
        List<Garnish> garnishes;

        public List<Garnish> Garnishes
        {
            get
            {
                return garnishes;
            }
            set
            {
                garnishes = value;
            }
        }

        public GarnishList() { }

        public GarnishList(String json)
        {
            Garnishes = (List<Garnish>)parseJSON(json, typeof(List<Garnish>));
        }
    }
}
