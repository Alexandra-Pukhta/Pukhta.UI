using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cars.Domain.Entities;
using Pukhta.API.Data;
using Cars.Domain.Models;

namespace Pukhta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context, IWebHostEnvironment _environment)
        {
            _context = context;

        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Car>>>> GetProductListAsync(
              string? category,
              int pageNo = 1,
              int pageSize = 3)
        {
            // Создать объект результата
            var result = new ResponseData<ListModel<Car>>();

            // Фильтрация по категории загрузка данных категории
            var data = _context.Products
            .Include(d => d.Category)
            .Where(d => String.IsNullOrEmpty(category)
            || d.Category.NormalizedName.Equals(category));

            // Подсчет общего количества страниц
            int totalPages = (int)Math.Ceiling(data.Count() / (double)pageSize);
            if (pageNo > totalPages)
                pageNo = totalPages;

            // Создание объекта ProductListModel с нужной страницей данных
            var listData = new ListModel<Car>()
            {
                Items = await data
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };
            // поместить данные в объект результата
            result.Data = listData;
            // Если список пустой
            if (data.Count() == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }
            return result;
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Car product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Car>> PostProduct(Car product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        [HttpPost("{id}")]

        public async Task<IActionResult> SaveImage(int id, IFormFile image, [FromServices] IWebHostEnvironment env)
        {
            // Найти объект по Id
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Путь к папке wwwroot/Images
            var imagesPath = Path.Combine(env.WebRootPath, "images");

            // получить случайное имя файла
            var randomName = Path.GetRandomFileName();
            // получить расширение в исходном файле
            var extension = Path.GetExtension(image.FileName);

            // задать в новом имени расширение как в исходном файле
            var fileName = Path.ChangeExtension(randomName, extension);

            // полный путь к файлу
            var filePath = Path.Combine(imagesPath, fileName);

            // создать файл и открыть поток для записи
            using var stream = System.IO.File.OpenWrite(filePath);

            // скопировать файл в поток
            await image.CopyToAsync(stream);

            // получить Url хоста
            var host = "https://" + Request.Host;

            // Url файла изображения
            var url = $"{host}/images/{fileName}";

            // Сохранить url файла в объекте
            product.Image = url;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
    
