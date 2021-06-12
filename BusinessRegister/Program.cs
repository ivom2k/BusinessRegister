using System.Threading.Tasks;


namespace BusinessRegister
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            DataStore.DataStore dataStore = new();

            await dataStore.GetFile();
        }
    }
}