using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedCategory(DataContext context)
        {
            if (await context.Category.AnyAsync()) return;

            var categoriesData = await System.IO.File.ReadAllTextAsync("Data/CategoriesData.json");

            var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

            foreach (var category in categories)
            {
                context.Category.Add(category);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedProducts(DataContext context)
        {
            if (await context.Products.AnyAsync()) return;

            var productsData = await System.IO.File.ReadAllTextAsync("Data/CourseData.json");

            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            foreach (var product in products)
            {
                context.Products.Add(product);
            }


            await context.SaveChangesAsync();
        }


    }
}