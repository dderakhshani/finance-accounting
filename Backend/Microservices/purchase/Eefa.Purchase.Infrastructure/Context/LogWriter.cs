using System;
using System.IO;
using System.Reflection;

namespace Eefa.Purchase.Infrastructure.Context
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\log\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {

                txtWriter.WriteLine("{0} {1} : {2}", DateTime.Now.ToLongTimeString(),DateTime.Now.ToLongDateString(), logMessage);
                txtWriter.WriteLine("-------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
