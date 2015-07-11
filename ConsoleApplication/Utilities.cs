using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApplication
{
    public static class Globals
    {
        private static Dictionary<string, object> _dictionary = new Dictionary<string, object>();
        public static object Get(string key)
        {
            return _dictionary[key];
        }

        public static void Set(string key, object value)
        {
            _dictionary[key] = value;
        }
    }

    public static class Extensions
    {
        private static string DumpFileName = "";
        private static string DumpOutput;
        /// <summary> 
        /// Dumps to a temporary html file and opens in the browser. 
        /// </summary> 
        /// <param name="o">The object to display.</param> 
        public static void Dump2<T>(this T o, bool reset = false)
        {
            if (reset || DumpFileName == "")
            {
                DumpFileName = Path.GetTempFileName() + ".html";
                DumpOutput = "";
            }

            using (var writer = LINQPad.Util.CreateXhtmlWriter(true))
            {
                writer.Write(o);
                DumpOutput = DumpOutput + writer.ToString();
                File.WriteAllText(DumpFileName, DumpOutput);
            }
            Process.Start(DumpFileName);
        }

        public static string Print<T>(this T o)
        {
            var writer = LINQPad.Util.CreateXhtmlWriter();
            writer.Write(o);
            return writer.ToString();
        }

        public static void DumpToJSON(this object obj, string fileName)
        {
            var file = new StreamWriter(fileName);
            //ObjectDumper.Dumper.Dump(model, "Model", file);
            var json = new JavaScriptSerializer().Serialize(obj);
            file.Write(json);
            file.Close();
            Process.Start(fileName);
        }

    }

    public static class Utilities
    {
        public static void Save<T>(this T obj, string filename)
        {
            //Console.WriteLine("Writing With XmlTextWriter");

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            // Create an XmlTextWriter using a FileStream.
            Stream fs = new FileStream(filename, FileMode.Create);
            XmlWriter writer = new XmlTextWriter(fs, Encoding.Unicode);
            // Serialize using the XmlTextWriter.
            serializer.Serialize(writer, obj);
            writer.Close();
        }

        public static T Load<T>(string filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader reader = new StreamReader(filename);
            object obj = deserializer.Deserialize(reader);
            reader.Close();
            return (T)obj;
        }
    }
}
