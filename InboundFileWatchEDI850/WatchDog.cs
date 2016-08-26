//using Domain;
//using Helpers;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace InboundFileWatchEDI850
//{
//    public class WatchDog
//    {
//        FileSystemWatcher m_FileSystemWatcher;
//        public string m_Path { get; private set; }
//        public string m_FilePath { get; set; }

//        public string m_ConnectionString { get; set; }

//        public bool m_IsHSA { get; private set; }

//        public RegKeys m_RegKeyEDIFile { get; set; }

//        public WatchDog(RegKeys _RegKeyEDIFile, string _ConnectionString, bool _IsHSA)
//        {
//            if (PathExists())
//            {

//                m_ConnectionString = _ConnectionString;
//                m_FileSystemWatcher = new FileSystemWatcher();
//                m_FileSystemWatcher.Path = _RegKeyEDIFile.RegistryKeyValue;
//                m_FileSystemWatcher.Created += OnFileAdded;
//                m_FileSystemWatcher.EnableRaisingEvents = true;
//                m_IsHSA = _IsHSA;

//            }

//        }

//        private bool PathExists()
//        {
//            bool bResult = false;

//            if (!Directory.Exists(m_RegKeyEDIFile.RegistryKeyValue))
//            {
//                SendEmailPathNotFound();

//            }
//            else
//            {
//                bResult = true;
//            }

//            return bResult;

//        }
//        private void SendEmailPathNotFound()
//        {
//            EDIHelperFunctions cEDIHelperFunctions = new EDIHelperFunctions();
//            cEDIHelperFunctions.SendEmail(string.Format("The path set for EDI 850 could not be found. Please contact your administrator to check registry key {0}  its value is {1}  Date time {2}",
//                                                        m_RegKeyEDIFile.RegistryKey, m_RegKeyEDIFile.RegistryKeyValue, DateTime.Now.ToString()),
//                                                      "ERROR !! Path to EDI 850 not found");

//        }

//        private void OnFileAdded(object sender, FileSystemEventArgs argsFileAdd)
//        {
//            EDIHelperFunctions cEDIHelperFunctions = new EDIHelperFunctions();
//            if (IsHSA)
//            {
//                ProcessEDI850 cProcessEDI850 = new EDIService.ProcessEDI850(argsFileAdd.FullPath, m_ConnectionString);
//            }
//            else
//            {
//                cEDIHelperFunctions.SendEmail(string.Format("EDI file 850 {0} has arrived and ready to be processed on the admin side. The current time is {1}", argsFileAdd.FullPath, DateTime.Now.ToString()),
//                                              "New EDI file 850 is here");
//            }
//        }

//    }
//}
