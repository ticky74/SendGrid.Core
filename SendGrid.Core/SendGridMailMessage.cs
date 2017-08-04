using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SendGrid.Core
{
    [DataContract]
    public class SendGridMailMessage
    {
        [DataMember(Name = "personalizations")]
        public List<Personalization> Personalizations { get; set; }
        [DataMember(Name = "from")]
        public SendGridMailContact From { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public List<MessageContent> Content { get; set; }
        [DataMember(Name="attachments")]
        public List<SendGridAttachment> Attachments {get;set;}
        public SendGridMailMessage Attach(SendGridAttachment attachment)
        {
            if (attachment == null)
                return this;

            if (Attachments == null)
                Attachments = new List<SendGridAttachment>();

            Attachments.Add(attachment);
            return this;
        } 
    }
}