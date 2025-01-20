using NMF.Expressions;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Analyses
{
    /// <summary>
    /// Denotes an implementation of Holm's incremental connectivity algorithm
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HolmConnectivity<T> : Connectivity<T>, INotifiable
    {
        internal class NoArgResult : INotificationResult
        {
            public INotifiable Source { get { return null; } }

            public bool Changed { get { return true; } }

            public static readonly NoArgResult Instance = new NoArgResult();

            public void IncreaseReferences(int references) { }

            public void FreeReference() { }
        }
        
        internal class TopTreeNode
        {
            private TopTreeNode parent;
            private TopTreeNode left;
            private TopTreeNode right;
            private TopTreeNode next;
            private TopTreeNode prev;
            private readonly IEulerNode value;
            private int count;
            private double priority;

            private static readonly Random random = new Random();

            private TopTreeNode(IEulerNode value, double priority, TopTreeNode parent, TopTreeNode left, TopTreeNode right, TopTreeNode next, TopTreeNode prev)
            {
                this.value = value;
                this.count = value != null && value.IsVertex ? 1 : 0;
                this.priority = priority;
                this.parent = parent;
                this.left = left;
                this.right = right;
                this.next = next;
                this.prev = prev;
            }

            public int Count { get { return count; } }

            public TopTreeNode(IEulerNode value) : this(value, random.NextDouble(), null, null, null, null, null) { }

            public TopTreeNode Root()
            {
                var current = this;
                while (current.parent != null)
                {
                    current = current.parent;
                }
                return current;
            }

            public TopTreeNode First()
            {
                var l = Root();
                while (l.left != null)
                {
                    l = l.left;
                }
                return l;
            }

            public TopTreeNode Last()
            {
                var r = Root();
                while (r.right != null)
                {
                    r = r.right;
                }
                return r;
            }

            public EulerHalfEdge FindReplacement(HashSet<T> values, int level)
            {
                if (value != null && value.IsVertex)
                {
                    var vertex = (EulerVertex)value;
                    if (TryFindReplacement(values, vertex, out var edge))
                    {
                        return edge;
                    }
                }
                if (left != null)
                {
                    var leftRepl = left.FindReplacement(values, level);
                    if (leftRepl != null) return leftRepl;
                }
                if (right != null)
                {
                    var rightRepl = right.FindReplacement(values, level);
                    if (rightRepl != null) return rightRepl;
                }
                return null;
            }

            private static bool TryFindReplacement(HashSet<T> values, EulerVertex vertex, out EulerHalfEdge result)
            {
                if (vertex.Incidents != null)
                {
                    foreach (var edge in vertex.Incidents.Values)
                    {
                        if (/*edge.Level <= level &&*/ edge.node == null)
                        {
                            if (values.Contains(edge.Target.value))
                            {
                                result = edge;
                                return true;
                            }
                            else
                            {
                                edge.Level++;
                                edge.opposite.Level++;
                            }
                        }
                    }
                }
                result = null;
                return false;
            }

            public void Collect(ICollection<T> values)
            {
                if (value != null && value.IsVertex) values.Add(((EulerVertex)value).value);
                if (right != null) right.Collect(values);
                if (left != null) left.Collect(values);
            }

            public TopTreeNode Insert(IEulerNode value)
            {
                if (right == null)
                {
                    var nn = right = new TopTreeNode(value, random.NextDouble(), this, null, null, this.next, this);
                    if (next != null)
                    {
                        this.next.prev = nn;
                    }
                    next = nn;
                    nn.BubbleUp();
                    return nn;
                }
                else
                {
                    var v = next;
                    var nn = v.left = new TopTreeNode(value, random.NextDouble(), v, null, null, v, this);
                    v.prev = nn;
                    next = nn;
                    nn.BubbleUp();
                    return nn;
                }
            }

            private void BubbleUp()
            {
                while (true)
                {
                    var p = parent;
                    if (p == null || p.priority < priority)
                    {
                        break;
                    }
                    if (this == p.left)
                    {
                        var b = right;
                        p.left = b;
                        if (b != null)
                        {
                            b.parent = p;
                        }
                        right = p;
                    }
                    else
                    {
                        var b = left;
                        p.right = b;
                        if (b != null)
                        {
                            b.parent = p;
                        }
                        left = p;
                    }
                    p.Update();
                    Update();
                    var gp = p.parent;
                    p.parent = this;
                    parent = gp;
                    if (gp != null)
                    {
                        if (gp.left == p)
                        {
                            gp.left = this;
                        }
                        else
                        {
                            gp.right = this;
                        }
                    }
                }
                var p2 = parent;
                while (p2 != null)
                {
                    p2.Update();
                    p2 = p2.parent;
                }
            }

            private static void SwapNodes(TopTreeNode a, TopTreeNode b)
            {
                var p = a.priority;
                a.priority = b.priority;
                b.priority = p;
                var t = a.parent;
                a.parent = b.parent;
                if (b.parent != null)
                {
                    if (b.parent.left == b)
                    {
                        b.parent.left = a;
                    }
                    else
                    {
                        b.parent.right = a;
                    }
                }
                b.parent = t;
                if (t != null)
                {
                    if (t.left == a)
                    {
                        t.left = b;
                    }
                    else
                    {
                        t.right = b;
                    }
                }
                t = a.left;
                a.left = b.left;
                if (b.left != null)
                {
                    b.left.parent = a;
                }
                b.left = t;
                if (t != null)
                {
                    t.parent = b;
                }
                t = a.right;
                a.right = b.right;
                if (b.right != null)
                {
                    b.right.parent = a;
                }
                b.right = t;
                if (t != null)
                {
                    t.parent = b;
                }
                t = a.next;
                a.next = b.next;
                if (b.next != null)
                {
                    b.next.prev = a;
                }
                b.next = t;
                if (t != null)
                {
                    t.prev = b;
                }
                t = a.prev;
                a.prev = b.prev;
                if (b.prev != null)
                {
                    b.prev.next = a;
                }
                b.prev = t;
                if (t != null)
                {
                    t.next = b;
                }
                var c = a.count;
                a.count = b.count;
                b.count = c;
            }

            public void Update()
            {
                var c = value != null ? 1 : 0;
                if (left != null)
                {
                    c += left.count;
                }
                if (right != null)
                {
                    c += right.count;
                }
                count = c;
            }

            public void Remove()
            {
                var node = this;
                if (node.left != null && node.right != null)
                {
                    var other = node.next;
                    SwapNodes(other, node);
                }
                if (node.next != null)
                {
                    node.next.prev = node.prev;
                }
                if (node.prev != null)
                {
                    node.prev.next = node.next;
                }
                TopTreeNode r = null;
                if (node.left != null)
                {
                    r = node.left;
                }
                else
                {
                    r = node.right;
                }
                if (r != null)
                {
                    r.parent = node.parent;
                }
                if (node.parent != null)
                {
                    if (node.parent.left == node)
                    {
                        node.parent.left = r;
                    }
                    else
                    {
                        node.parent.right = r;
                    }
                    //Update all ancestor counts
                    var p = node.parent;
                    while (p != null)
                    {
                        p.Update();
                        p = p.parent;
                    }
                }
                //Remove all pointers from detached node
                node.parent = node.left = node.right = node.prev = node.next = null;
                node.count = 1;
            }

            public TopTreeNode Split()
            {
                var node = this;
                var s = node.Insert(null);
                s.priority = double.NegativeInfinity;
                s.BubbleUp();
                var l = s.left;
                var r = s.right;
                if (l != null)
                {
                    l.parent = null;
                }
                if (r != null)
                {
                    r.parent = null;
                }
                if (s.prev != null)
                {
                    s.prev.next = null;
                }
                if (s.next != null)
                {
                    s.next.prev = null;
                }
                return r;
            }

            private static TopTreeNode ConcatRecurse(TopTreeNode a, TopTreeNode b)
            {
                if (a == b)
                {
                    throw new InvalidOperationException();
                }
                if (a == null)
                {
                    return b;
                }
                else if (b == null)
                {
                    return a;
                }
                else if (a.priority < b.priority)
                {
                    a.right = ConcatRecurse(a.right, b);
                    a.right.parent = a;
                    a.Update();
                    return a;
                }
                else
                {
                    b.left = ConcatRecurse(a, b.left);
                    b.left.parent = b;
                    b.Update();
                    return b;
                }
            }

            public TopTreeNode Concat(TopTreeNode other)
            {
                if (other == null)
                {
                    return null;
                }
                var ra = this.Root();
                var ta = ra;
                while (ta.right != null)
                {
                    ta = ta.right;
                }
                var rb = other.Root();
                var sb = rb;
                while (sb.left != null)
                {
                    sb = sb.left;
                }
                ta.next = sb;
                sb.prev = ta;
                var r = ConcatRecurse(ra, rb);
                r.parent = null;
                return r;
            }
        }

        internal interface IEulerNode
        {
            bool IsVertex { get; }
        }

        internal class EulerVertex : Notifiable, IEulerNode
        {
            public readonly T value;
            public readonly TopTreeNode node;
            public readonly Dictionary<T, EulerHalfEdge> Incidents = new Dictionary<T, EulerHalfEdge>();
            public INotifyEnumerable<T> IncIncidents;
            public readonly HolmConnectivity<T> holm;

            public EulerVertex(T value, HolmConnectivity<T> holm)
            {
                this.value = value;
                node = new TopTreeNode(this);
                this.holm = holm;
            }

            public override IEnumerable<INotifiable> Dependencies
            {
                get
                {
                    yield return IncIncidents;
                }
            }

            public bool IsVertex => true;

            public override INotificationResult Notify(IList<INotificationResult> sources)
            {
                foreach (var change in sources.Cast<ICollectionChangedNotificationResult>())
                {
                    HandleIncidentsChanged(change);
                }
                return NoArgResult.Instance;
            }

            public bool AreConnected(EulerVertex other)
            {
                return this.node.Root() == other.node.Root();
            }

            public void MakeRoot()
            {
                var a = this.node;
                var b = a.Split();
                if (b != null)
                {
                    b.Concat(a);
                }
            }

            public EulerHalfEdge Link(EulerVertex other)
            {

                //Create half edges and link them to each other
                var st = new EulerHalfEdge(this, other, null, null);
                var ts = new EulerHalfEdge(other, this, null, st);
                st.opposite = ts;

                // check whether the nodes are
                if (!this.AreConnected(other))
                {
                    //Move both vertices to root
                    this.MakeRoot();
                    other.MakeRoot();

                    //Insert entries in Euler tours
                    st.node = this.node.Insert(st);
                    ts.node = other.node.Insert(ts);

                    //Link tours together
                    this.node.Concat(other.node);
                }
                //Return half edge
                return st;
            }

            internal void HandleIncidentsChanged(ICollectionChangedNotificationResult e)
            {
                if (!e.IsReset)
                {
                    if (e.RemovedItems != null)
                    {
                        foreach (T item in e.RemovedItems)
                        {
                            RemoveEdgeTo(item);
                        }
                    }
                    if (e.AddedItems != null)
                    {
                        lock (holm)
                        {
                            foreach (T item in e.AddedItems)
                            {
                                AddEdgeTo(item);
                            }
                        }
                    }
                }
                else
                {
                    lock (holm)
                    {
                        foreach (var edge in Incidents.Values)
                        {
                            edge.Count--;
                            edge.opposite.Count--;
                            if (edge.Count == 0)
                            {
                                edge.Cut();
                                edge.Target.Incidents.Remove(value);
                            }
                        }
                        Incidents.Clear();
                        foreach (var item in IncIncidents)
                        {
                            AddEdgeTo(item);
                        }
                    }
                }
            }

            private void RemoveEdgeTo(T item)
            {
                var edge = Incidents[item];
                edge.Count--;
                edge.opposite.Count--;
                if (edge.Count == 0)
                {
                    edge.Cut();
                    edge.Target.Incidents.Remove(value);
                    Incidents.Remove(item);
                }
            }

            public void AddEdgeTo(T item)
            {
                EulerHalfEdge edge;
                if (Incidents.TryGetValue(item, out edge))
                {
                    edge.Count++;
                    edge.opposite.Count++;
                }
                else
                {
                    var target = holm.GetOrCreate(item, true);
                    if (target.Incidents.TryGetValue(value, out edge))
                    {
                        edge.Count++;
                        edge.opposite.Count++;
                    }
                    else
                    {
                        edge = Link(target);
                        Incidents.Add(item, edge);
                        target.Incidents.Add(value, edge.opposite);
                    }
                }
            }
        }

        internal class EulerHalfEdge : IEulerNode
        {
            private readonly EulerVertex source;
            private readonly EulerVertex target;
            public TopTreeNode node;
            public EulerHalfEdge opposite;

            public int Level { get; set; }

            public EulerHalfEdge(EulerVertex s, EulerVertex t, TopTreeNode node, EulerHalfEdge opposite)
            {
                if (s == null) throw new ArgumentNullException(nameof(s));
                if (t == null) throw new ArgumentNullException(nameof(t));

                this.source = s;
                this.target = t;
                this.node = node;
                this.opposite = opposite;
                this.Count = 1;
            }

            public bool IsVertex => false;

            public int Count { get; set; }

            public EulerVertex Target
            {
                get
                {
                    return target;
                }
            }

            public void Cut()
            {
                if (node == null) return;
                var other = this.opposite;

                //Split into parts
                var a = this.node;
                var b = a.Split();
                var c = other.node;
                var d = c.Split();

                //Pull out the roots
                if (d != null && a.Root() != d.Root())
                {
                    //a comes before c:
                    // [a, bc, d]
                    a.Concat(d);
                }
                else if (b != null && c.Root() != b.Root())
                {
                    //c comes before a:
                    // [c, da, b]
                    c.Concat(b);
                }

                var a_root = a.Root();
                var b_root = c.Root();
                
                if (b_root.Count < a_root.Count)
                {
                    var tmp = a_root;
                    a_root = b_root;
                    b_root = tmp;
                }

                var values = new HashSet<T>();
                a_root.Collect(values);
                var replacement = b_root.FindReplacement(values, Level);
                if (replacement != null)
                {
                    var replaceS = replacement.source;
                    var replaceT = replacement.target;

                    replaceS.MakeRoot();
                    replaceT.MakeRoot();

                    replacement.node = replaceS.node.Insert(replacement);
                    replacement.opposite.node = replaceT.node.Insert(replacement.opposite);

                    replaceS.node.Concat(replaceT.node);
                }
            }
        }

        internal class AreConnectedValue : NotifyExpression<bool>
        {
            public T Source { get; private set; }
            public T Target { get; private set; }

            public HolmConnectivity<T> Holm { get; private set; }

            public override bool IsParameterFree { get { return true; } }

            public override IEnumerable<INotifiable> Dependencies { get { yield return Holm; } }

            public AreConnectedValue(T source, T target, HolmConnectivity<T> holm)
            {
                Source = source;
                Target = target;
                Holm = holm;
            }

            protected override bool GetValue()
            {
                return Holm.AreConnected(Source, Target);
            }

            protected override INotifyExpression<bool> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
            {
                return this;
            }
        }
        
        private readonly Dictionary<T, EulerVertex> nodes = new Dictionary<T, EulerVertex>();
        private INotifyEnumerable<T> incrementalElements;
        private readonly ExecutionMetaData metadata = new ExecutionMetaData();
        private readonly ISuccessorList successors = new MultiSuccessorList();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="incidents">A function returning incident nodes</param>
        /// <param name="incremental">A flag determing whether the algorithms runs incremental</param>
        /// <param name="elements">A collection of nodes</param>
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
                        nodes.Add(node, new EulerVertex(node, this));
                    }
                    foreach (var vertex in nodes.Values)
                    {
                        CreateEdgesFor(vertex);
                    }
                    incElements.Successors.Set(this);
                    incrementalElements = incElements;
                }
                else
                {
                    foreach (var node in elements)
                    {
                        nodes.Add(node, new EulerVertex(node, this));
                    }
                    foreach (var vertex in nodes.Values)
                    {
                        CreateEdgesFor(vertex);
                    }
                }
            }
        }

        private void RootElementsChanged(ICollectionChangedNotificationResult e)
        {
            if (e.IsReset)
            {
                nodes.Clear();
                foreach (var node in incrementalElements)
                {
                    GetOrCreate(node, true);
                }
            }
            else
            {
                if (e.RemovedItems != null)
                {
                    // ignore for now
                }
                if (e.AddedItems != null)
                {
                    foreach (T newNode in e.AddedItems)
                    {
                        GetOrCreate(newNode, true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance runs incremental
        /// </summary>
        public bool Incremental { get; private set; }

        /// <summary>
        /// Gets a function returning incident nodes
        /// </summary>
        public Func<T, IEnumerableExpression<T>> Incidents { get; private set; }

        /// <inheritdoc />
        public ISuccessorList Successors { get { return successors; } }

        /// <inheritdoc />
        public IEnumerable<INotifiable> Dependencies
        {
            get
            {
                IEnumerable<INotifiable> results = nodes.Values;
                if (incrementalElements != null)
                {
                    results = results.Concat(incrementalElements);
                }
                return results;
            }
        }

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData { get { return metadata; } }

        private EulerVertex GetOrCreate(T value, bool createIfNecessary)
        {
            EulerVertex vertex;
            if (!nodes.TryGetValue(value, out vertex) && createIfNecessary)
            {
                vertex = new EulerVertex(value, this);
                nodes.Add(value, vertex);

                CreateEdgesFor(vertex);
            }
            return vertex;
        }

        private void CreateEdgesFor(EulerVertex vertex)
        {
            var targets = Incidents(vertex.value);
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
                            vertex.AddEdgeTo(target);
                        }
                    }
                    incTargets.Successors.Set(vertex);
                    vertex.Successors.Set(this);
                }
                else
                {
                    foreach (var target in targets)
                    {
                        if (target != null)
                        {
                            vertex.AddEdgeTo(target);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override bool AreConnected(T source, T target)
        {
            var sourceV = GetOrCreate(source, false);
            var targetV = GetOrCreate(target, false);

            if (sourceV == null || targetV == null)
            {
                return false;
            }

            return sourceV.node.Root() == targetV.node.Root();
        }

        /// <inheritdoc />
        protected override INotifyValue<bool> AreConnectedInc(T source, T target)
        {
            var incValue = new AreConnectedValue(source, target, this);
            incValue.Successors.SetDummy();
            return incValue;
        }

        /// <inheritdoc />
        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            foreach (var change in sources)
            {
                if (change.Source == incrementalElements)
                {
                    RootElementsChanged((ICollectionChangedNotificationResult)change);
                }
            }
            return NoArgResult.Instance;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Successors.UnsetAll();
        }
    }
}
