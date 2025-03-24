using System;
using System.Threading.Tasks;

namespace OmnionChat
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();
            // Créer une instance de la classe Omnion
            Omnion omnion = new Omnion("Omnion");

            // Lancer l'interaction avec l'utilisateur
            await omnion.TalkWith();
        }
    }
}
