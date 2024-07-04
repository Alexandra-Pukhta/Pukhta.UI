using System.Linq;
using System.Threading.Tasks;
using Pukhta.UI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cars.Domain.Entities;
using Pukhta.UI.Services;
using Cars.Domain.Entities;

namespace Pukhta.UI.Areas.Admin.Pages
{
    public class CreateModel(ICategoryService categoryService, IProductService productService) : PageModel
    {

        public async Task<IActionResult> OnGet()
        {
            var categoryListData = await categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id",
            "GroupName");
            return Page();
        }
        [BindProperty]
        public Car product { get; set; } = default!;
        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await productService.CreateProductAsync(product, Image);
            return RedirectToPage("./Index");
        }
    }
}
