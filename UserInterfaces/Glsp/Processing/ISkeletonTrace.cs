using NMF.Glsp.Graph;

namespace NMF.Glsp.Processing
{
    public interface ISkeletonTrace
    {
        GElement ResolveElement(object element, object skeleton);
        void Trace(object element, GElement gElement);

        GElement RemoveElement(object element, object skeleton);
    }
}