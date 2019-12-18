using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Ceasar
{
    class Program
    {
        static void Main(string[] args)
        {
            string html = string.Empty;
            string url = @"https://api.codenation.dev/v1/challenge/dev-ps/generate-data?token=";
            string token = string.Empty;
            string fileName = "answer.json";
            string targetPath = @"C:\TEMP\";

            token = Console.ReadLine();

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

            string decripted = Utilities.DecriptMessage(int.Parse(dossie.numero_casas), dossie.cifrado.ToLower());
            Console.WriteLine(decripted);
            dossie.decifrado = decripted;

            string hash = Utilities.Hash(dossie.decifrado);
            Console.WriteLine(hash);
            dossie.resumo_criptografico = hash;

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
                    if (alphabetIndex - keyNumber < 0)
                        alphabetIndex = (alphabetIndex - keyNumber) + 26;
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

        public static string Hash(string input)
        {
            byte[] hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }
    }
}
