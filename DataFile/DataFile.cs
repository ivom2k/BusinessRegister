using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataFile
{
    public static class DataFile
    {
        private static HttpClient _httpClient = default!;
        private const string Url = "https://avaandmed.rik.ee/andmed/ARIREGISTER/ariregister_csv.zip";
        private const string FilePrefix = "ettevotja_rekvisiidid";

        public static async Task GetFile()
        {
            var appDataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var csvFilePath = Directory.EnumerateFiles(appDataDirPath).FirstOrDefault(f => f.Contains(FilePrefix));
            var zipFilePath = Path.Combine(appDataDirPath, "ariregister_csv.zip");

            if (string.IsNullOrEmpty(csvFilePath))
            {
                await DownloadFile(zipFilePath);
                ExtractFile(zipFilePath, appDataDirPath);
                DeleteFile(zipFilePath);
            }
            else if (File.Exists(csvFilePath))
            {
                if (FindAgeOfTheFileInDays(csvFilePath) > 7)
                {
                    Console.WriteLine("age: " + FindAgeOfTheFileInDays(csvFilePath));
                    await DownloadFile(zipFilePath);
                    ExtractFile(zipFilePath, appDataDirPath);
                    DeleteFile(zipFilePath);
                    DeleteFile(csvFilePath);
                }
            }
        }

        private static int FindAgeOfTheFileInDays(string filePath)
        {
            var dateFromFilePath = Path.GetFileNameWithoutExtension(filePath).Split("_").LastOrDefault();

            try
            {
                var date = DateTime.Parse(dateFromFilePath!);
                return DateTime.Now.Subtract(date).Days;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Extracting the days failed! ", nameof(filePath), e);
            }
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

        private static void ExtractFile(string filePath, string directory)
        {
            try
            {
                ZipFile.ExtractToDirectory(filePath, directory);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Extracting failed! ", nameof(filePath), e);
            }
        }

        private static void DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Deleting failed! ", nameof(filePath), e);
            }
        }
    }
}