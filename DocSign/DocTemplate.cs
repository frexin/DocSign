using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Novacode;
using System.IO;

namespace DocSign
{
    class DocTemplate
    {
        protected Dictionary<string, string> postData;
        protected string[] monthsNames;

        public DocTemplate(string rawData)
        {
            postData = DecodePostData(rawData);
            monthsNames = new string[] { "января", "февраля", "марта", "апреля", "мая", "июня", "июля",
            "августа", "сентября", "октября", "ноября", "декабря"};
        }

        public Dictionary<string, string> GetPostData()
        {
            return postData;
        }

        public Dictionary<string, string> GetDocDict()
        {
            return DecodeDocSubstitutions();
        }

        public string ProcessFile(string templatePath)
        {
            Dictionary<string, string> replaces = GetDocDict();

            string monthString = replaces["%MON%"];
            int monthIndex = Int32.Parse(monthString) - 1;
            string monthName = monthsNames[monthIndex];

            replaces["%MON%"] = monthName;

            var doc = DocX.Load(templatePath);

            foreach (KeyValuePair<string, string> entry in replaces)
            {
                doc.ReplaceText(entry.Key, entry.Value);
            }

            string tempFileName = GenerateTempFilePath();
            doc.SaveAs(tempFileName);

            return tempFileName;
        }

        private Dictionary<string, string> DecodePostData(string postData)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            char delimiter = '&';
            string[] parts = postData.Split(delimiter);

            foreach (string part in parts)
            {
                delimiter = '=';
                string[] keyval = part.Split(delimiter);

                result.Add(keyval[0], keyval[1]);
            }

            return result;
        }

        private Dictionary<string, string> DecodeDocSubstitutions()
        {
            Dictionary <string, string> result = new Dictionary<string, string>();

            if (postData.ContainsKey("template"))
            {
                string rawJson = postData["template"];
                result = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawJson);
            }

            return result;
        }

        private string GenerateTempFilePath()
        {
            string randomString = Guid.NewGuid().ToString().Substring(0, 8);
            string tempPath = Path.GetTempPath();

            string result = tempPath + "\\" + randomString + ".docx";

            return result;
        }
    }
}
