using System;
using System.Threading.Tasks;


namespace BusinessRegister
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var filePath = await DataFile.DataFile.GetFile();
            var repository = new Repository.Repository(filePath);
            var result = repository.GetCompany("12652512").Result;

            Console.WriteLine(result);
        }
    }
}