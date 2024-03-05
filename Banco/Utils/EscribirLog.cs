using System;
using System.IO;

namespace Banco.Utils
{
  
    public static class EscribirLog
    {
    
        public static void escribirEnFicheroLog(string mensajeLog)
        {
            try
            {

                using (FileStream fs = new FileStream(@AppDomain.CurrentDomain.BaseDirectory + "banco.log", FileMode.OpenOrCreate, FileAccess.Write))
                {
                   
                    using (StreamWriter m_streamWriter = new StreamWriter(fs))
                    {
                     
                        m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                        
                        mensajeLog = mensajeLog.Replace(Environment.NewLine, " | ");
                        mensajeLog = mensajeLog.Replace("\r\n", " | ").Replace("\n", " | ").Replace("\r", " | ");

                      
                        m_streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " " + mensajeLog);

                       
                        m_streamWriter.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Error EscribirLog - escribirEnFicheroLog()] Error al escribir en el fichero log:" + e.Message);
            }
        }
    }
}
