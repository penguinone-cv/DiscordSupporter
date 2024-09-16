using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordSupporter.Utils
{
    public static class FileReader
    {
        public static string ReadFile(string path)
        {
            string data;
            try
            {
                var sr = new StreamReader(path);
                data = sr.ReadToEnd();
                sr.Close();
            }
            catch
            {
                throw;
            }
            return data;
        }
    }
}
