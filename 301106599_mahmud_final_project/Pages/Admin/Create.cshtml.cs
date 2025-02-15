using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using _301106599_mahmud_final_project.Data;
using _301106599_mahmud_final_project.Models;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace _301106599_mahmud_final_project.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IAmazonS3 _s3Client;

        private const string bucketName = "myawsbucket-tanvir";
        private readonly ILogger<CreateModel> _logger;
        private readonly IMemoryCache _cache;
        private readonly IAmazonSimpleSystemsManagement _ssmClient;
        DateTime totalStartTime = DateTime.UtcNow;

        public CreateModel(ApplicationDbContext db, IMemoryCache cache, IAmazonS3 s3Client, ILogger<CreateModel> logger)
        {
            var startTime = DateTime.UtcNow;
            _db = db;
            _s3Client = s3Client;
            _cache = cache;
            _logger = logger;
            _logger.LogInformation($"Constructor took {DateTime.UtcNow - startTime} ms");
        }

        [BindProperty]
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IFormFile ImageFile { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            var startTime = DateTime.UtcNow;
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

            _logger.LogInformation($"OnGetAsync took {DateTime.UtcNow - startTime} ms");
            _logger.LogInformation($"Total it took {DateTime.UtcNow - totalStartTime} ms");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
          

            var product = new Product
            {
              
                Name = Product.Name,
                SerialNo = Product.SerialNo,
                Date = Product.Date,
                Price = Product.Price,
                Quantity = Product.Quantity,
                CategoryId = Product.CategoryId,
                LocationId = Product.LocationId,
                Description = Product.Description,
                ImageUrl = await UploadFileToS3(ImageFile)
            };
            

            _db.Product.Add(product);
            await _db.SaveChangesAsync();
            TempData["success"] = "Product added successfully";
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
