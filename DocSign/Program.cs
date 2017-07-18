using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Novacode;
using FastCGI;

namespace DocSign
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Content-type: text/html\n\n"); // support CGI mode

            string rawData = Console.ReadLine();
            DocTemplate docTemplate = new DocTemplate(rawData);
            Dictionary <string, string> docDict = docTemplate.GetDocDict();
            Dictionary<string, string> postData = docTemplate.GetPostData();

            foreach (KeyValuePair<string, string> entry in postData)
            {
                Console.WriteLine("Key = {0}, Value = {1}", entry.Key, entry.Value);
            }

            foreach (KeyValuePair<string, string> docPart in docDict)
            {
                Console.WriteLine("DocKey = {0}, DocValue = {1}", docPart.Key, docPart.Value);
            }

            string filePath = docTemplate.ProcessFile(postData["path"]);
            Console.WriteLine("File Path: " + filePath);
        }

        private static void App_OnRequestReceived(object sender, Request request)
        {
            var responseString =
                  "HTTP/1.1 200 OK\n"
                + "Content-Type:text/html\n"
                + "\n"
                + "Hello World!";

            request.WriteResponseASCII(responseString);
            request.Close();
        }

        public void TemplateDoc(string[] args)
        {
            string templateFileName = @"D:\docs\sogl_template.docx";
            string outputFileName = @"D:\docs\out.docx";
            string clientName = args[0];

            var doc = DocX.Load(templateFileName);
            doc.ReplaceText("%CLIENT%", clientName);
            doc.SaveAs(outputFileName);
        }
    }
}
