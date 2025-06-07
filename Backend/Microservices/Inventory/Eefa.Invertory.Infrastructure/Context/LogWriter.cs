using System;
using System.IO;
using System.Reflection;

namespace Eefa.Invertory.Infrastructure.Context
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        public LogWriter(string logMessage,string name="")
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            m_exePath +=  name == ""? "\\log\\log.txt": "\\log\\" + name+ ".txt";
            LogWrite(logMessage, m_exePath);
        }
        public void LogWrite(string logMessage, string Url)
        {

            try
            {
                using (StreamWriter w = File.AppendText(Url))
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
