using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PowerCube
{
    public abstract class FileSystemNode: TreeViewableNode<FileSystemNode>
    {
        private string _name;
        public override string Name { get { return _name; }  }
        public FileSystemNode() { }
        public FileSystemNode(string path)
        {
            _name = path;
        }
    }

    public class FileNode : FileSystemNode
    {
        public FileNode(string path) : base(path) { }
        public override List<ITreeViewable<FileSystemNode>> Nodes { get { return new List<ITreeViewable<FileSystemNode>>(); } }
        public override bool Expandable { get { return false; } }
    }

    public class DirectoryNode : FileSystemNode
    {
        public DirectoryNode(string path) : base(path) { }
        public override bool Expandable { get { return true; } }

        private List<ITreeViewable<FileSystemNode>> _nodes;
        public List<ITreeViewable<FileSystemNode>> Items { get { return Nodes; } }

        public override List<ITreeViewable<FileSystemNode>> Nodes
        {
            get
            {
                if (_nodes != null)
                    return _nodes;

                _nodes = new List<ITreeViewable<FileSystemNode>>();

                try
                {
                    Console.WriteLine("Reading " + Name);
                    var subDirs = System.IO.Directory.GetDirectories(Name);
                    foreach (string subdir in subDirs)
                    {
                        _nodes.Add(new DirectoryNode(subdir));
                    }
                }
                // An UnauthorizedAccessException exception will be thrown if we do not have 
                // discovery permission on a folder or file. It may or may not be acceptable  
                // to ignore the exception and continue enumerating the remaining files and  
                // folders. It is also possible (but unlikely) that a DirectoryNotFound exception  
                // will be raised. This will happen if currentDir has been deleted by 
                // another application or thread after our call to Directory.Exists. The  
                // choice of which exceptions to catch depends entirely on the specific task  
                // you are intending to perform and also on how much you know with certainty  
                // about the systems on which this code will run. 
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }


                string[] files = new string[0];

                try
                {
                    files = System.IO.Directory.GetFiles(Name);
                }

                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }
                // Perform the required action on each file here. 
                // Modify this block to perform your required task. 
                
                foreach (string file in files)
                {
                    try
                    {
                        // Perform whatever action is required in your scenario.
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        Console.WriteLine("{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime);
                        _nodes.Add(new FileNode(fi.Name));
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        // If file was deleted by a separate application 
                        //  or thread since the call to TraverseTree() 
                        // then just continue.
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }
                return _nodes;
            }
        }
    }
}
