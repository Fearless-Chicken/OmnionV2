using System;
using System.IO;
using System.Diagnostics;

namespace OmnionChat {
    public class OmnionUtils {
        public static string execCode(string command){
            // Création du processus
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",  // Utilise cmd.exe pour les commandes sur Windows
                Arguments = "/c " + command,  // "/c" exécute la commande puis ferme cmd.exe
                RedirectStandardOutput = true,  // Redirige la sortie standard vers notre programme
                UseShellExecute = false,  // Empêche l'utilisation de l'interface shell
                CreateNoWindow = true  // Ne crée pas de fenêtre de terminal
            };

            // Démarrer le processus et capturer la sortie
            using (Process process = Process.Start(processStartInfo))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    // Lire et retourner la sortie
                    return reader.ReadToEnd();
                }
            }
        }

        public static string createFile(string name, string content, string exec, string execCommand, string temp, string getOutput){
            string path = $"./data/FileCreated/{name}";
            string res = "Aucune commande d'exec fournis. Pas de return";
            if (!File.Exists(path)){
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(content);
                }
            }

            if (exec == "true"){
                res = execCode(execCommand);
            }

            if (temp == "true"){
                // delete(file)
            }
            return res;
        }
    }
}  