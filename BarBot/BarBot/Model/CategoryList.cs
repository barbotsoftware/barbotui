using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.Core.Model
{
    class CategoryList : JsonModelObject
    {
        List<Category> categories;

        public List<Category> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
            }
        }

        public CategoryList() { }

        public CategoryList(String json)
        {
            Categories = (List<Category>)parseJSON(json, typeof(List<Category>));
        }
    }
}
