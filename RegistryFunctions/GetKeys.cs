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
                        return  item;
                        

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


        public string  GetInboundLocation()
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
                            return  item;
                        }
                    }
                    else
                    {
                        object oUserName = reg64.GetValue(EDIHelperFunctions.EDIFLOER, EDIHelperFunctions.INBOUND_FOLDER_NOT_FOUND, RegistryValueOptions.None);
                        cEDIInboundPathRegistryKeyInfo.RegistryKey = string.Format(@"HKEY_LOCAL_MACHINE\SOFTWARE\EDI");
                        foreach (string item in (string[])oUserName)
                        {
                         return  item;
                        }
                    }
                
            }
            return string.Empty; 
        }




        public void SendEmail(string Msg, string Subject)
        {
            EmailRecipient cEmailRecipient = GetUsernameAmdPassword();
            ExchangeService service = new ExchangeService();
            service.Credentials = new WebCredentials(cEmailRecipient.UserName, cEmailRecipient.Password);
            service.AutodiscoverUrl(cEmailRecipient.EmailAddress);
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
