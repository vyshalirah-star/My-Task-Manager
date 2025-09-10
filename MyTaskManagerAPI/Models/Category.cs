using System.Collections.Generic;

namespace MyTaskManagerAPI.Models
{
    public class Category
    {
        public required int CategoryId { get; set; }// like story id
        public required string Name { get; set; }// Story Name
        public string? Description { get; set; }// story desc
        
    }
}
