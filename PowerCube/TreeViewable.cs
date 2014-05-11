using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PowerCube
{
    public interface ITreeViewable<T>
    {
        string Name { get; }
        bool Expandable { get; }
        List<ITreeViewable<T>> Nodes { get; }
        TreeViewItem ToTreeViewItem();
    }

    public abstract class TreeViewableNode<T> : ITreeViewable<T>
    {
        public abstract List<ITreeViewable<T>> Nodes { get; }
        public abstract string Name { get; }
        public abstract bool Expandable { get; }
        public TreeViewItem ToTreeViewItem()
        {
            var item = new TreeViewItem();

            StackPanel pan = new StackPanel();
            pan.Orientation = Orientation.Horizontal;
//            pan.Children.Add(new TextBlock(new Run("Header")));
            pan.Children.Add(new TextBlock(new Run( Name )));
            item.Header = pan;

            if (Expandable) item.Items.Add(new TreeViewItem());
            item.DataContext = this;
            item.Expanded += (sender, e) =>
            {
                var parent = (TreeViewItem)sender;
                var node = parent.DataContext as TreeViewableNode<T>;
                if (node == null)
                    return; // if we have already transfered the nodes onto the items of the parent

                // transfer nodes onto the items of the parent
                parent.Items.Clear();
                foreach (var subnode in node.Nodes)
                    parent.Items.Add(subnode.ToTreeViewItem());
                parent.DataContext = null;
            };

            return item;
        }
    }
}
