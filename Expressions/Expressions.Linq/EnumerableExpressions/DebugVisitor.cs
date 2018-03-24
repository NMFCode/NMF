using System.Linq.Expressions;

namespace NMF.Expressions
{
    public class DebugVisitor : ExpressionVisitor
    {
        private int level = 0;

        public override Expression Visit(Expression exp)
        {
            if (exp != null)
            {
                if (level == 0)
                {
                    System.Diagnostics.Debug.WriteLine("#################################");
                }


                for (int i = 0; i < level; i++)
                {
                    System.Diagnostics.Debug.Write("   ");
                }
                System.Diagnostics.Debug.WriteLine("{0}  -  {1} : || {2} ||",
                    exp.NodeType, exp.GetType().Name, exp.ToString());
            }
            level++;
            Expression result = base.Visit(exp);
            level--;
            return result;
        }

        public void Display(Expression exp)
        {
            System.Diagnostics.Debug.WriteLine("===== DisplayVisitor.Display =====");
            this.Visit(exp);
        }
    }
}