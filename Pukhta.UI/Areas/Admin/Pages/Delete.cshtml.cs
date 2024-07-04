using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cars.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pukhta.API.Data;

namespace Pukhta.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Car product { get; set; }

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var prod = await _context.Products.FindAsync(id);

            if (prod != null)
            {
                product = prod;
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
