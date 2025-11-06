using NMF.AnyText.Metamodel;
using NMF.AnyText.Transformation;
using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal static class Helper
    {
        public static bool IsOptional(IFeatureExpression featureExpression)
        {
            return featureExpression is not ExistsAssignExpression && IsOptionalCore(featureExpression.Assigned);
        }

        public static bool IsNullable(IFeatureExpression featureExpression)
        {
            var feature = CodeGenerator._trace.LookupFeature(featureExpression);
            if (feature != null)
            {
                if (feature.LowerBound == 0 && feature.Type.GetExtension<MappedType>() is var mappedType)
                {
                    return mappedType?.SystemType != null && mappedType.SystemType.IsValueType;
                }
                return false;
            }
            return featureExpression is not ExistsAssignExpression && IsOptional(featureExpression);
        }

        public static bool IsOptionalCore(IParserExpression expression)
        {
            switch (expression.Parent)
            {
                case IModelRule:
                    return false;
                case IStarExpression:
                    return true;
                case IMaybeExpression:
                    return true;
                case IParserExpression otherExpression:
                    return IsOptionalCore(otherExpression);
                case IFormattedExpression formatted:
                    return formatted.Parent is IParserExpression parentExpression && IsOptionalCore(parentExpression);
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }
    }
}
