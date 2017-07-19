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
            DocumentSignature docSignature = new DocumentSignature();

            Dictionary<string, string> docDict = docTemplate.GetDocDict();
            Dictionary<string, string> postData = docTemplate.GetPostData();

            string filePath = docTemplate.ProcessFile(postData["path"]);
            string signatureSN = postData["ssn"];

            docSignature.AddSignature(filePath, signatureSN);

            FileSender fileSender = new FileSender(filePath);
            string output = fileSender.getFullContent();

            Console.Write(output);
        }
    }
}
