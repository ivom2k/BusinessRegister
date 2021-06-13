using System.Threading.Tasks;


namespace BusinessRegister
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await DataStore.DataFile.GetFile();
            
        }
    }
}