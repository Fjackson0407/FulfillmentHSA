using Domain;
using Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidUSAEDI;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
using System.Net.Mail;
using Microsoft.Exchange.WebServices;
using EDIException;
using static Helpers.EDIHelperFunctions;

namespace RegistryFunctions
{
    public class GetKeys
    {


        public bool HSA
        {
            get
            {
                int iHSA = 0;
                RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey reg64 = localMachineRegistry64.OpenSubKey(EDIHelperFunctions.SoftwareNode, false);

                if (reg64 != null)
                {
                    object oHSA = reg64.GetValue(EDIHelperFunctions.HSA, EDIHelperFunctions.HSA_NOT_FOUND, RegistryValueOptions.None);
                    RegistryValueKind cRegistryValueKind = reg64.GetValueKind(EDIHelperFunctions.HSA);
                    iHSA = (int)oHSA;
                    if (iHSA == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new ExceptionsEDI(string.Format("{0} {1}", EDIHelperFunctions.Help, ErrorCodes.HSAError2));
                }


            }
        }


        public string ConnectionString
        {
            get
            {

                RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey reg64 = localMachineRegistry64.OpenSubKey(EDIHelperFunctions.SoftwareNode, false);

                if (reg64 != null)
                {
                    object oConnectionString = reg64.GetValue(EDIHelperFunctions.CONNECTIONSTRING, EDIHelperFunctions.CONNECTION_STRING_NOPT_FOUND, RegistryValueOptions.None);
                    RegistryValueKind cRegistryValueKind = reg64.GetValueKind(EDIHelperFunctions.CONNECTIONSTRING);
                    foreach (string item in (string[])oConnectionString)
                    {
                        return item;


                    }
                }

                return string.Empty;
            }
        }


        public RegKeys GetInbound
        {
            get
            {
                string sResult = string.Empty;
                RegKeys cEDIInboundPathRegistryKeyInfo = new RegKeys();
                RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey reg64 = localMachineRegistry64.OpenSubKey(EDIHelperFunctions.SoftwareNode, false);
                if (reg64 != null)
                {
                    if (this.HSA)
                    {

                        object oUserName = reg64.GetValue(EDIHelperFunctions.HSAFOLDEROLCATION, EDIHelperFunctions.INBOUND_FOLDER_NOT_FOUND, RegistryValueOptions.None);
                        cEDIInboundPathRegistryKeyInfo.RegistryKey = string.Format(@"HKEY_LOCAL_MACHINE\SOFTWARE\EDI");
                        foreach (string item in (string[])oUserName)
                        {
                            cEDIInboundPathRegistryKeyInfo.RegistryKeyValue = item;
                            break;
                        }
                    }
                    else
                    {
                        object oUserName = reg64.GetValue(EDIHelperFunctions.EDIFLOER, EDIHelperFunctions.INBOUND_FOLDER_NOT_FOUND, RegistryValueOptions.None);
                        cEDIInboundPathRegistryKeyInfo.RegistryKey = string.Format(@"HKEY_LOCAL_MACHINE\SOFTWARE\EDI");
                        foreach (string item in (string[])oUserName)
                        {
                            cEDIInboundPathRegistryKeyInfo.RegistryKeyValue = item;
                            break;
                        }
                    }
                }

                return cEDIInboundPathRegistryKeyInfo;
            }
        }


        public string GetInboundLocation()
        {
            string sResult = string.Empty;
            RegKeys cEDIInboundPathRegistryKeyInfo = new RegKeys();
            RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey reg64 = localMachineRegistry64.OpenSubKey(EDIHelperFunctions.SoftwareNode, false);
            if (reg64 != null)
            {
                if (this.HSA)
                {

                    object oUserName = reg64.GetValue(EDIHelperFunctions.HSAFOLDEROLCATION, EDIHelperFunctions.INBOUND_FOLDER_NOT_FOUND, RegistryValueOptions.None);
                    cEDIInboundPathRegistryKeyInfo.RegistryKey = string.Format(@"HKEY_LOCAL_MACHINE\SOFTWARE\EDI");
                    foreach (string item in (string[])oUserName)
                    {
                        return item;
                    }
                }
                else
                {
                    object oUserName = reg64.GetValue(EDIHelperFunctions.EDIFLOER, EDIHelperFunctions.INBOUND_FOLDER_NOT_FOUND, RegistryValueOptions.None);
                    cEDIInboundPathRegistryKeyInfo.RegistryKey = string.Format(@"HKEY_LOCAL_MACHINE\SOFTWARE\EDI");
                    foreach (string item in (string[])oUserName)
                    {
                        return item;
                    }
                }

            }
            return string.Empty;
        }



        public string GetASNPath()
        {
            string sResult = string.Empty;
            RegKeys cEDIInboundPathRegistryKeyInfo = new RegKeys();
            RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey reg64 = localMachineRegistry64.OpenSubKey(EDIHelperFunctions.SoftwareNode, false);
            if (reg64 != null)
            {
                object oUserName = reg64.GetValue(EDIHelperFunctions.ASNFOLDERLOCATION, EDIHelperFunctions.ASN_FOLDER_NOT_FOUND, RegistryValueOptions.None);
                cEDIInboundPathRegistryKeyInfo.RegistryKey = string.Format(@"HKEY_LOCAL_MACHINE\SOFTWARE\EDI");
                foreach (string item in (string[])oUserName)
                {
                    return item;
                }

            }
            return string.Empty;
        }

        public string GetASNTempLocation()
        {
            string sResult = string.Empty;
            RegKeys cEDIInboundPathRegistryKeyInfo = new RegKeys();
            RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey reg64 = localMachineRegistry64.OpenSubKey(EDIHelperFunctions.SoftwareNode, false);
            if (reg64 != null)
            {
                object oUserName = reg64.GetValue(EDIHelperFunctions.ASNTEMPFOLDERLOCATION, EDIHelperFunctions.ASN_FOLDER_NOT_FOUND, RegistryValueOptions.None);
                cEDIInboundPathRegistryKeyInfo.RegistryKey = string.Format(@"HKEY_LOCAL_MACHINE\SOFTWARE\EDI");
                foreach (string item in (string[])oUserName)
                {
                    return item;
                }

            }
            return string.Empty;
        }


        static bool RedirectionCallback(string url)
        {
            // Return true if the URL is an HTTPS URL.
            return url.ToLower().StartsWith("https://");
        }



        public void SendEmail(string Msg, string Subject, bool inuse)
        {
            EmailRecipient cEmailRecipient = GetUsernameAmdPassword();
            ExchangeService service = new ExchangeService();
            SmtpClient client = new SmtpClient();
            NetworkCredential cNetworkCredential = new NetworkCredential(cEmailRecipient.UserName, cEmailRecipient.Password);
            MailMessage cMailMessage = new MailMessage(cEmailRecipient.EmailAddress, "fjackson0407@yahoo.com");
            client.Port = 25;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = cNetworkCredential;
            client.Host = "mail.validusa.com";
            cMailMessage.Subject = "this is a test email.";
            cMailMessage.Body = "this is my test email body";
            client.Send(cMailMessage);
        }

        public void SendEmail()
        {

            EmailRecipient cEmailRecipient = GetUsernameAmdPassword();
            ExchangeService service = new ExchangeService();
            service.Credentials = new WebCredentials(cEmailRecipient.UserName,
                cEmailRecipient.Password);
            //("https://red003.mail.apac.microsoftonline.com/EWS/Exchange.asmx");
            service.Url = new Uri("https://mail.validusa.com/EWS/Exchange.asmx");
            //I need to handle this better!!!!
            //service.AutodiscoverUrl("ediprocess@validusa.com");
            EmailMessage message = new EmailMessage(service);
            message.Subject = "Subject";
           
            message.ToRecipients.Add("fjackson0407@yahoo.com");

            message.Body = "Msg";
            message.Send();



            ////System.Web.Mail.SmtpMail oMail = new System.Web.Mail.SmtpMail();
            //System.Net.Mail.SmtpClient cSmtpClient = new SmtpClient("validusa.com", 25);
            ////SmtpClient oSmtp = new SmtpClient();
            //MailAddress from = new MailAddress("ediprocess@validusa.com");
            //MailAddress to = new MailAddress("fjackson0407@yahoo.com");
            //MailMessage cMailMessage = new MailMessage(from, to);
            //cMailMessage.Body = "Testing 123 Testing 123";
            //cMailMessage.Subject = "Test for EDI";
            //cSmtpClient.Credentials = new NetworkCredential("ediprocess", "$ecure9!");
            //cSmtpClient.Send(cMailMessage);
            //cMailMessage.Dispose();
        }

        public void SendEmail(string Msg, string Subject)
        {

            EmailRecipient cEmailRecipient = GetUsernameAmdPassword();
            ExchangeService service = new ExchangeService();
            service.Credentials = new WebCredentials(cEmailRecipient.UserName, cEmailRecipient.Password);
            //I need to handle this better!!!!
            service.AutodiscoverUrl(cEmailRecipient.EmailAddress, RedirectionCallback);
            EmailMessage message = new EmailMessage(service);
            message.Subject = Subject;
            List<EmailAddress> lisAddress = new List<EmailAddress>();
            foreach (string item in cEmailRecipient.Recipients)
            {
                EmailAddress cEmailAddress = new EmailAddress();
                cEmailAddress.Address = item;
                lisAddress.Add(cEmailAddress);
            }
            message.ToRecipients.AddRange(lisAddress);

            message.Body = Msg;
            message.Send();



        }

        public EmailRecipient GetUsernameAmdPassword()
        {
            EmailRecipient cEmailInfomation = new EmailRecipient();
            RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey reg64 = localMachineRegistry64.OpenSubKey(EDIHelperFunctions.SoftwareNode, false);

            if (reg64 != null)
            {
                object oUserName = reg64.GetValue(EDIHelperFunctions.USERNAME, EDIHelperFunctions.USERNAME_NOT_FOUND, RegistryValueOptions.None);
                foreach (string item in (string[])oUserName)
                {
                    cEmailInfomation.UserName = item;
                    break;
                }

                object oPassword = reg64.GetValue(EDIHelperFunctions.PASSWORD, EDIHelperFunctions.PASSWORD_NOT_FOUND, RegistryValueOptions.None);
                foreach (string item in (string[])oPassword)
                {
                    cEmailInfomation.Password = item;
                    break;
                }

                object oEmail = reg64.GetValue(EDIHelperFunctions.EMAILADDRESS, EDIHelperFunctions.EMAIL_ADDRESS_NOT_FOUND, RegistryValueOptions.None);
                foreach (string item in (string[])oEmail)
                {
                    cEmailInfomation.EmailAddress = item;
                    break;
                }

                object oSMTP = reg64.GetValue(EDIHelperFunctions.SMTP, EDIHelperFunctions.SMTP_NOT_FOUND, RegistryValueOptions.None);
                foreach (string item in (string[])oSMTP)
                {
                    cEmailInfomation.SMTP = item;
                    break;
                }

                object oRecipients = reg64.GetValue(EDIHelperFunctions.RECIPIENTS, EDIHelperFunctions.RECIPIENTS_NOT_FOUNND, RegistryValueOptions.None);
                string Recipients = string.Empty;
                foreach (string item in (string[])oRecipients)
                {
                    Recipients = item;
                    break;
                }

                string[] results = Recipients.Split(',');
                foreach (string item in results)
                {
                    cEmailInfomation.Recipients.Add(item);
                }

            }

            return cEmailInfomation;

        }

    }
}
