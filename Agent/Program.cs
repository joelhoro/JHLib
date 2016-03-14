using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Agent
{
    public struct Job
    {
        public string Assembly;
        public string Function;
        public double Arg;
    }


    class Program
    {

        static void LoadAndExecute(Job job, string fileName)
        {
        }

        static void Main(string[] args)
        {
            var dir = @"c:\temp\agent\job1";

            var file = "task.txt";
            var fullFileName = string.Format(@"{0}\{1}", dir, file);
            var text = File.ReadAllText(fullFileName);
            var job = new JavaScriptSerializer().Deserialize<Job>(text);
            var assemblyName = string.Format(@"{0}\{1}", dir, Path.GetFileName(job.Assembly));
            var assembly = Assembly.LoadFrom(assemblyName);

            var c = 0;
            var tasks = 0;
            var running = new HashSet<string>();
            while (true)
            {
                var argFiles = Directory.EnumerateFiles(dir + @"\args", "taskargs*.txt");
                foreach (var argFile in argFiles)
                {
                    if (running.Contains(argFile))
                        continue;
                    try
                    {
                        running.Add(argFile);
                        var thread = new Thread(new ThreadStart(() =>
                        {
                            var argsText = File.ReadAllText(argFile);
                            var argsJob = new JavaScriptSerializer().Deserialize<Job>(argsText);
                            var type = assembly.GetType("ConsoleApplication.Program");
                            var fn = type.GetMethod(argsJob.Function);
                            var result = fn.Invoke(null, new object[] { argsJob.Arg });
                           // if(c++ % 100 == 0)
                                Console.WriteLine("Returned {0} for {1} [task processed: {2}", result, argsJob.Arg, tasks++);
                            //File.Move(argFile, argFile.Replace(".txt", ".done"));
                            File.Delete(argFile);
                            running.Remove(argFile);
                        }));
                        thread.Start();
                    }
                    catch (Exception) { }
                }
                Thread.Sleep(1);
                if (c++ % 1000 == 0)
                    Console.WriteLine("Waiting...");
            }

            var output = @"c:\temp\results.txt";
//            DumpToFile(x, output);


        }
    }
}
