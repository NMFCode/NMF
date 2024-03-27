namespace NMF.Expressions
{
    /// <summary>
    /// Denotes an interface for a list of subsequent dependency graph nodes
    /// </summary>
    public interface ISuccessorList
    {
        /// <summary>
        /// The number of elements
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the successor at the given index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The DDG node with the given index</returns>
        INotifiable GetSuccessor(int index);
        
        /// <summary>
        /// True, if there is any successor, otherwise False
        /// </summary>
        bool HasSuccessors { get; }

        /// <summary>
        /// True, if successors are attached, otherwise False
        /// </summary>
        bool IsAttached { get; }

        /// <summary>
        /// Add the given DDG node to the list
        /// </summary>
        /// <param name="node">The DDG node to add</param>
        void Set(INotifiable node);

        /// <summary>
        /// Sets a dummy
        /// </summary>
        void SetDummy();

        /// <summary>
        /// Unset the given DDG node as successor
        /// </summary>
        /// <param name="node">the DDG node</param>
        /// <param name="leaveDummy">True, to leave the dummy in operation, otherwise False</param>
        void Unset(INotifiable node, bool leaveDummy = false);

        /// <summary>
        /// Clear the list
        /// </summary>
        void UnsetAll();
    }
}
