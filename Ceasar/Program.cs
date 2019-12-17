using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;

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

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url+token);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            StreamWriter streamWriter = File.AppendText(targetPath+fileName);
            streamWriter.Write(html);
            streamWriter.Close();

            Console.WriteLine($"File copied to {targetPath}");
            Console.ReadKey();
        }
    }
}
