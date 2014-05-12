﻿using System;
using System.Text.RegularExpressions;

namespace JHLib.Utils
{
    public class URLParser
    {
        public const string PROTOCOL = "finom";

        // the various keywords, e.g. finom:bbg/somearguments
        public const string BBGParser = "bbg";

        private static Parser ParserObject(string type)
        {
            switch (type)
            {
                case BBGParser  :       return new BBGParser();
                default         :       return new Parser(type);
            }
        }

        public static void ParseURL(string url)
        {
            string pattern = String.Format(@"{0}:(.*)\/(.*)", PROTOCOL);
            Match match = Regex.Match(url,pattern);
            if (match.Success)
            {
                string type    = match.Groups[1].Value;
                string command = match.Groups[2].Value;
                DispatchCommand(type, command);
            }
            else
                Console.WriteLine("Failed parsing " + url );
        }

        private static void DispatchCommand(string type, string command)
        {
            Parser parser = ParserObject(type);
            parser.RunCommand(command);
        }
    }

    public class Parser
    {
        string _type;
        public Parser() { }
        public Parser(string type) { _type = type; }

        public virtual void RunCommand(string command)
        {
            Console.WriteLine("No action known for parser of type '{0}'", _type);
        }
    }

    public class BBGParser : Parser
    {
        public override void RunCommand(string command)
        {
            Console.WriteLine("Running BBGParser {0}", command);                
        }
    }

}