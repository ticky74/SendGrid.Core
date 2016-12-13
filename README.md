# SendGrid.Core

Wrapper around SendGrid web api v3 to send mail messages from your app.

1. Specify your api key before using the SendGridServer. You only have to do this once in your app in AppHost, Startup etc.
   ```
   var key = "<Your api key from sendgrid>";
   SendGridServer.Authorize(key);
   ```

2. Create a new SendGridMailMessage object
   ```
    var emailMessage = new SendGridMailMessage
    {
        Content = new List<MessageContent> 
        {
            {
                new MessageContent
                {
                    ContentType = "text/plain",
                    Content = "Hello from SendGrid."
                }            
            }
        },
        From = new SendGridMailContact
        {
            Email = "fromaddress@yourdomain.com",
            Name = "nameis notrequired"
        },
        Personalizations = new List<Personalization>
        {
            new Personalization
            {
                To = new List<SendGridMailContact> 
                {
                    {
                        new SendGridMailContact
                        {
                            Email = "your.recipient@theirdomain.com",
                            Name = "nameis notrequired"
                        }
                    }
                }
            }
        },
        Subject = "Some Subject Line"
    };
    ```

3. Use SendGridServer to send the message and check the http result. A successful operation should return a 202/Accepted response.
For more info on response codes you can check out the [SendGrid APIv3 Docs](https://sendgrid.com/docs/API_Reference/Web_API_v3/index.html).
   ```
   var httpresult = await SendGridServer.SendMailAsync(emailMessage);
   ```

