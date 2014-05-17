using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JHLib.QuantLIB.Graph
{
    public interface IGraphNode
    {
        void Invalidate();
        void Calculate();
        HashSet<IGraphNode> Precedents { get; }
        HashSet<IGraphNode> Dependents { get; }
    }
    
    public struct GraphNodeID 
    {
        public object obj;
        public string tag;
    }

    public static class Graph
    {
        public static Stack<IGraphNode> Context = new Stack<IGraphNode>();

        public static Dictionary<GraphNodeID,object> Cache = new Dictionary<GraphNodeID,object>();
        
    }

    public class Graph<TnodeType>
    {
        //public GraphNodelet<TnodeType> Node(object obj,Func<TnodeType> definition, [CallerMemberName] string tag = "")
        //{
        //    var graphNodeID = new GraphNodeID { obj = obj, tag = tag };

        //    Graph.Cache[graphNodeID] = new GraphNodelet<TnodeType>(definition);
        //}

    }

    public enum NodeState { VALID, INVALID, CALCULATING }
    public class GraphNodelet<TNodeType>: IGraphNode
    {
        private TNodeType _value;
        private Func<TNodeType> _definition;
        public NodeState State { get; set; }
        public HashSet<IGraphNode> Precedents { get; set; }
        public HashSet<IGraphNode> Dependents { get; set; }
        private static int counter = 0;
        private string _name;
        public bool DebugMode { get; set; }

        private void Initialize()
        {
            Precedents = new HashSet<IGraphNode>();
            Dependents = new HashSet<IGraphNode>();
            _name = "Node[" + counter++ + "]";
            Invalidate();
        }

        public GraphNodelet(TNodeType value)
        {
            _definition = () => value;
            Initialize();
        }

        public GraphNodelet(Func<TNodeType> definition )
        {
            _definition = definition;
            Initialize();
        }

        private Func<TNodeType> Definition
        {
            get
            {
                if (DebugMode)
                    return () => { Debugger.Break(); return _definition(); };
                else
                    return _definition;
            }
        }


        private void print(string message)
        {
            String indentation = new String('>', ( Graph.Context.Count  + 1 ) * 3) + " ";
            Console.WriteLine(indentation + message);
        }

        public override string ToString()
        {
            return "GraphNode (" + _name + ")";
        }

        public void Invalidate()
        {
            print("Invalidating " + this);
            if (State == NodeState.INVALID) return;
            State = NodeState.INVALID;
            foreach (var graphnode in Dependents)
                graphnode.Invalidate();
            Precedents = new HashSet<IGraphNode>();
        }

        [DebuggerNonUserCode]
        public void Calculate()
        {
            if(Graph.Context.Count > 0)
            {
                var caller = Graph.Context.Peek();
                caller.Precedents.Add(this);
                this.Dependents.Add(caller);
            }

            if (State == NodeState.INVALID)
            {
                print("Calculating " + this);
                Graph.Context.Push(this);
                State = NodeState.CALCULATING;
                _value = Definition();
                Graph.Context.Pop();
                State = NodeState.VALID;
            }

            if (State == NodeState.CALCULATING)
            {
                print("Detected circular reference in " + this);
                return;
            }
        }

        public TNodeType V
        {
            get
            {
                Calculate();
                return _value;
            }

            set
            {
                _value = value;
            }
        }
    }
}
