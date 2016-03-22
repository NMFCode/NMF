using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Analysis
{
    public class HolmConnectivity<T> : Connectivity<T>
    {
        internal class TopTreeNode
        {
            public TopTreeNode Parent;
            public TopTreeNode LeftChild;
            public TopTreeNode RightChild;
            public Vertex Vertex;
            public int Count;

            public TopTreeNode FindRoot()
            {
                var current = this;
                while (current.Parent != null)
                {
                    current = current.Parent;
                }
                return current;
            }

            public void UpdateCounts()
            {
                var count = 0;
                if (LeftChild != null) count += LeftChild.Count;
                if (RightChild != null) count += RightChild.Count;
                if (Vertex != null) count += 1;
                Count = count;
            }

            public void MakeRoot()
            {
                while (Parent != null)
                {
                    var p = Parent;
                    TopTreeNode b;
                    Parent = p.Parent;
                    p.Parent = this;
                    if (Parent != null)
                    {
                        if (Parent.LeftChild == p)
                        {
                            Parent.LeftChild = this;
                        }
                        else
                        {
                            Parent.RightChild = this;
                        }
                    }
                    if (this == p.LeftChild)
                    {
                        b = p.RightChild;
                    }
                    else
                    {
                        b = p.LeftChild;
                    }
                    if (b != null)
                    {
                        b.Parent = this;
                    }
                    p.LeftChild = LeftChild;
                    if (LeftChild != null) LeftChild.Parent = p;
                    p.RightChild = RightChild;
                    if (RightChild != null) RightChild.Parent = p;
                    LeftChild = b;
                    RightChild = p;

                    p.UpdateCounts();
                    UpdateCounts();
                }
            }
        }

        internal class Vertex
        {
            public T Value;
            public TopTreeNode Node;
            public List<Edge> Incidents = new List<Edge>();
            public INotifyEnumerable<T> IncIncidents;
            public HolmConnectivity<T> holm;

            internal void HandleIncidentsChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action != NotifyCollectionChangedAction.Reset)
                {
                    if (e.OldItems != null)
                    {
                        foreach (T item in e.OldItems)
                        {
                            for (int i = Incidents.Count - 1; i >= 0; i--)
                            {
                                if (EqualityComparer<T>.Default.Equals(Incidents[i].Target.Value, item))
                                {
                                    holm.DeleteEdge(Incidents[i]);
                                    Incidents.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }
                    if (e.NewItems != null)
                    {
                        foreach (T item in e.NewItems)
                        {
                            holm.InsertEdge(this, item);
                        }
                    }
                }
                else
                {
                    for (int i = Incidents.Count - 1; i >= 0; i--)
                    {
                        holm.DeleteEdge(Incidents[i]);
                    }
                    Incidents.Clear();
                    foreach (var item in IncIncidents)
                    {
                        holm.InsertEdge(this, item);
                    }
                }
                holm.UpdateListeners();
            }
        }

        internal class Edge
        {
            public Vertex Source;
            public Vertex Target;
            public TopTreeNode Node;
            public int Count;
            public int Level;
        }

        internal class AreConnectedValue : INotifyValue<bool>
        {
            public T Source { get; private set; }
            public T Target { get; private set; }
            public HolmConnectivity<T> Holm { get; private set; }
            private bool currentValue;

            public AreConnectedValue(T source, T target, HolmConnectivity<T> holm)
            {
                Source = source;
                Target = target;
                Holm = holm;
            }

            public void Attach()
            {
                Holm.listeners.Add(this);
                Recheck();
            }

            public void Recheck()
            {
                var newValue = Holm.AreConnected(Source, Target);
                if (newValue != currentValue)
                {
                    currentValue = newValue;
                    OnValueChanged(new ValueChangedEventArgs(!newValue, newValue));
                }
            }

            public void Detach()
            {
                Holm.listeners.Remove(this);
            }

            public bool IsAttached
            {
                get { return Holm.listeners.Contains(this); }
            }

            public bool Value
            {
                get { return currentValue; }
            }

            protected virtual void OnValueChanged(ValueChangedEventArgs e)
            {
                var handler = ValueChanged;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            public event EventHandler<ValueChangedEventArgs> ValueChanged;
        }

        
        private Dictionary<T, Vertex> nodes = new Dictionary<T, Vertex>();
        private INotifyEnumerable<T> incElements;

        private HashSet<AreConnectedValue> listeners = new HashSet<AreConnectedValue>();

        public HolmConnectivity(Func<T, IEnumerableExpression<T>> incidents, bool incremental, IEnumerableExpression<T> elements)
        {
            Incidents = incidents;
            Incremental = incremental;
            AddElements(elements);
        }

        private void AddElements(IEnumerableExpression<T> elements)
        {
            if (elements != null)
            {
                if (Incremental)
                {
                    var incElements = elements.AsNotifiable();
                    foreach (var node in incElements)
                    {
                        GetOrCreate(node, true);
                    }
                    incElements.CollectionChanged += RootElementsChanged;
                    this.incElements = incElements;
                }
                else
                {
                    foreach (var node in elements)
                    {
                        GetOrCreate(node, true);
                    }
                }
            }
        }

        private void RootElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                nodes.Clear();
                if (sender == incElements)
                {
                    foreach (var node in incElements)
                    {
                        GetOrCreate(node, true);
                    }
                }
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (T oldNode in e.OldItems)
                    {
                        nodes.Remove(oldNode);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (T newNode in e.NewItems)
                    {
                        GetOrCreate(newNode, true);
                    }
                }
            }
            UpdateListeners();
        }

        public bool Incremental { get; private set; }

        public Func<T, IEnumerableExpression<T>> Incidents { get; private set; }

        private Edge InsertEdge(Vertex sourceV, T target)
        {
            var edge = new Edge();

            var targetV = GetOrCreate(target, true);
            var sourceN = sourceV.Node;
            var targetN = targetV.Node;

            ConnectNodes(edge, sourceN, targetN);

            edge.Source = sourceV;
            edge.Target = targetV;
            edge.Count += 1;
            return edge;
        }

        private void Delete(T source, T target)
        {
            Vertex sourceV;
            if (nodes.TryGetValue(source, out sourceV))
            {
                foreach (var edge in sourceV.Incidents)
                {
                    if (EqualityComparer<T>.Default.Equals(edge.Target.Value, target))
                    {
                        DeleteEdge(edge);
                        break;
                    }
                }
            }
        }

        private static void ConnectNodes(Edge edge, TopTreeNode sourceN, TopTreeNode targetN)
        {
            if (sourceN.FindRoot() == targetN.FindRoot()) return;

            sourceN.MakeRoot();
            targetN.MakeRoot();

            var edgeNode = new TopTreeNode();
            edgeNode.LeftChild = sourceN;
            edgeNode.RightChild = targetN;
            sourceN.Parent = edgeNode;
            targetN.Parent = edgeNode;

            edgeNode.Count = sourceN.Count + targetN.Count;
            edge.Node = edgeNode;
        }

        private void DeleteEdge(Edge edge)
        {
            edge.Count--;
            if (edge.Count > 0) return;

            var sourceV = edge.Source;
            var targetV = edge.Target;
            if (edge.Node != null)
            {
                var edgeNode = edge.Node;
                edgeNode.MakeRoot();
                if (edgeNode.LeftChild != null)
                {
                    edgeNode.LeftChild.Parent = null;
                    edgeNode.LeftChild = null;
                }
                if (edgeNode.RightChild != null)
                {
                    edgeNode.RightChild.Parent = null;
                    edgeNode.RightChild = null;
                }
                var tx = edge.Source.Node;
                var ty = edge.Target.Node;
                tx.MakeRoot();
                ty.MakeRoot();
                if (ty.Count < tx.Count)
                {
                    var tmp = tx;
                    tx = ty;
                    ty = tmp;
                }
                // tx.Count <= ty.Count
                var tyValues = new HashSet<T>();
                Collect(ty, tyValues);
                var replacementEdge = FindReplacement(tx, tyValues, edge.Level);
                if (replacementEdge != null)
                {
                    ConnectNodes(replacementEdge, replacementEdge.Source.Node, replacementEdge.Target.Node);
                }
            }
        }

        private Edge FindReplacement(TopTreeNode tx, HashSet<T> tyValues, int level)
        {
            if (tx.Vertex != null)
            {
                var vertex = tx.Vertex;
                if (vertex.Incidents != null)
                {
                    foreach (var edge in vertex.Incidents)
                    {
                        if (/* target.Level == level && */ edge.Node == null)
                        {
                            if (tyValues.Contains(edge.Target.Value))
                            {
                                return edge;
                            }
                            else
                            {
                                edge.Level++;
                            }
                        }
                    }
                }
            }
            if (tx.LeftChild != null)
            {
                var left = FindReplacement(tx.LeftChild, tyValues, level);
                if (left != null) return left;
            }
            if (tx.RightChild != null)
            {
                var right = FindReplacement(tx.RightChild, tyValues, level);
                if (right != null) return right;
            }
            return null;
        }

        private void Collect(TopTreeNode tx, ICollection<T> txValues)
        {
            if (tx.Vertex != null) txValues.Add(tx.Vertex.Value);
            if (tx.RightChild != null) Collect(tx.RightChild, txValues);
            if (tx.LeftChild != null) Collect(tx.LeftChild, txValues);
        }

        private Vertex GetOrCreate(T value, bool createIfNecessary)
        {
            Vertex vertex = null;
            if (!nodes.TryGetValue(value, out vertex) && createIfNecessary)
            {
                vertex = new Vertex();
                vertex.Value = value;
                vertex.holm = this;
                nodes.Add(value, vertex);

                var node = new TopTreeNode();
                node.Count = 1;
                node.Vertex = vertex;
                vertex.Node = node;
                var targets = Incidents(value);
                if (targets != null)
                {
                    if (Incremental)
                    {
                        var incTargets = targets.AsNotifiable();
                        vertex.IncIncidents = incTargets;
                        foreach (var target in incTargets)
                        {
                            if (target != null)
                            {
                                vertex.Incidents.Add(InsertEdge(vertex, target));
                            }
                        }
                        incTargets.CollectionChanged += vertex.HandleIncidentsChanged;
                        UpdateListeners();
                    }
                    else
                    {
                        foreach (var target in targets)
                        {
                            if (target != null)
                            {
                                vertex.Incidents.Add(InsertEdge(vertex, target));
                            }
                        }
                    }
                }
            }
            return vertex;
        }

        private void UpdateListeners()
        {
            foreach (var listener in listeners)
            {
                listener.Recheck();
            }
        }

        public override bool AreConnected(T source, T target)
        {
            var sourceV = GetOrCreate(source, false);
            var targetV = GetOrCreate(target, false);

            if (sourceV == null || targetV == null)
            {
                return false;
            }

            return sourceV.Node.FindRoot() == targetV.Node.FindRoot();
        }

        protected override INotifyValue<bool> AreConnectedInc(T a, T b)
        {
            var incValue = new AreConnectedValue(a, b, this);
            incValue.Attach();
            return incValue;
        }
    }
}
