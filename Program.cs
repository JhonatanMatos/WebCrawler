using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string urlPage = "https://github.com/JhonatanMatos/WebCrawlere";
            int tamanhoPalavra = 4;
            string codigoHtml = PageHtmlCode(urlPage);
            List<string> wordList = formatCode(codigoHtml, tamanhoPalavra);
            WriteWordList(wordList);

            Console.ReadLine();
        }
        public static String PageHtmlCode(string Url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(),
                System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            return result;
        }
        public static List<string> formatCode(string code, int tamanho)
        {
            //code.Split('\r\n');
            string[] separators = { "\r\n" };
            var lines = code.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //new List<string>();
            List<string> wordList = new List<string>();
            foreach (string element in lines)
            {
                string element_trim;
                //element_trim = Regex.Replace(element.Trim(), "<[^>]*>", string.Empty);
                element_trim = StripHtml(element.Trim());
                if (element_trim.Length > 0)
                {
                    foreach (string palavra in element_trim.Split(' '))
                    {
                        //string palavra_trim;
                        //palavra_trim = Regex.Replace(palavra, "^![a-zA-Z]", string.Empty);
                        if (palavra.Length >= tamanho && !Regex.IsMatch(palavra, @"\W"))
                        {
                            //Spalavra_trim = Regex.Replace(palavra.Trim(), @"\W", "");
                            if (!wordList.Contains(palavra))
                                wordList.Add(palavra);
                        }
                    }
                }
            }
            return wordList;
        }
        public static String StripHtml(string source)
        {
            string output;
            output = Regex.Replace(source, "<[^>]*>", string.Empty);
            output = Regex.Replace(output, @"^\s*$\n", string.Empty, RegexOptions.Multiline);
            return output;
        }
        public static void WriteWordList(List<string> wordList)
        {
            try
            {
                if (File.Exists("wordList.txt"))
                    File.Delete("wordList.txt");
                StreamWriter sw = new StreamWriter("wordList.txt", true, Encoding.UTF8);

                foreach (string element in wordList)
                {
                    sw.WriteLine(element);
                    Console.WriteLine(element);
                }
                sw.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao criar WordList: " + e.Message);

            }
            finally
            {
                Console.WriteLine("WordList criada com sucesso!!");

            }

        }
    }
}
