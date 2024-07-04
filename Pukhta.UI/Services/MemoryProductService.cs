using Cars.Domain.Entities;
using Cars.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Pukhta.UI.Services
{
    public class MemoryProductService : IProductService
    {
        List<Car> _car;
        List<Category> _categories;
        IConfiguration _config;



        public MemoryProductService(ICategoryService categoryService, [FromServices] IConfiguration config)
        {
            _config = config;
            _categories = categoryService.GetCategoryListAsync()
                .Result
                .Data;

            SetupData();
        }





        /// <summary>
        /// Инициализация списков
        /// </summary>
        public void SetupData()
        {

            _car = new List<Car>
        {
            new Car { Id = 1, Name = "Mercedes",
                Description = "Спортивная серая ",
                Image = "Images/01.jfif",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Легковые")).Id }, 

            new Car { Id = 2, Name = "Range Rover",
                Description = "Внедорожник оранжевый",
                Image = "Images/011.jpg",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Внедорожные")).Id }, 

            new Car { Id = 1, Name = "BMW",
                Description = "Легковая красная",
                Image = "Images/033.jfif",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Легковые")).Id },
  
            new Car { Id = 3, Name = "Freightliner",
                Description = "Грузовая серая",
                Image = "Images/036.jfif",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Грузовые")).Id },


            new Car { Id = 2, Name = "AUDI",
                Description = "Внедорожник оранжевый",
                Image = "Images/099.jpg",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Внедорожные")).Id },
            
            new Car { Id = 3, Name = "Freightliner",
                Description = "Грузовая голубая",
                Image = "Images/035.jfif",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Грузовые")).Id },
           
            new Car { Id = 2, Name = "AUDI",
                Description = "Внедорожник серый",
                Image = "Images/023.jfif",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Внедорожные")).Id },
            
            new Car { Id = 1, Name = "BMW",
                Description = "Легковая голубая",
                Image = "Images/055.jfif",
                CategoryId = _categories.Find(c => c.NormalizedName.Equals("Легковые")).Id },
            
        };
        }


        Task<ResponseData<ListModel<Car>>> IProductService.GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {


            // Создать объект результата
            var result = new ResponseData<ListModel<Car>>();

            // Id категории для фильрации
            int? categoryId = null;

            // если требуется фильтрация, то найти Id категории
            // с заданным categoryNormalizedName
            if (categoryNormalizedName != null)
                categoryId = _categories
                .Find(c =>
                c.NormalizedName.Equals(categoryNormalizedName))
                ?.Id;

            // Выбрать объекты, отфильтрованные по Id категории,
            // если этот Id имеется
            var data = _car
            .Where(d => categoryNormalizedName == null || d.CategoryId.Equals(categoryId))?
            .ToList();

            // получить размер страницы из конфигурации
            int pageSize = _config.GetSection("ItemsPerPage").Get<int>();


            // получить общее количество страниц
            int totalPages = (int)Math.Ceiling(data.Count / (double)pageSize);

            // получить данные страницы
            var listData = new ListModel<Car>()
            {
                Items = data.Skip((pageNo - 1) *
            pageSize).Take(pageSize).ToList(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            // поместить ранные в объект результата
            result.Data = listData;



            // Если список пустой
            if (data.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбраннной категории";
            }
            // Вернуть результат
            return Task.FromResult(result);

        }

        public Task<ResponseData<Car>> CreateProductAsync(Car product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Car>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }



        public Task UpdateProductAsync(int id, Car product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
