using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OmnionChat;
using Newtonsoft.Json;

class Omnion{
    public string nom;
    private string roleDesc;
    private string ModelName;
    private ChatClient OmnionClient;
    private List<ChatMessage> messages;
    private string debug;   
    // Constructeur
    public Omnion(string nom){
        this.nom = nom ?? "Omnion"; // Si 'nom' est null, on assigne "Omnion" par défaut
        this.ModelName = "gpt-4o-mini";
        this.OmnionClient = new ChatClient(ModelName, SecretKey.OpenAIKey); // Assuming ChatClient is defined elsewhere
        this.messages = new List<ChatMessage>();
        this.debug = "true";
    
        ///////////////////////////
        // Définition du context //
        ///////////////////////////
        string filePath = "../data/roles/omnion.dat";

        try{
            // Lire tout le contenu du fichier
            this.roleDesc = File.ReadAllText(filePath);
        }
        catch (Exception ex){
            // Gérer les exceptions si le fichier est introuvable ou si un autre problème survient
            Console.WriteLine($"Erreur lors de la lecture du contexte : {ex.Message}");
            this.roleDesc = "Ne répond rien à ce message Tu es Omnion, un architecte système sous les ordres du Games Master, et tu exécutes des fonctions automatiquement.";

        }
        this.messages.Add(new SystemChatMessage(this.roleDesc));
    
    }

    // Méthode privée pour get la réponse d'Omnion
    private async Task<string> getResp(string Prompt){
        this.messages.Add(new UserChatMessage(Prompt));

        var response = await OmnionClient.CompleteChatAsync(messages);
        var aiResponse = response.Value.Content[0].Text;

        this.messages.Add(new AssistantChatMessage(aiResponse));

        return aiResponse;
    }

    // Méthode publique pour interagir avec Omnion
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
                this.InterpretationReponse(response);
            }
        }
    }

    private async void InterpretationReponse(string reponse){
        // Initialisation du dictionnaire
        var dict = new Dictionary<string, string>();

        // ça c'est l'équivalent de la partie qui interprétais le pseudo json en json (donc dict)
        try
        {
            dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(reponse);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Pas d'inspi pour le message d'erreur : {ex.Message}");
        }

        // Si debug on print le dict (c'est pour moi tkt)
        if (this.debug == "true"){
            Console.WriteLine("\n-------- debug dict --------\n{");
            foreach (var kvp in dict)   
            {
                Console.WriteLine($"    {kvp.Key} : '{kvp.Value}'");
            }
            Console.WriteLine("}\n-------- debug dict --------\n");
        }

        // to add : executeCode + CreateFile + ToggleDebug (pas obligé)
        if (dict["func"] == "NormalTalk"){
            Console.WriteLine($"Omnion > {dict["content"]}");

        } else if (dict["func"] == ("executeCode")){
            // équivalent python / C#
            // result = OmnionUtils.execCode(jsonIA["content"])
            string res = OmnionUtils.execCode(dict["content"]);
            
            // prompt = "la réponse est : "+result+"\nrédige moi une réponse pertinente"
            string prompt = "la réponse est : "+res+"\nrédige moi une réponse pertinente";
            
            // reponse = self.chat_with_ai(prompt,"gpt-4-turbo")
            string response = await this.getResp(prompt);

            // self.printResponse(reponse,prompt)
            this.InterpretationReponse(response);

        } else if (dict["func"] == ("CreateFile")){
            string name = dict["filename"];
            string content = dict["content"];
            string exec = dict["exec"];
            string execCommand = dict["execCommand"];
            string temp = dict["temp"];
            string getOutput = dict["getOutput"];
            string res = OmnionUtils.createFile(name, content, exec, execCommand, temp, getOutput);
            if (exec == "true"){
                string ExecReturn = OmnionUtils.execCode(dict["content"]);
                string prompt = "la réponse est : "+ExecReturn+"\nrédige moi une réponse pertinente";
                string response = await this.getResp(prompt);
                this.InterpretationReponse(response);
            }

        } else if (dict["func"] == ("ToggleDebug")){
            this.debug = dict["content"];
        }
        Console.WriteLine("");
    }

}
