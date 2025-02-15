using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _301106599_mahmud_final_project.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SerialNo { get; set; }   
        [Required]
        public DateOnly Date { get; set; }    
        [Required]
        public decimal Price { get; set; }    
        [Required]
        public int Quantity { get; set; }      
        [Required]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        [Required]
        [ForeignKey("LocationId")]
        public int LocationId { get; set; }    
        [Required]
        public string Description { get; set; } 
        [Required]
        public string ImageUrl { get; set; }  
        
    }
}
