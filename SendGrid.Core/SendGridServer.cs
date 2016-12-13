using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ServiceStack;

namespace SendGrid.Core
{
    internal static class InputValidator
    {
        private const string EmailPattern = @"\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*";
        private static readonly Regex EmailRegex = new Regex(EmailPattern, RegexOptions.Compiled);

        public static bool ValidEmailWasProvided(string input)
        {
            return EmailRegex.IsMatch(input);
        }
    }

    public class SendGridServer
    {
        private static string _apiKey;
        private static readonly Uri SendMailApiServer = new Uri("https://api.sendgrid.com/v3/mail/send");

        public static void Authorize(string apiKey)
        {
            _apiKey = apiKey;
        }

        public static async Task<HttpStatusCode> SendMailAsync(SendGridMailMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (string.IsNullOrEmpty(message.From?.Email)
                || !InputValidator.ValidEmailWasProvided(message.From.Email))
                throw new ArgumentException("Message must specify valid .From email address");

            if (message.Content?.Any() == false)
                throw new ArgumentException("No message content specified");

            if ((message.Personalizations?.Any() == false)
                || (message.Personalizations.SelectMany(x => x.To)?.Any() == false))
                throw new ArgumentException("No recipients specified");

            if (message.Personalizations
                .SelectMany(x => x.To).Any(y => !InputValidator.ValidEmailWasProvided(y.Email)))
                throw new ArgumentException("Invalid email recipient address(es) specified");


            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var result =
                await
                    httpClient.PostAsync(SendMailApiServer,
                        new StringContent(message.ToJson(), Encoding.UTF8, "application/json"));

            return result.StatusCode;
        }
    }
}