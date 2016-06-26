using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Configuration;
using System.IO;

namespace MD.Bidding.Helpers
{
    public class SmsHelper
    {

        public static void SendSmsToOneContact(string apiBaseUrl, string phoneNumber, string messageBody, string senderId)
        {
            string apiUrl = apiBaseUrl + "&message=" + messageBody + "&from=" + senderId + "&to=" + phoneNumber;
            ConnectToSmsProvider(apiUrl);
        }

        public static void SendSmsToMultipleContacts(string apiBaseUrl, List<string> phoneNumbers, string messageBody, string senderId)
        {
            StringBuilder phoneContacts = new StringBuilder();
            foreach (string phoneNumber in phoneNumbers)
            {
                phoneContacts.Append(phoneNumber).Append(",");
            }
            string number = phoneContacts.ToString();
            string apiUrl = apiBaseUrl + "&message=" + messageBody + "&from=" + senderId + "&to=" + phoneContacts.ToString();
            ConnectToSmsProvider(apiUrl);
        }

        public static void ConnectToSmsProvider(string providerUrl)
        {
            Uri url = new Uri(providerUrl);
            WebRequest request = WebRequest.Create(providerUrl);                    
        }
    }
}
