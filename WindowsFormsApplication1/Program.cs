using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var dd = default(int);
            GetBlockExpression();
            LoopExpression();
            ToLambdaExpression();
            BuildWithReturnValue();
            BuildSwitchCase();
        }

        private static void LoopExpression()
        {
            MethodInfo methodInfo = typeof(Console).GetMethod("WriteLine", new Type[1] { typeof(string) });
            var parameter = Expression.Parameter(typeof(int), "number");

            Expression body = Expression.Block(Expression.Call(null, methodInfo, Expression.Constant("Hello")),
                Expression.AddAssign(parameter, Expression.Constant(1)));

            LabelTarget labelTarget = Expression.Label();
            var test = Expression.LessThanOrEqual(parameter, Expression.Constant(10));
            var ifThenElse = Expression.IfThenElse(test, body, Expression.Break(labelTarget));
            BlockExpression block = BlockExpression.Block(parameter, ifThenElse);
            var loop = Expression.Loop(block, labelTarget);
            var exe = Expression.Block(new[] { parameter }, loop);
            Expression.Lambda<Action>(exe).Compile().Invoke();
        }

        private static void GetBlockExpression()
        {
            var parameter = Expression.Parameter(typeof(int), "number");
            var parameters = new ParameterExpression[1] { parameter };
            var addAssign = Expression.AddAssign(parameter, Expression.Constant(6));
            var divideAssign = Expression.DivideAssign(parameter, Expression.Constant(2));
            BlockExpression body = Expression.Block(parameters, addAssign, divideAssign);

            var lambda = Expression.Lambda<Func<int>>(body);
            Console.WriteLine(lambda.Compile().DynamicInvoke());
        }

        private static void ToLambdaExpression()
        {
            while (true)
            {
                int i = 0;
                if (i > 10)
                {
                    i = i + 1;
                }
                else
                {
                    break;
                }
            }

            LabelTarget label = Expression.Label();
            ParameterExpression ip = ParameterExpression.Parameter(typeof(int), "i");
            var ifThenElse = Expression.IfThenElse(Expression.GreaterThan(ip, Expression.Constant(10)), Expression.AddAssign(ip, Expression.Constant(1)), Expression.Break(label));
            var loop = Expression.Loop(ifThenElse, label);
            Expression b = Expression.Block(new[] { ip }, loop);
            Expression.Lambda(b).Compile().DynamicInvoke();
        }

        private static void BuildWithReturnValue()
        {
            //int i=0;return i;
            ParameterExpression p = ParameterExpression.Parameter(typeof(int), "i");
            LabelTarget returnTarget = Expression.Label(typeof(int));
            LabelExpression labelExp = Expression.Label(returnTarget, Expression.Constant(20));
            var @return = Expression.Return(returnTarget, p);

            var assign = Expression.AddAssign(p, Expression.Constant(10000));
            var block = Expression.Block(labelExp);
            var lambda = Expression.Lambda<Func<int, int>>(block, p);
            Console.WriteLine(lambda.Compile().Invoke(10));
        }

        private static void BuildSwitchCase()
        {
            //switch (@enum)
            //{
            //    default:
            //        return "不详";
            //    case 1:
            //        return "男";
            //    case 0:
            //        return "女";
            //}

            var p = Expression.Parameter(typeof(int));
            LabelTarget labelTarget = Expression.Label(typeof(string), "ReturnPoint");
            LabelExpression labelExp = Expression.Label(labelTarget, Expression.Constant("不详"));

            var swith = Expression.Switch(p
                , Expression.Return(labelTarget, labelExp.DefaultValue)
                , Expression.SwitchCase(Expression.Return(labelTarget, Expression.Constant("男")), Expression.Constant(1))
                , Expression.SwitchCase(Expression.Return(labelTarget, Expression.Constant("女")), Expression.Constant(0)));

            ;
            var l = Expression.Lambda(Expression.Block(swith, labelExp), p);
            Console.WriteLine(l.Compile().DynamicInvoke(10));
        }
    }
}
