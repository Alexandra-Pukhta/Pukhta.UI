using Cars.Domain.Entities;
using Cars.Domain.Models;

namespace Pukhta.UI.Services
{
    public class MemoryCategoryService : ICategoryService
    {
        public Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var categories = new List<Category>
        {
        new Category {Id=1, GroupName="Легковые",
        NormalizedName="Легковые"},
        new Category {Id=2, GroupName="Внедорожные",
        NormalizedName="Внедорожные"},
        new Category {Id=3, GroupName="Грузовые",
        NormalizedName="Грузовые"}

        };
            var result = new ResponseData<List<Category>>();
            result.Data = categories;
            return Task.FromResult(result);
        }


    }
}
