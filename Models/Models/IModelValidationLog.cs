namespace NMF.Models
{
    public interface IModelValidationLog
    {
        void LogError(IModelElement element, string message);
    }
}
