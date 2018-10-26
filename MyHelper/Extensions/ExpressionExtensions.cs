using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace MyHelper.Extensions
{
    /// <summary>
    /// 表达式树扩展
    /// </summary>
    public static class ExpressionExtensions
    {

        #region 拓展And和Or方法

        /// <summary>
        /// 连接表达式与运算
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="one">原表达式</param>
        /// <param name="another">新的表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            //创建新参数
            var newParameter = Expression.Parameter(typeof(T), "parameter");

            var parameterReplacer = new ParameterReplacer(newParameter);
            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);
            var body = Expression.AndAlso(left, right);

            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        /// <summary>
        /// 连接表达式或运算
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="one">原表达式</param>
        /// <param name="another">新表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            //创建新参数
            var newParameter = Expression.Parameter(typeof(T), "parameter");

            var parameterReplacer = new ParameterReplacer(newParameter);
            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);
            var body = Expression.OrElse(left, right);

            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        #endregion

        #region 拓展Expression的Invoke方法

        public static TResult Invoke<TResult>(this Expression<Func<TResult>> expression)
        {
            return expression.Compile().Invoke();
        }

        public static TResult Invoke<T1, TResult>(this Expression<Func<T1, TResult>> expression, T1 arg1)
        {
            return expression.Compile().Invoke(arg1);
        }

        public static TResult Invoke<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> expression, T1 arg1, T2 arg2)
        {
            return expression.Compile().Invoke(arg1, arg2);
        }

        public static TResult Invoke<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> expression, T1 arg1, T2 arg2, T3 arg3)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3);
        }

        public static TResult Invoke<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, TResult>(this Expression<Func<T1, T2, T3, T4, T5, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
        #endregion
    }

    /// <summary>
    /// 继承ExpressionVisitor类，实现参数替换统一
    /// </summary>
    class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            Parameter = paramExpr;
        }

        //新的表达式参数
        private ParameterExpression Parameter { get; set; }

        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }

        /// <summary>
        /// 覆盖父方法，返回新的参数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            return Parameter;
        }
    }
}
