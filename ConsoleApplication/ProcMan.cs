using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Task
    {
        public string Name;
        public List<Task> Dependencies;
        public int Duration;
        public Task(string name, int duration,List<Task> dependencies) {
            Name = name;
            Duration = duration;
            Dependencies = dependencies;
        }

        public override string ToString()
        {
            var depsNames = String.Join(",", Dependencies.Select(t => t.Name));
            return String.Format("{0} > {1}", Name, depsNames );
        }

    }

    public class Resource
    {
        public string Name;
        public Resource(string name)
        {
            Name = name;
        }
        public void RunTask(Task task) {

        }
    }

    public class ProcMan
    {
        public List<Task> Tasks;
        public List<Resource> Resources;
        public ProcMan()
        {
            Tasks = new List<Task>();
        }

        public void AddTask(Task task) {
            Tasks.Add(task);
        }
    }


    public static class Test
    {
        public static void Run()
        {
            var procMan = new ProcMan();
            int tasks = 10;
            var random = new Random();
            for (int i = 0; i < tasks; i++)
            {
                var deps = procMan.Tasks
                    .Where(t => random.NextDouble() < 0.3)
                    .ToList();
                var task = new Task("Task#" + i.ToString(), 5, deps);
                Console.WriteLine(task.ToString());
                procMan.AddTask(task);
            }
        }
    }
}
