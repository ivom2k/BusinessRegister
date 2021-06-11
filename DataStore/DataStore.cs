using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace DataStore
{
    public class DataStore
    {
        private const string Url = "https://avaandmed.rik.ee/andmed/ARIREGISTER/ariregister_csv.zip";
        private const string FilePrefix = "ettevotja_rekvisiidid_";

        public void GetFile()
        {
            var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var zipFilePath = $"{appDataDirPath}\\ariregister_csv.zip";
            var zipFileExistsAlready = File.Exists(zipFilePath);
            
            Console.WriteLine("file exists? " + zipFileExistsAlready);
            
            Console.WriteLine(appDataDirPath);
            Console.WriteLine("appDataDir exists? " + Directory.Exists(appDataDirPath));

            var zipSavePath = Path.Combine(appDataDirPath, "ariregister_csv.zip");
            
            // new WebClient().DownloadFile(new Uri(Url), zipSavePath);

            Console.WriteLine("downloading file");
            Console.WriteLine("file exists? " + zipFileExistsAlready);
            
            // ZipFile.ExtractToDirectory(zipFilePath, appDataDirPath);

            var csvFilePath = Directory.EnumerateFiles(appDataDirPath).FirstOrDefault(f => f.Contains(FilePrefix));
            
            Console.WriteLine("csvFilePath '" + csvFilePath + "'");

            var shouldReplaceFile = checkIfOlderThanNineDays(csvFilePath);

            // File.Delete(appDataDirPath + "\\ariregister_csv.zip");
        }

        private bool checkIfOlderThanNineDays(string filePath)
        {
            var result = false;

            Console.WriteLine(Path.GetFileName(filePath));
            
            return result;
        }

    }
}