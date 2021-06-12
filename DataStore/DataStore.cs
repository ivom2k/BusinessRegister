using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace DataStore
{
    public class DataStore
    {
        private static HttpClient _httpClient = default!;
        private const string Url = "https://avaandmed.rik.ee/andmed/ARIREGISTER/ariregister_csv.zip";
        private const string FilePrefix = "ettevotja_rekvisiidid";

        public async Task GetFile()
        {
            var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var csvFilePath = Directory.EnumerateFiles(appDataDirPath).FirstOrDefault(f => f.Contains(FilePrefix));
            var zipFilePath = Path.Combine(appDataDirPath, "ariregister_csv.zip");
            // var zipFileExistsAlready = File.Exists(zipFilePath);

            if (string.IsNullOrEmpty(csvFilePath)) // Searching for extracted file
            {
                await DownloadFile(zipFilePath); // Downloading the zip to %AppData%
            }

            // Console.WriteLine("file exists? " + zipFileExistsAlready);
            //
            // Console.WriteLine(appDataDirPath);
            // Console.WriteLine("appDataDir exists? " + Directory.Exists(appDataDirPath));
            //
            // Console.WriteLine("downloading file");
            // Console.WriteLine("file exists? " + zipFileExistsAlready);

            // ZipFile.ExtractToDirectory(zipFilePath, appDataDirPath);

            // Console.WriteLine("csvFilePath '" + csvFilePath + "'");

            // var fileAge = FindAgeOfTheFileInDays(csvFilePath);

            // Console.WriteLine("fileAge " + fileAge);

            // File.Delete(appDataDirPath + "\\ariregister_csv.zip");
        }

        private int FindAgeOfTheFileInDays(string filePath)
        {
            var dateFromFilePath = Path.GetFileNameWithoutExtension(filePath).Split("_").LastOrDefault();
            var date = DateTime.Parse(dateFromFilePath!);

            return DateTime.Now.Subtract(date).Days;
        }

        private static async Task DownloadFile(string savePath)
        {
            _httpClient = new HttpClient();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();

                    FileStream fs = new FileStream(savePath, FileMode.Create);
                    await CopyContent(content, fs);
                }
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException("URI is invalid! ", e);
            }
            catch (HttpRequestException e)
            {
                throw new ArgumentException("Network resource is unavailable! ", e);
            }
            catch (TaskCanceledException e)
            {
                throw new ArgumentException("Request timed out! ", e);
            }
        }

        private static async Task CopyContent(Stream content, FileStream fs)
        {
            try
            {
                await using (fs)
                {
                    await content.CopyToAsync(fs);
                }
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentException("Destination is null! ", e);
            }
            catch (ObjectDisposedException e)
            {
                throw new ArgumentException("Current or destination stream is already closed! ", e);
            }
            catch (NotSupportedException e)
            {
                throw new ArgumentException("Writing or reading is not supported! ", e);
            }
        }
    }
}