using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Novacode;
using FastCGI;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace DocSign
{
    class Program
    {
        static void Main(string[] args)
        {
            string rawData = Console.ReadLine();
            rawData = WebUtility.UrlDecode(rawData);
            DocTemplate docTemplate = new DocTemplate(rawData);

            Dictionary<string, string> docDict = docTemplate.GetDocDict();
            Dictionary<string, string> postData = docTemplate.GetPostData();

            string filePath = docTemplate.ProcessFile(postData["path"]);
            FileSender fileSender = new FileSender(filePath);
            string output = fileSender.getFullContent();

            Console.Write(output);
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
