using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DocSign
{
    class FileSender
    {
        protected string filePath;

        public FileSender(string tempFilePath)
        {
            filePath = tempFilePath;
        }

        public string getFullContent()
        {
            List<string> headers = getHeaders();
            string fileContent = getBodyContent();
            string headersLine = String.Join("\n", headers);

            string result = headersLine + "\n\n" + fileContent;

            return result;
        }

        protected List<string> getHeaders()
        {
            List<string> headers = new List<string>();

            long fileSize = new FileInfo(filePath).Length;

            headers.Add("Content-Type: text/html");
            headers.Add("Content-Description: File Transfer");
            headers.Add("Content-Disposition: attachment; filename=Соглашение.docx");
            headers.Add("Content-Transfer-Encoding: binary");
            headers.Add("Expires: 0");
            headers.Add("Cache-Control: must-revalidate, post-check=0, pre-check=0");
            headers.Add("Pragma: public");
            headers.Add("Content-Length: " + fileSize);

            return headers;
        }

        protected string getBodyContent()
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            string content = System.Text.Encoding.Default.GetString(bytes);

            return content;
        }
    }
}
