using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace TmxToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                Console.WriteLine("Usage: input.tmx output.json");
            }
            else
            {
                new Program(args[0], args[1]);
            }
        }

        private Program(string input, string output)
        {
            TestXml(input, output);
            Console.Read();
        }

        void TestXml(string input, string output)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(input);

            string xdimension;
            string ydimension;
            Dictionary<string, string> properties = new Dictionary<string, string>();

            XmlNode mapNode = doc.DocumentElement.SelectSingleNode("/map");
            XmlNode propertiesNode = doc.DocumentElement.SelectSingleNode("/map/properties");
            XmlNode dataNode = doc.DocumentElement.SelectSingleNode("/map/layer[@name='Tiles']/data");

            xdimension = mapNode.Attributes["width"].InnerText;
            ydimension = mapNode.Attributes["height"].InnerText;

            foreach (XmlNode node in propertiesNode.ChildNodes)
            {
                properties.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
            }
            string level = dataNode.InnerText.Replace(",", "");
            level = Regex.Replace(level, @"\r\n?|\n", "");

            string json = "{ \"level\":\"" + level + "\",\"xdimension\":" + xdimension + ",\"ydimension\":" + ydimension + ",";

            foreach (KeyValuePair<string, string> entry in properties)
            {
                json += "\"" + entry.Key + "\":" + entry.Value;
                if (!properties[entry.Key].Equals(properties.Last().Value))
                {
                    json += ",";
                }
            }
            json += "}";

            Console.WriteLine("Writing " + json);
            Console.WriteLine("To " + output);
            File.WriteAllText(output, json);
        }


    }

}