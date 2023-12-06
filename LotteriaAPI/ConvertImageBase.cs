namespace LotteriaAPI
{
    public class ConvertImageBase
    {
        public static async Task<string> ImageToBase64(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }

        public static async Task<IFormFile> Base64ToImage(string base64String, string namefile)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);

            var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            var formFile = new FormFile(ms, 0, ms.Length, null, $"{namefile}.png");

            return formFile;
        }
    }
}
