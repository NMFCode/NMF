using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    public class ExpressionVisitorBase : ExpressionVisitor
    {

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var right = Visit(node.Right);
            var left = Visit(node.Left);
            var conversion = node.Conversion != null ? Visit(node.Conversion) : null;
            if (left != node.Left || right != node.Right || conversion != node.Conversion)
            {
                return node.Update(left, node.Conversion, right);
            }
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
            var test = Visit(node.Test);
            var ifTrue = Visit(node.IfTrue);
            var ifFalse = Visit(node.IfFalse);
            if (test != node.Test || ifTrue != node.IfTrue || ifFalse != node.IfFalse)
            {
                return node.Update(test, ifTrue, ifFalse);
            }
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return node;
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            return node;
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
            var changed = false;
            var arguments = new Expression[node.Arguments.Count];
            var i = 0;
            foreach (var arg in node.Arguments)
            {
                var argument = Visit(arg);
                arguments[i] = argument;
                changed |= argument != arg;
            }
            var obj = Visit(node.Object);
            changed |= obj != node.Object;
            if (changed)
            {
                return node.Update(obj, arguments);
            }
            return node;
        }

        protected override Expression VisitExtension(Expression node)
        {
            return node;
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            var changed = false;
            var arguments = new Expression[node.Arguments.Count];
            var i = 0;
            foreach (var arg in node.Arguments)
            {
                var argument = Visit(arg);
                arguments[i] = argument;
                changed |= argument != arg;
                i++;
            }
            var obj = Visit(node.Expression);
            changed |= obj != node.Expression;
            if (changed)
            {
                return node.Update(obj, arguments);
            }
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
            var newBody = Visit(node.Body);
            if (newBody != node.Body)
            {
                return node.Update(newBody, node.Parameters);
            }
            return node;
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            var newExpression = Visit(node.NewExpression) as NewExpression;
            var changed = newExpression != node.NewExpression;
            var initializers = new ElementInit[node.Initializers.Count];
            for (int i = 0; i < node.Initializers.Count; i++)
            {
                initializers[i] = VisitElementInit(node.Initializers[i]);
                changed |= initializers[i] != node.Initializers[i];
            }
            if (changed)
            {
                return node.Update(newExpression, initializers);
            }
            return node;
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            throw new NotSupportedException("Statements are not supported!");
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var newExpression = Visit(node.Expression);
            if (newExpression != node.Expression)
            {
                return node.Update(newExpression);
            }
            return node;
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            var newExpression = Visit(node.Expression);
            var changed = newExpression != node.Expression;
            if (changed)
            {
                return node.Update(newExpression);
            }
            return node;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var newExpression = Visit(node.NewExpression) as NewExpression;
            var changed = newExpression != node.NewExpression;
            var bindings = new MemberBinding[node.Bindings.Count];
            for (int i = 0; i < node.Bindings.Count; i++)
            {
                bindings[i] = VisitMemberBinding(node.Bindings[i]);
                changed |= bindings[i] != node.Bindings[i];
            }
            if (changed)
            {
                return node.Update(newExpression, bindings);
            }
            return node;
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            var changed = false;
            var initializers = new ElementInit[node.Initializers.Count];
            for (int i = 0; i < node.Initializers.Count; i++)
            {
                initializers[i] = VisitElementInit(node.Initializers[i]);
                changed |= initializers[i] != node.Initializers[i];
            }
            if (changed)
            {
                return node.Update(initializers);
            }
            return node;
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            var changed = false;
            var bindings = new MemberBinding[node.Bindings.Count];
            for (int i = 0; i < node.Bindings.Count; i++)
            {
                bindings[i] = VisitMemberBinding(node.Bindings[i]);
                changed |= bindings[i] != node.Bindings[i];
            }
            if (changed)
            {
                return node.Update(bindings);
            }
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var changed = false;
            var arguments = new Expression[node.Arguments.Count];
            var i = 0;
            foreach (var arg in node.Arguments)
            {
                var argument = Visit(arg);
                arguments[i] = argument;
                changed |= argument != arg;
                i++;
            }
            var obj = Visit(node.Object);
            changed |= obj != node.Object;
            if (changed)
            {
                return node.Update(obj, arguments);
            }
            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            var changed = false;
            var arguments = new Expression[node.Arguments.Count];
            var i = 0;
            foreach (var arg in node.Arguments)
            {
                var argument = Visit(arg);
                arguments[i] = argument;
                i++;
                changed |= argument != arg;
            }
            if (changed)
            {
                return node.Update(arguments);
            }
            return node;
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            var changed = false;
            var arguments = new Expression[node.Expressions.Count];
            var i = 0;
            foreach (var arg in node.Expressions)
            {
                var argument = Visit(arg);
                arguments[i] = argument;
                changed |= argument != arg;
            }
            if (changed)
            {
                return node.Update(arguments);
            }
            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
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
            var exp = Visit(node.Expression);
            if (exp != node.Expression)
            {
                return node.Update(exp);
            }
            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            var exp = Visit(node.Operand);
            if (exp != node.Operand)
            {
                return node.Update(exp);
            }
            return node;
        }
    }
}
