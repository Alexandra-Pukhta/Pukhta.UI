using Microsoft.AspNetCore.Mvc.RazorPages;
using Cars.Domain.Entities;
using Pukhta.UI.Services;
using Cars.Domain.Entities;

namespace Pukhta.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IProductService _productService;
        public IndexModel(IProductService productService)
        {
            //_context = context;
            _productService = productService;
        }
        public List<Car> Products { get; set; } = default!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public async Task OnGetAsync(int? pageNo = 1)
        {
            var response = await _productService.GetProductListAsync(null, pageNo.Value);
            if (response.Success)
            {
                Products = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
        }
    }
}
