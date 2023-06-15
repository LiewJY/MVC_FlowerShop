using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;


namespace MVC_FlowerShop.Controllers
{
    public class S3ExampleController : Controller
    {
        private const string S3BucketName = "mvcflowershoplab3tp054701";

        //function 2: modify to bevume upload file page
        public IActionResult Index()
        {
            return View();
        }

        //fucntion 1: learn how to get back the key from the appsetting.json file
        private List<string> getKeys()
        {
            List<string> keys = new List<string>();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfiguration configure = builder.Build();

            keys.Add(configure["Values:key1"]);
            keys.Add(configure["Values:key2"]);
            keys.Add(configure["Values:key3"]);

            return keys;
        }

        //function 3: learn how to upload multiple or single file to s3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessUploadImage(List<IFormFile> imagefile)
        {
            //start connect
            List<string> keys = getKeys();
            AmazonS3Client s3Agent = new AmazonS3Client(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);

            //file validation 
            foreach (var singleimage in imagefile)
            {
                if (singleimage.Length <= 0)
                {
                    return BadRequest("File of " + singleimage.FileName + " is no content! Please try again.");
                }
                else if (singleimage.Length >= 1048576) //file more than 1MB
                {
                    return BadRequest("File of " + singleimage.FileName + " is more than 1MB! Please try again.");
                }
                else if (singleimage.ContentType.ToLower() != "image/png" && singleimage.ContentType.ToLower() != "image/jpeg")
                {
                    return BadRequest("File of " + singleimage.FileName + " is not a valid image we need! Please try again.");

                }

                //pass the validation then submit to s3
                try
                {
                    //upload to s3
                    PutObjectRequest request = new PutObjectRequest
                    {
                        InputStream = singleimage.OpenReadStream(),
                        BucketName = S3BucketName,
                        Key =  "images/" + singleimage.FileName,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    //execute your request
                    await s3Agent.PutObjectAsync(request);

                }
                catch (AmazonS3Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            //once done uploading go back to index page
            return RedirectToAction("Index", "S3Example");
        }
    
    
    }
}
