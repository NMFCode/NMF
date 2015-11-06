using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    internal class GeneralizingExpressionVisitor : ExpressionVisitor
    {
        private List<PropertyPath> signatures = new List<PropertyPath>();
        private PropertyPath currentPath;

        public ICollection<PropertyPath> Signatures
        {
            get
            {
                return signatures;
            }
        }

        private static bool IsImmutableMethod(MethodInfo method)
        {
            return HasAttribute(method, typeof(ObservableProxyAttribute), false);
        }

        private static bool IsContainmentProperty(PropertyInfo property)
        {
            if (property.DeclaringType.IsValueType) return true;
            return HasAttribute(property, typeof(ContainmentAttribute), true);
        }

        private static bool HasAttribute(MemberInfo member, Type attributeType, bool inherit)
        {
            var attributes = member.GetCustomAttributes(attributeType, inherit);
            return attributes != null && attributes.Length > 0;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            Visit(node.Right);
            currentPath = null;
            return node;
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            Visit(node.Test);
            Visit(node.IfTrue);
            Visit(node.IfFalse);

            currentPath = null;
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            currentPath = null;
            return node;
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            currentPath = null;
            return node;
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            throw new NotSupportedException("Dynamic expressions are not supported!");
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            throw new NotImplementedException();
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            foreach (var arg in node.Arguments)
            {
                Visit(arg);
            }
            Visit(node.Object);
            return node;
        }

        protected override Expression VisitExtension(Expression node)
        {
            throw new NotSupportedException();
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            foreach (var arg in node.Arguments)
            {
                Visit(arg);
            }
            Visit(node.Expression);
            return node;
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return base.VisitLambda<T>(node);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            throw new NotImplementedException();
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            Visit(node.Expression);
            if (currentPath != null)
            {
                var newPath = signatures.FirstOrDefault(p => p.Parent == currentPath && p.Member == node.Member);
                if (newPath == null)
                {
                    newPath = new PropertyAccess(currentPath, node.Member);
                    signatures.Add(newPath);
                }
                currentPath = newPath;
            }
            else
            {
                throw new NotImplementedException();
            }
            return node;
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            throw new NotImplementedException();
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            throw new NotImplementedException();
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            throw new NotImplementedException();
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            throw new NotImplementedException();
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            throw new NotImplementedException();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (IsImmutableMethod(node.Method))
            {
                foreach (var arg in node.Arguments)
                {
                    Visit(arg);
                }
                Visit(node.Object);
                currentPath = null;
                return node;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected override Expression VisitNew(NewExpression node)
        {
            foreach (var arg in node.Arguments)
            {
                Visit(arg);
            }
            currentPath = null;
            return node;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            foreach (var arg in node.Expressions)
            {
                Visit(arg);
            }
            currentPath = null;
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            currentPath = signatures.OfType<PathRoot>().FirstOrDefault(p => p.RootParameter == node);
            if (currentPath == null)
            {
                currentPath = new PathRoot(node);
                signatures.Add(currentPath);
            }
            return node;
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitTry(TryExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            Visit(node.Expression);
            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            Visit(node.Operand);
            return node;
        }
    }
}
