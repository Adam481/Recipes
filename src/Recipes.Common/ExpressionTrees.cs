using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Recipes.Common
{
    public class ExpressionTrees
    {
        public static void SimpleExpressionTreeExample()
        {
            ConstantExpression constExp = Expression.Constant(5, typeof(int));
            Console.WriteLine(constExp.NodeType);
            Console.WriteLine(constExp.Type);
            Console.WriteLine(constExp.Value);

            ParameterExpression iParam = Expression.Parameter(typeof(int), "i");
            Console.WriteLine(iParam.NodeType);
            Console.WriteLine(iParam.Type);
            Console.WriteLine(iParam.Name);

            BinaryExpression greaterThan = Expression.GreaterThan(iParam, constExp);
            Console.WriteLine(greaterThan.NodeType);
            Console.WriteLine(greaterThan.Type);
            Console.WriteLine(greaterThan.Left);
            Console.WriteLine(greaterThan.Left.Type);
            Console.WriteLine(greaterThan.Right);
            Console.WriteLine(greaterThan.Right.Type);

            Expression<Func<int, bool>> test = Expression.Lambda<Func<int, bool>>(greaterThan, iParam);


            Func<int, bool> meDel = test.Compile();

            Console.WriteLine(meDel(3));
            Console.WriteLine(meDel(8));


            int[] ints = new[] { 1, 9, 2, 7, 4, 3, 6, 5 };

            // compiler will translate lambda to normal code
            IEnumerable<int> result1 = ints.Where(i => i < 5);
            IEnumerable<int> result2 = Enumerable.Where(ints, i => i < 5);

            
            // public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate);
            // compiler will translate lambda expression into object (no code is generated)
            IQueryable<int> intsQ = ints.AsQueryable();
            IQueryable<int> result3 = intsQ.Where(i => i < 5);
            IQueryable<int> result4 = Queryable.Where(intsQ, i => i < 5);
            Console.WriteLine(result4); //print expression object
        }
    }
}
