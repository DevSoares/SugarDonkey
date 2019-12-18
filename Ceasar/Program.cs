using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ceasar
{
    class Program
    {
        static void Main(string[] args)
        {
            string html = string.Empty;
            string url = @"https://api.codenation.dev/v1/challenge/dev-ps/generate-data?token=";
            string token = "62d7bc55dd38ba9f47a64c310afb88d3dba8453e";
            string fileName = "answer.json";
            string targetPath = @"C:\TEMP\";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + token);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            StreamWriter streamWriter = File.AppendText(targetPath + fileName);
            streamWriter.Write(html);
            streamWriter.Close();

            Console.WriteLine($"File copied to {targetPath}");
            Console.ReadKey();

            Dossie dossie = JsonConvert.DeserializeObject<Dossie>(html);
            Console.WriteLine($"numero_casas: {dossie.numero_casas}, token: {dossie.token}, cifrado: {dossie.cifrado}, decifrado: {dossie.decifrado}, resumo_criptografico: {dossie.resumo_criptografico}");

            string decripted = Utilities.DecriptMessage(int.Parse(dossie.numero_casas), dossie.cifrado);
            Console.WriteLine(decripted);

            dossie.decifrado = decripted;
            html = JsonConvert.SerializeObject(dossie);
            File.WriteAllText(targetPath + fileName, html);
            Console.ReadKey();

        }
    }

    public class Dossie
    {
        public string numero_casas;
        public string token;
        public string cifrado;
        public string decifrado;
        public string resumo_criptografico;

        public Dossie()
        {
        }

        public Dossie(string numero_casas, string token, string cifrado, string decifrado, string resumo_criptografico)
        {
            this.numero_casas = numero_casas;
            this.token = token;
            this.cifrado = cifrado;
            this.decifrado = decifrado;
            this.resumo_criptografico = resumo_criptografico;
        }
    }

    public static class Utilities
    {
        public static string DecriptMessage(int keyNumber, string message)
        {
            string decriptedMessage = null;
            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            alphabet.ToCharArray();
            message.ToCharArray();

            foreach (char c in message)
            {
                if (alphabet.Contains(c))
                {
                    int alphabetIndex = alphabet.IndexOf(c);
                    if (alphabetIndex - keyNumber <= 0)
                        alphabetIndex = (alphabetIndex - keyNumber) + 25;
                    else
                        alphabetIndex -= keyNumber;

                    decriptedMessage += alphabet.Substring(alphabetIndex, 1);
                }
                else
                {
                    decriptedMessage += c;
                }
            }

            return decriptedMessage;
        }
    }
}
