using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using API.ApiViewModels;
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

            var products = JsonSerializer.Deserialize<List<ProductViewModels>>(productsData);

            foreach (var newProduct in products)
            {
                Product product = new Product();

                product.Name = newProduct.Name;
                product.Description = newProduct.Description;
                product.Rating = newProduct.Rating;
                product.Price = newProduct.Price;
                product.CategoryId = newProduct.CategoryId;
                product.ImageUrl = newProduct.ImageUrl;
                product.Instructor = newProduct.Instructor;
                product.Language = newProduct.Language;


                await context.Products.AddAsync(product);
                context.SaveChanges();


                ProductDetail productDetail = new ProductDetail();

                productDetail.Detail = newProduct.Detail;
                productDetail.ProductId = product.Id;

                await context.ProductDetail.AddAsync(productDetail);
                context.SaveChanges();
            }
            await context.SaveChangesAsync();
        }


    }
}