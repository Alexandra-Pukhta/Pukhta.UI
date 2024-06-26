using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.Domain.Entities
{
    public class Car
    {
        [Key]
        public int Id { get; set; } // id машины
        public string Name { get; set; } // название машины
        public string Description { get; set; } // описание машины

        public string? Image { get; set; } // имя файла изображения 

        // Навигационные свойства
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
