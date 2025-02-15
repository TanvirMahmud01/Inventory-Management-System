using _301106599_mahmud_final_project.Data;
using _301106599_mahmud_final_project.Models;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace _301106599_mahmud_final_project.Pages.Admin
{
    public class Admin_IndexModel : PageModel
    {
        public readonly ApplicationDbContext _db;
        private readonly IAmazonS3 _s3Client;
       

        private const string bucketName = "myawsbucket-tanvir";


        public Admin_IndexModel(ApplicationDbContext db, IAmazonS3 s3Client)
        {
            _db = db;
            _s3Client = s3Client;
      
        }

        public IList<Product> Products { get; set; }
        public IList<Category> Categories { get; set; }
        public IList<Location> Locations { get; set; }
        public string SearchTitle { get; set; }

        public async Task OnGetAsync()
        {
            var products = from m in _db.Product
                         select m;
            var categories = from m in _db.Category
                         select m;
            var locations = from m in _db.Location
                            select m;

            if (!string.IsNullOrEmpty(SearchTitle))
            {
                products = products.Where(s => s.Name.Contains(SearchTitle));
            }

            Products = await products.ToListAsync();
            Categories = await categories.ToListAsync();
            Locations = await locations.ToListAsync();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var product = await _db.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Delete the image from S3
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var uri = new Uri(product.ImageUrl);
                var key = uri.AbsolutePath.TrimStart('/'); // Extract the key from the URI

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            }

            _db.Product.Remove(product);
            await _db.SaveChangesAsync();

            TempData["success"] = "Product deleted successfully";
            return RedirectToPage();
        }

   
    }
}
