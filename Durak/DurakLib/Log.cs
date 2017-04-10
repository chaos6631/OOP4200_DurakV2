/* Log.cs - The object for the logging of the users actions during the game.
 * 
 * Author : Cameron Fenton
 * Version : 1.1
 * Since : 1.0, Apr 2017
 */

using CardLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DurakGameLib
{
    public class Log
    {
        #region CLASS MEMBERS

        #endregion

        #region INSTANCE MEMBERS
        /// <summary>
        /// Instance Members
        /// </summary>
        private StreamWriter log;
        private FileStream fileStream = null;
        private DirectoryInfo logDirInfo = null;
        private FileInfo logFileInfo;
        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Default Constructor for the Log object
        /// </summary>
        public Log()
        {

        }

        #endregion

        #region METHODS
        public void logString(string strLog)
        {

            string logFilePath = "C:\\Logs\\";

            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";

            logFileInfo = new FileInfo(logFilePath);

            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);

            if (!logDirInfo.Exists)
            {
                logDirInfo.Create();
            }

            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }

            log = new StreamWriter(fileStream);

            log.WriteLine(strLog);

            log.Close();
        }
        #endregion
    }
}
