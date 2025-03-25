using System;
using System.Threading.Tasks;
using System.Threading;

namespace OmnionChat
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();
            // // Créer une instance de la classe Omnion
            // Omnion omnion = new Omnion("Omnion");
            // // Lancer l'interaction avec l'utilisateur
            // await omnion.TalkWith();

            marchand npc1 = new marchand("Bili","friendly","marchand");
            Console.WriteLine("debut");
            npc1.StartLife();
            npc1.test();
            Thread.Sleep(1500);
            npc1.printLife();
        }
    }
}
