using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace BraAuto.Helpers.CloudinaryService
{
    public static class CloudinaryService
    {
        private static IConfiguration _config;
        private static IConfiguration Config => _config ??= new HttpContextAccessor().HttpContext.RequestServices.GetService<IConfiguration>();

        private static readonly Cloudinary Cloudinary = new(new Account(Config.GetValue<string>("Cloudinary:AppName"), Config.GetValue<string>("Cloudinary:AppKey"), Config.GetValue<string>("Cloudinary:AppSecret")));

        public static async Task DeletePhoto(string url) => await DeletePhotos(new string[] { url });

        public static async Task DeletePhotos(IEnumerable<string> urls)
        {
            foreach (var url in urls)
            {
                var publicId = Path.ChangeExtension(url.Split("/").Last(), null);

                DeletionParams deletionParams = new DeletionParams(publicId);

                await Cloudinary.DestroyAsync(deletionParams);
            }
        }
    }
}
