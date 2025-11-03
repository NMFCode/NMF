using NMF.AnyText.Grammars;
using NMF.Models;
using NMF.Synchronizations;
using NMF.Synchronizations.Models;
using NMF.Transformations;
using System;
using System.IO;

namespace NMF.AnyText
{
    /// <summary>
    ///     Represents the abstract base class for model synchronization logic of two models with a role separation of left and right.
    /// </summary>
    public abstract class ModelSynchronization : IModelSynchronization
    {
        /// <summary>
        /// Gets the file extension to identify files of the left models
        /// </summary>
        public string LeftExtension { get; init; }

        /// <summary>
        /// Gets the file extension to identify files of the right models
        /// </summary>
        public string RightExtension { get; init; }

        /// <inheritdoc />
        public bool IsAutomatic { get; init; }

        /// <inheritdoc />
        public string Name { get; init; }

        /// <inheritdoc />
        public bool CanSynchronize(Uri uri, out Uri synchronizedUri)
        {
            var path = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
            var dotIndex = path.LastIndexOf('.');
            if (dotIndex != -1)
            {
                var extension = path.Substring(dotIndex);
                if (extension == LeftExtension)
                {
                    synchronizedUri = new Uri(path.Substring(0, dotIndex) + RightExtension, UriKind.RelativeOrAbsolute);
                    return true;
                }
                if (extension == RightExtension)
                {
                    synchronizedUri = new Uri(path.Substring(0, dotIndex) + LeftExtension, UriKind.RelativeOrAbsolute);
                    return true;
                }
            }
            synchronizedUri = null;
            return false;
        }

        /// <inheritdoc />
        public IRunningSynchronization Synchronize(Uri uri1, ref IModelElement root1, Uri uri2, ref IModelElement root2)
        {
            var path = uri1.IsAbsoluteUri ? uri1.AbsoluteUri : uri1.ToString();
            var dotIndex = path.LastIndexOf('.');
            if (dotIndex != -1)
            {
                var extension = path.Substring(dotIndex);
                if (extension == LeftExtension)
                {
                    return SynchronizeLeftToRight(uri1, ref root1, uri2, ref root2);
                }
                if (extension == RightExtension)
                {
                    return SynchronizeRightToLeft(uri2, ref root2, uri1, ref root1);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the synchronization direction
        /// </summary>
        public SynchronizationDirection Direction { get; init; } = SynchronizationDirection.LeftToRightForced;

        /// <summary>
        /// True, if the synchronization direction is inverted if the right model is opened as second
        /// </summary>
        public bool OpposeDirection { get; init; } = true;

        /// <summary>
        /// Gets the direction for right to left synchronizations
        /// </summary>
        /// <returns>the synchronization direction for reverse synchronizations</returns>
        protected SynchronizationDirection GetRightToLeftDirection()
        {
            if (!OpposeDirection)
            {
                return Direction;
            }

            switch (Direction)
            {
                case SynchronizationDirection.LeftToRight:
                    return SynchronizationDirection.RightToLeft;
                case SynchronizationDirection.RightToLeft:
                    return SynchronizationDirection.LeftToRight;
                case SynchronizationDirection.LeftToRightForced:
                    return SynchronizationDirection.RightToLeftForced;
                case SynchronizationDirection.LeftWins:
                    return SynchronizationDirection.RightWins;
                case SynchronizationDirection.RightWins:
                    return SynchronizationDirection.LeftWins;
                default:
                    throw new NotSupportedException($"Direction {Direction} is not supported");
            }
        }

        /// <summary>
        /// Starts the synchronization from the left model to the right model
        /// </summary>
        /// <param name="leftUri">the URI of the left model</param>
        /// <param name="leftRoot">the left root model</param>
        /// <param name="rightUri">the URI of the right model</param>
        /// <param name="rightRoot">the right root model</param>
        /// <returns>A running synchronization or null, if the synchronization is aborted</returns>
        protected abstract IRunningSynchronization SynchronizeLeftToRight(Uri leftUri, ref IModelElement leftRoot, Uri rightUri, ref IModelElement rightRoot);

        /// <summary>
        /// Starts the synchronization from the right model to the left model
        /// </summary>
        /// <param name="leftUri">the URI of the left model</param>
        /// <param name="leftRoot">the left root model</param>
        /// <param name="rightUri">the URI of the right model</param>
        /// <param name="rightRoot">the right root model</param>
        /// <returns>A running synchronization or null, if the synchronization is aborted</returns>
        protected abstract IRunningSynchronization SynchronizeRightToLeft(Uri leftUri, ref IModelElement leftRoot, Uri rightUri, ref IModelElement rightRoot);
    }

    /// <summary>
    ///     Represents a synchronization class for homogeneous model elements.
    /// </summary>
    /// <typeparam name="T">The type of model element being synchronized.</typeparam>
    public class HomogenModelSync<T> : ModelSynchronization where T : class, IModelElement
    {
        private readonly ReflectiveSynchronization _sync = new HomogeneousSynchronization<T>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public HomogenModelSync()
        {
            _sync.Initialize();
        }

        /// <inheritdoc />
        protected override IRunningSynchronization SynchronizeLeftToRight(Uri leftUri, ref IModelElement leftRoot, Uri rightUri, ref IModelElement rightRoot)
        {
            var leftModel = leftRoot as T;
            var rightModel = rightRoot as T;
            var context = _sync.Synchronize(ref leftModel, ref rightModel, Direction, ChangePropagationMode.TwoWay);
            leftRoot = leftModel;
            rightRoot = rightModel;
            return new RunningSynchronization(context, leftUri, rightUri, leftModel, rightModel, this);
        }

        /// <inheritdoc />
        protected override IRunningSynchronization SynchronizeRightToLeft(Uri leftUri, ref IModelElement leftRoot, Uri rightUri, ref IModelElement rightRoot)
        {
            var leftModel = leftRoot as T;
            var rightModel = rightRoot as T;
            var context = _sync.Synchronize(ref leftModel, ref rightModel, GetRightToLeftDirection(), ChangePropagationMode.TwoWay);
            leftRoot = leftModel;
            rightRoot = rightModel;
            return new RunningSynchronization(context, leftUri, rightUri, leftModel, rightModel, this);
        }
    }

    /// <summary>
    ///     Represents a generic model synchronization class for specific model element types.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left model element.</typeparam>
    /// <typeparam name="TRight">The type of the right model element.</typeparam>
    /// <typeparam name="TSynchronization">The type of the synchronization engine used.</typeparam>
    /// <typeparam name="TStartRule">The type of the starting synchronization rule.</typeparam>
    public class ModelSynchronization<TLeft, TRight, TSynchronization, TStartRule> : ModelSynchronization
        where TLeft : class, IModelElement
        where TRight : class, IModelElement
        where TSynchronization : ReflectiveSynchronization, new()
        where TStartRule : SynchronizationRule<TLeft, TRight>
    {
        private readonly TSynchronization _sync = new();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ModelSynchronization()
        {
            Name = typeof(TStartRule).Name;
        }

        /// <inheritdoc />
        protected override IRunningSynchronization SynchronizeLeftToRight(Uri leftUri, ref IModelElement leftRoot, Uri rightUri, ref IModelElement rightRoot)
        {
            var leftModel = leftRoot as TLeft;
            var rightModel = rightRoot as TRight;
            var context = _sync.Synchronize(_sync.SynchronizationRule<TStartRule>(), ref leftModel, ref rightModel, Direction, ChangePropagationMode.TwoWay);
            leftRoot = leftModel;
            rightRoot = rightModel;
            return new RunningSynchronization(context, leftUri, rightUri, leftModel, rightModel, this);
        }

        /// <inheritdoc />
        protected override IRunningSynchronization SynchronizeRightToLeft(Uri leftUri, ref IModelElement leftRoot, Uri rightUri, ref IModelElement rightRoot)
        {
            var leftModel = leftRoot as TLeft;
            var rightModel = rightRoot as TRight;
            var context = _sync.Synchronize(_sync.SynchronizationRule<TStartRule>(), ref leftModel, ref rightModel, GetRightToLeftDirection(), ChangePropagationMode.TwoWay);
            leftRoot = leftModel;
            rightRoot = rightModel;
            return new RunningSynchronization(context, leftUri, rightUri, leftModel, rightModel, this);
        }
    }
}