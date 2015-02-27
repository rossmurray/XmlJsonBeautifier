using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XmlJsonBeautifier
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var text = GetClipboardText();
            if (text == "")
            {
                return;
            }
            var format = DetermineTextFormat(text);
            if(format == TextFormat.Xml)
            {
                SetClipboardText(BeautifyXml(text));
                return;
            }
            else if(format == TextFormat.Json)
            {
                SetClipboardText(BeautifyJson(text));
                return;
            }
            else if (Clipboard.ContainsText(TextDataFormat.Html) || Clipboard.ContainsText(TextDataFormat.Rtf))
            {
                SetClipboardText(text);
            }
        }

        private static string BeautifyXml(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                return xml;
            }
        }

        private static string BeautifyJson(string json)
        {
            //this could be improved with something custom
            var o = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }

        private static TextFormat DetermineTextFormat(string s)
        {
            var trimmed = s.TrimStart();
            if(s.Length < 1)
            {
                return TextFormat.Unknown;
            }
            var firstChar = trimmed.Substring(0, 1);
            if(firstChar == "{" || firstChar == "[")
            {
                return TextFormat.Json;
            }
            if(firstChar == "<")
            {
                return TextFormat.Xml;
            }
            return TextFormat.Unknown;
        }

        private static string GetClipboardText()
        {
            if(Clipboard.ContainsText(TextDataFormat.Html))
            {

            }
            if (Clipboard.ContainsData(DataFormats.Text))
            {
                try
                {
                    return Clipboard.GetText();
                }
                catch (Exception) { }
            }
            return string.Empty;
        }

        private static void SetClipboardText(string s)
        {
            Clipboard.SetText(s);
        }
    }

    public enum TextFormat
    {
        Xml,
        Json,
        Unknown
    }
}
