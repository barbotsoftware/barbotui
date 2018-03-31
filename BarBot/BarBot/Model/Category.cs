using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarBot.Core.Model
{
    public class Category : JsonModelObject, INotifyPropertyChanged
    {
        string categoryId;
        string name;
        string img;
        List<Recipe> recipes;
        List<Category> subCategories;

        [JsonProperty("category_id")]
        public string CategoryId
        {
            get { return categoryId; }
            set
            {
                categoryId = value;
                OnPropertyChanged("CategoryId");
            }
        }

        [JsonProperty("name")]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [JsonProperty("img")]
        public string Img
        {
            get { return img; }
            set
            {
                img = value;
                OnPropertyChanged("Img");
            }
        }

        [JsonProperty("recipes")]
        public List<Recipe> Recipes
        {
            get { return recipes; }
            set
            {
                recipes = value;
                OnPropertyChanged("Recipes");
            }
        }

        [JsonProperty("sub_categories")]
        public List<Category> SubCategories
        {
            get { return subCategories; }
            set
            {
                subCategories = value;
                OnPropertyChanged("SubCategories");
            }
        }

        public Category() { }

        public Category(string categoryId, string name, string img, List<Category> subCategories, List<Recipe> recipes)
        {
            CategoryId = categoryId;
            Name = name;
            Img = img;
            SubCategories = subCategories;
            Recipes = recipes;
        }

        public Category(String json)
        {
            var c = (Category)parseJSON(json, typeof(Category));
            CategoryId = c.CategoryId;
            Name = c.Name;
            Img = c.Img;
            SubCategories = c.SubCategories;
            Recipes = c.Recipes;
        }

        public static Category AllRecipes()
        {
            Category category = new Category();
            category.name = Constants.AllRecipesCategoryName;
            category.CategoryId = null;
            return category;
        }
    }
}
