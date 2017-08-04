using System;

namespace SendGrid.Core
{
    public class SendGridAttachment
    {
        public string Base64Content {get;set;}
        public string ContentType {get;set;}
        public string FileName {get;set;}
        public string Disposition => "attachment";

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