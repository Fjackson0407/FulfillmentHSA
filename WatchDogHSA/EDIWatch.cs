using Domain;
using EDIService;
using RegistryFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDogHSA
{
    public class EDIWatch
    {
        #region "Class members"
        public string m_Path { get; private set; }
        public string m_FilePath { get; set; }
        FileSystemWatcher m_FileSystemWatcher;
        public string m_ConnectionString { get; set; }

        public string  m_RegKeyEDIFile { get; set; }

        #endregion

        #region Constractor
        public EDIWatch(string  _RegKeyEDIFile, string _ConnectionString)
        {
            if (PathExists(_RegKeyEDIFile))
            {

                m_ConnectionString = _ConnectionString;

                //Cisco, is there a standalone FTP service that loads this directory?  I was expecting to see an FTP library in place.
                m_FileSystemWatcher = new FileSystemWatcher();
                m_FileSystemWatcher.Path = _RegKeyEDIFile;
                m_FileSystemWatcher.Created += OnFileAdded;
                m_FileSystemWatcher.EnableRaisingEvents = true;
            }

        }
        #endregion


        #region Class function 
        /// <summary>
        /// if thew path is not found send a email to let some one know about the error 
        /// </summary>
        /// <returns>bool</returns>
        private bool PathExists(string path )
        {
            if (!Directory.Exists(path))
            {
                return false;

            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Send email that path can not be found
        /// </summary>
        private void SendEmailPathNotFound()
        {
            GetKeys cGetKeys = new GetKeys();
            cGetKeys.SendEmail(string.Format("The path set for EDI 850 could not be found. Please contact your administrator to check registry key {0}  its value is {1}  Date time {2}",
                                                        m_RegKeyEDIFile, m_RegKeyEDIFile, DateTime.Now.ToString()),
                                                      "ERROR !! Path to EDI 850 not found");

        }

        /// <summary>
        /// Send file to get processed or parse edi csv file 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="argsFileAdd"></param>
        private void OnFileAdded(object sender, FileSystemEventArgs argsFileAdd)
        {
            GetKeys cGetKeys = new GetKeys();
            if (cGetKeys.HSA)
            {
                EDIPOService cEDIPOService = new EDIPOService(argsFileAdd.FullPath, m_ConnectionString);
                cEDIPOService.ParseEDI850();
            }
            else
            {
                cGetKeys.SendEmail(string.Format("EDI file 850 {0} has arrived and ready to be processed on the admin side. The current time is {1}", argsFileAdd.FullPath, DateTime.Now.ToString()),
                                              "New EDI file 850 is here");
            }
        }

        #endregion
    }
}
