using System.Collections.Specialized;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Models;
using NMF.Models.Repository;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Transforms a model element into its XML representation.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string ToXmlString(this IModelElement element)
        {
            var serializer = MetaRepository.Instance.Serializer;

            var stream = new NonImplicitDisposableMemoryStream();
            serializer.SerializeFragment(element, stream);

            var result = Encoding.UTF8.GetString(stream.ToArray(), 0, (int)stream.Length);
            stream.CanDispose = true;
            stream.Dispose();

            return result;
        }
    }
}