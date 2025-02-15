using _301106599_mahmud_final_project.Data;
using _301106599_mahmud_final_project.Models;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _301106599_mahmud_final_project.Pages.Admin
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<EditModel> _logger;
        private readonly IAmazonS3 _s3Client;
        private readonly IAmazonSimpleSystemsManagement _ssmClient;

        private const string bucketName = "myawsbucket-tanvir";

        public EditModel(ApplicationDbContext db, ILogger<EditModel> logger, IAmazonS3 s3Client, IAmazonSimpleSystemsManagement ssmClient)
        {
            _db = db;
            _logger = logger;
            _s3Client = s3Client;
            _ssmClient = ssmClient;
        }


        public Product Product { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IFormFile? ImageFile { get; set; }

        public async Task OnGetAsync(int? id)
        {
            Categories = await _db.Category.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToListAsync();

            Locations = await _db.Location.Select(l => new SelectListItem
            {
                Value = l.LocationId.ToString(),
                Text = l.Name
            }).ToListAsync();


           
            if (id != null && id != 0)
            {
                Product = await _db.Product.FindAsync(id);
            }
            if (Categories == null || Locations == null)
            {
                _logger.LogError("Categories or Locations list is null.");
                
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Product == null || Product.ProductId == 0)
            {
                return Page();
            }

            var existingProduct = await _db.Product.FindAsync(Product.ProductId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = Product.Name;
            existingProduct.SerialNo = Product.SerialNo;
            existingProduct.Date = Product.Date;
            existingProduct.Price = Product.Price;
            existingProduct.Quantity = Product.Quantity;
            existingProduct.CategoryId = Product.CategoryId;
            existingProduct.LocationId = Product.LocationId;
            existingProduct.Description = Product.Description;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                existingProduct.ImageUrl = await UploadFileToS3(ImageFile);
            }
      

            _db.Product.Update(existingProduct);
            await _db.SaveChangesAsync();
            TempData["success"] = "Product updated successfully";
            return RedirectToPage("/Admin/Admin-Index");
        }


        private async Task<string> GetParameterValueAsync(string parameterName)
        {
            try
            {
                var request = new GetParameterRequest
                {
                    Name = parameterName,
                    WithDecryption = true
                };

                var response = await _ssmClient.GetParameterAsync(request);
                return response.Parameter.Value;
            }
            catch (ParameterNotFoundException)
            {
                Console.WriteLine($"Parameter {parameterName} not found.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving parameter {parameterName}: {ex.Message}");
                return null;
            }
        }

        private async Task<string> UploadFileToS3(IFormFile file)
        {
    
            string accessKey = await GetParameterValueAsync("AWS_ACCESS_KEY_ID");
            string secretKey = await GetParameterValueAsync("AWS_SECRET_ACCESS_KEY");


            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("AccessKey or SecretKey not found in Parameter Store.");
            }

            var s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.USEast2);
            var fileTransferUtility = new TransferUtility(s3Client);

            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(newMemoryStream);

                    var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = uniqueFileName,
                        BucketName = bucketName
                    };

                    await fileTransferUtility.UploadAsync(uploadRequest);
                    return $"https://{bucketName}.s3.amazonaws.com/{uniqueFileName}";
                }
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error encountered on server. Message:'{e.Message}' when writing an object");
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unknown encountered on server. Message:'{e.Message}' when writing an object");
                throw;
            }
        }
    }
}
