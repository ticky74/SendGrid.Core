using System;
using System.IO;
using System.Threading.Tasks;

namespace SendGrid.Core
{
    public class SendGridAttachment
    {
        public string Base64Content {get;set;}
        public string ContentType {get;set;}
        public string FileName {get;set;}
        public string Disposition => "attachment";

        public static SendGridAttachment FromFile(string localFilePath, string contentType, string filename)
        {
            using (var stream = File.OpenRead(localFilePath))
            {
                return FromStream(stream, contentType, filename);
            }
        }

        public static async Task<SendGridAttachment> FromFileAsync(string localFilePath, string contentType, string filename)
        {
            using (var stream = File.OpenRead(localFilePath))
            {
                return await FromStreamAsync(stream, contentType, filename);
            }
        }

        public static SendGridAttachment FromStream(Stream sourceStream, string contentType, string filename)
        {
                using (var memoryStream = new MemoryStream())
                {
                    sourceStream.CopyTo(memoryStream);
                    return FromBytes(memoryStream.ToArray(), contentType, filename);
                }
        }

        public static async Task<SendGridAttachment> FromStreamAsync(Stream sourceStream, string contentType, string filename)
        {
                using (var memoryStream = new MemoryStream())
                {
                    await sourceStream.CopyToAsync(memoryStream);
                    return FromBytes(memoryStream.ToArray(), contentType, filename);
                }
        }        
        public static SendGridAttachment FromBytes(byte[] data, string contentType, string filename)
        {
            return new SendGridAttachment {
                Base64Content = Convert.ToBase64String(data),
                ContentType = contentType,
                FileName = filename
            };
        }
    }
}