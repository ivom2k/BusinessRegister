using System;
using DataStore;

namespace BusinessRegister
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DataStore.DataStore dataStore = new();
            
            dataStore.GetFile();
        }
    }
}