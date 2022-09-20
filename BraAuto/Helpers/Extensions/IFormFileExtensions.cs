using BraAuto.Resources;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace BraAuto.Helpers.Extensions
{
    public static class IFormFileExtensions
    {
        private const int ImgMaxLength = 10 * 1024 * 1024;
        private static IConfiguration _config;
        private static IConfiguration Config => _config ??= new HttpContextAccessor().HttpContext.RequestServices.GetService<IConfiguration>();

        private static readonly Cloudinary Cloudinary = new(new Account(Config.GetValue<string>("Cloudinary:AppName"), Config.GetValue<string>("Cloudinary:AppKey"), Config.GetValue<string>("Cloudinary:AppSecret")));

        public static bool IsValidImg(this IFormFile file)
        {
            if(file == null) { return false; }

            var fileExtension = Path.GetExtension(file.FileName);
            var contentType = file.ContentType;
            var size = file.Length;

            if (!string.Equals(fileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(fileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(fileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(contentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(contentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (size >= ImgMaxLength)
            {
                return false;
            }

            return true;
        }

        public static async Task<List<string>> UploadImgsAsync(this IEnumerable<IFormFile> imgs)
        {
            var imgUrls = new List<string>();

            foreach (var img in imgs)
            {
                imgUrls.Add(await img.UploadImgAsync());
            }

            return imgUrls;
        }

        public static async Task<string> UploadImgAsync(this IFormFile img)
        {
            byte[] destinationImage;

            using (var memoryStream = new MemoryStream())
            {
                await img.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            var result = string.Empty;

            using (var destinationStream = new MemoryStream(destinationImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(img.FileName, destinationStream),
                };

                var res = await Cloudinary.UploadAsync(uploadParams);
                var url = res.Url.AbsoluteUri.Split("/", StringSplitOptions.RemoveEmptyEntries).ToList();

                result = url[url.Count - 2] + "/" + url[url.Count - 1];
            }

            return result;
        }
    }
}
