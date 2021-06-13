using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Repository
    {
        private readonly string _filePath;

        public Repository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task ReadFile()
        {
            var separator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            Console.WriteLine("filePath: " + _filePath);
            Console.WriteLine("separator " + separator);
            
            try
            {
                using StreamReader streamReader = new StreamReader(_filePath, Encoding.UTF8);
                for (var i = 0; i < 5; i++)
                {
                    Console.WriteLine(await streamReader.ReadLineAsync());
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}