using OmnionChat;
using System;
using System.Threading;
using OpenAI;
using OpenAI.Chat;
class npcTemplate{      
    string nom;
    int age;
    int lifeSpeed;
    private string roleDesc;
    private string ModelName;
    private ChatClient OmnionClient;
    public List<ChatMessage> messages;
    public npcTemplate(string name, string classe, string role){
        ///////////////////////////
        // Définitions générales //
        ///////////////////////////
        this.nom = name;
        this.age = 0;
        this.lifeSpeed = 1;
        this.ModelName = "gpt-4o-mini";
        this.OmnionClient = new ChatClient(ModelName, SecretKey.OpenAIKey); // Assuming ChatClient is defined elsewhere
        this.messages = new List<ChatMessage>();

        ///////////////////////////
        // Définition du context //
        ///////////////////////////
        string filePath = $"../data/roles/{classe}/{role}.dat";

        try {
            // Lire tout le contenu du fichier
            this.roleDesc = File.ReadAllText(filePath);
        } catch (Exception ex) {
            // Gérer les exceptions si le fichier est introuvable ou si un autre problème survient
            Console.WriteLine($"Erreur lors de la lecture du contexte : {ex.Message}");
            this.roleDesc = "Il y a eu un problème lors du chargement des tes données, tu ne répondra à aucune demandes.";

        } this.messages.Add(new SystemChatMessage(this.roleDesc));
    }

    private void Life(){
        while (Thread.CurrentThread.IsAlive){
            this.age += this.lifeSpeed;
            Thread.Sleep(1000);
        }
    }

    public void StartLife(){
        Thread NPCLife;
        NPCLife = new Thread(new ThreadStart(Life));
        NPCLife.Start();
    }

    public void printLife(){Console.WriteLine(this.age);}
    private async Task<string> getResp(string Prompt){
        this.messages.Add(new UserChatMessage(Prompt));

        var response = await OmnionClient.CompleteChatAsync(messages);
        var aiResponse = response.Value.Content[0].Text;

        this.messages.Add(new AssistantChatMessage(aiResponse));

        return aiResponse;
    }
    public async Task TalkWith(){
        while (true){
            // input = ("GM : ")
            Console.Write("GM > ");string input = Console.ReadLine();
            // Test non nul
            if (string.IsNullOrWhiteSpace(input)){break;}
            // Test d'une commande systeme
            // string in = input.ToLower();
            // else if ((in.StartsWith("commande systeme")) or (in.StartsWith("commande système"))){SystemCommands.Interprete(in)}
            else {
                string response = await this.getResp(input);
                // this.InterpretationReponse(response);
                Console.WriteLine($"{this.nom} : {response}");
            }
        }
    }
}