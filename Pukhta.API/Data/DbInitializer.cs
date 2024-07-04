using Cars.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Pukhta.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            // Получение контекста БД
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            //Выполнение миграций
            await context.Database.MigrateAsync();

            if (!context.Categories.Any() && !context.Products.Any())
            {
                var _categories = new Category[]
            {
        new Category {GroupName="Легковые",
        NormalizedName="Легковые"},
        new Category {GroupName="Внедорожные",
        NormalizedName="Внедорожные"},
        new Category {GroupName="Грузовые",
        NormalizedName="Грузовые"}
            };

                await context.Categories.AddRangeAsync(_categories);
                await context.SaveChangesAsync();

                var _products = new List<Car>
        {
        new Car { Name = "Mercedes",
                Description = "Спортивная серая ",
                Image = "Images/01.jfif",
            Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Легковые")) },

        new Car { Name = "Range Rover",
                Description = "Внедорожник оранжевый",
                Image = "Images/011.jpg",
            Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Внедорожные")) },


        new Car { Name = "BMW",
                Description = "Легковая красная",
                Image = "Images/033.jfif",
            Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Легковые")) },


        new Car { Name = "Freightliner",
                Description = "Грузовая серая",
                Image = "Images/036.jfif",
            Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Грузовые")) },


        new Car { Name = "AUDI",
                Description = "Внедорожник оранжевый",
                Image = "Images/099.jpg",
            Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Внедорожные")) },

        new Car { Name = "Freightliner",
                Description = "Грузовая голубая",
                Image = "Images/035.jfif",
            Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Грузовые")) },

        new Car { Name = "AUDI",
                Description = "Внедорожник серый",
                Image = "Images/023.jfif",
            Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Внедорожные")) },

        new Car { Name = "BMW",
                Description = "Легковая голубая",
                Image = "Images/055.jfif",
            Category = _categories.FirstOrDefault(c => c.NormalizedName.Equals("Легковые")) },

        };

                await context.Products.AddRangeAsync(_products);
                await context.SaveChangesAsync();

            }
        }
    }
}
