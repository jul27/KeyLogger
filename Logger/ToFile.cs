using System.IO;

namespace Logger
{
    public class ToFile : Sender
    {
        private string file = "log.txt";
        public ToFile()
        {

        }
        public string GetFromFile()
        {
            try
            {
                string s = File.ReadAllText(file);
                File.Delete(file);
                return s;
            }
            catch
            {
                return null;
            }
        }
        public bool Exists()
        {
            return File.Exists(file);
        }
        public override bool Send(string s)
        {
            try
            {
                if (!File.Exists(file))
                    File.WriteAllText(file, s);
                else
                    File.AppendAllText(file, s);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
