namespace NMF.Synchronizations
{
    internal interface IOutputAccept<in T>
    {
        void AcceptNewOutput(T value);
    }
}
