using System;
using System.Net.Mail;

namespace MD.Bidding.Helpers
{
    public class Email
    {
        #region Constants

        /// <summary>
        /// the html type
        /// </summary>
        private const string emailTypeHtml = "text/html";

        /// <summary>
        /// plain type
        /// </summary>
        private const string emailTypePlain = "text/plain";

        /// <summary>
        /// send failure message
        /// </summary>
        private const string errorSendFailure = "the email could not be sent";

        #endregion // Constants

        #region Member Variables

        /// <summary>
        /// the mail to address
        /// </summary>
        private string mailToAddress;

        /// <summary>
        /// the mail to name
        /// </summary>
        private string mailToName;

        /// <summary>
        /// the mail from address
        /// </summary>
        private string mailFromAddress;

        /// <summary>
        /// the mail from name
        /// </summary>
        private string mailFromName;

        /// <summary>
        /// the mail subject
        /// </summary>
        private string mailSubject;

        /// <summary>
        /// the mail body html
        /// </summary>
        private string mailBodyHtml;

        /// <summary>
        /// the mail body text
        /// </summary>
        private string mailBodyText;

        #endregion

        #region Properties

        /// <summary>
        /// Sets the to address
        /// </summary>
        public string MailToAddress
        {
            set
            {
                this.mailToAddress = value;
            }
        }

        /// <summary>
        /// Sets the to display name
        /// </summary>
        public string MailToName
        {
            set
            {
                this.mailToName = value;
            }
        }

        /// <summary>
        /// Sets the from address
        /// </summary>
        public string MailFromAddress
        {
            set
            {
                this.mailFromAddress = value;
            }
        }

        /// <summary>
        /// Sets the mail from name
        /// </summary>
        public string MailFromName
        {
            set
            {
                this.mailFromName = value;
            }
        }

        /// <summary>
        /// Sets the subject
        /// </summary>
        public string Subject
        {
            set
            {
                this.mailSubject = value;
            }
        }

        /// <summary>
        /// Sets the mail body html
        /// </summary>
        public string MailBodyHtml
        {
            set
            {
                this.mailBodyHtml = value;
            }
        }

        /// <summary>
        /// Sets the mail body text
        /// </summary>
        public string MailBodyText
        {
            set
            {
                this.mailBodyText = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send the email now
        /// </summary>
        /// <returns></returns>
        public bool SendMail()
        {
            bool success = false;
            //MailAddress from;
            //MailAddress to;

            // execute the guard clauses
            this.EnsureValidData();

            // set the addresses
            //from = (this.mailFromName != null) ? new MailAddress(this.mailFromAddress, this.mailFromName) : new MailAddress(this.mailFromAddress);
            //to = (this.mailToName != null) ? new MailAddress(this.mailToAddress, this.mailToName) : new MailAddress(this.mailToAddress);

            //// create the message
            //MailMessage mail = new MailMessage(from, to);

            MailMessage mail = new MailMessage();
            foreach (string address in this.mailToAddress.Split(';'))
            {
                mail.To.Add(new MailAddress(address, this.mailToName));
            }
            mail.From = new MailAddress(this.mailFromAddress, this.mailFromName);

            // set the content
            mail.Subject = this.mailSubject;

            // dual format mail
            if (this.mailBodyHtml != null && this.mailBodyText != null)
            {
                // create the Plain Text and html parts part
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(this.mailBodyText, null, emailTypePlain);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(this.mailBodyHtml, null, emailTypeHtml);

                // add to the mail message
                mail.AlternateViews.Add(plainView);
                mail.AlternateViews.Add(htmlView);
            }
            else if (this.mailBodyHtml != null)
            {
                mail.Body = this.mailBodyHtml;
                mail.IsBodyHtml = true;
            }
            else if (this.mailBodyText != null)
            {
                mail.Body = this.mailBodyText;
            }

            // send the message
            SmtpClient smtp = new SmtpClient(string.Empty);

            try
            {
                smtp.Send(mail);
                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        #endregion // Public Methods

        #region Private Methods

        /// <summary>
        /// Ensure that the data is valid
        /// </summary>
        private void EnsureValidData()
        {
            if (this.mailFromAddress == null)
            {
                throw new ArgumentOutOfRangeException("from", this.mailFromAddress, "must be a non empty value");
            }

            if (this.mailToAddress == null)
            {
                throw new ArgumentOutOfRangeException("to", this.mailToAddress, "must be a non empty value");
            }

            if (this.mailSubject == null)
            {
                throw new ArgumentOutOfRangeException("subject", this.mailSubject, "must be a non empty value");
            }

            if (this.mailBodyHtml == null && this.mailBodyText == null)
            {
                throw new ArgumentOutOfRangeException("mail body", "must not be an empty value");
            }
        }

        #endregion // Private Methods
    }
}
