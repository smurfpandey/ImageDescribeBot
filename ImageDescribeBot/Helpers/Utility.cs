﻿
using AngleSharp.Parser.Html;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageDescribeBot
{
    class Utility
    {
        public static string GetTextFromHtml(string htmlText)
        {
            var dom = new HtmlParser().Parse(htmlText);
            return dom.Body.TextContent;
        }

        public static async Task<byte[]> DownloadImage(string imageUri)
        {
            HttpClient client = new HttpClient();
            MemoryStream memStream = new MemoryStream();

            Stream stream = await client.GetStreamAsync(imageUri);
            await stream.CopyToAsync(memStream);

            return memStream.ToArray();
        }
    }
}