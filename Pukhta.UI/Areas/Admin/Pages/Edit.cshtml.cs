using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pukhta.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cars.Domain.Entities;

namespace Pukhta.UI.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _environment;
        public EditModel(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }


        [BindProperty]
        public Car product { get; set; } = default!;
        [BindProperty]
        public IFormFile Image { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var prod = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (prod == null)
            {
                return NotFound();
            }
            else
            {
                product = prod;
            }
            ViewData["GroupId"] = new SelectList(_context.Products, "GroupId", "GroupName");
            return Page();

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Image != null)
            {
                var fileName = $"{product.Id}" +
                Path.GetExtension(Image.FileName);
                product.Image = fileName;
                var path = Path.Combine(_environment.WebRootPath, "Images",
                fileName);
                using (var fStream = new FileStream(path, FileMode.Create))
                {
                    await Image.CopyToAsync(fStream);
                }
            }


            _context.Attach(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
