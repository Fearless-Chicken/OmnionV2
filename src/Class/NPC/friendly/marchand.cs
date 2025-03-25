using OpenAI;
using System;
using System.IO;
using OmnionChat;
using OpenAI.Chat;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;

// Classe pour représenter un item dans le stock
public class Item
{
    public int id { get; set; }
    public string name { get; set; }
    public int price { get; set; }
}

class marchand : npcTemplate
{
    string stock;
    List<Item> stockData;

    public marchand(string nom, string classe, string role) : base(nom, classe, role)
    {
        string filePath = $"../data/NPCData/{classe}/{role}/Blanche/shop_inventory.json";
        this.stock = File.ReadAllText(filePath);
        this.stockData = JsonConvert.DeserializeObject<List<Item>>(stock);
        string res = "voici ton stock :\n"+this.stock;
        this.messages.Add(new SystemChatMessage(res));

    }

    public void test()
    {
        foreach (var item in this.stockData)
        {
            Console.WriteLine($"ID: {item.id}, Name: {item.name}, Price: {item.price}");
        }
    }
}
