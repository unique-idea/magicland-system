﻿using System.Linq.Expressions;

namespace MagicLand_System.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Creates a lambda expression that represents a conditional AND operation
        /// </summary>
        /// <param name="left">An expression to set the left property of the binary expression</param>
        /// <param name="right">An expression to set the right property of the binary expression</param>
        /// <returns>A binary expression that has the node type property equal to AndAlso, 
        /// and the left and right properties set to the specified values</returns>
        public static Expression<Func<T, Boolean>> AndAlso<T>(this Expression<Func<T, Boolean>> left, Expression<Func<T, Boolean>> right)
        {
            var combined = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    left.Body,
                    new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)
                ), left.Parameters);

            return combined;
        }

        /// <summary>
        /// Creates a lambda expression that represents a conditional OR operation
        /// </summary>
        /// <param name="left">An expression to set the left property of the binary expression</param>
        /// <param name="right">An expression to set the right property of the binary expression</param>
        /// <returns>A binary expression that has the node type property equal to OrElse, 
        /// and the left and right properties set to the specified values</returns>
        public static Expression<Func<T, Boolean>> OrElse<T>(Expression<Func<T, Boolean>> left, Expression<Func<T, Boolean>> right)
        {
            Expression<Func<T, bool>> combined = Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(
                    left.Body,
                    new ExpressionParameterReplacer(right.Parameters, left.Parameters).Visit(right.Body)
                ), left.Parameters);

            return combined;
        }
    }

    public class ExpressionParameterReplacer : ExpressionVisitor
    {
        private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

        public ExpressionParameterReplacer
            (IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
        {
            ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();

            for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
            { ParameterReplacements.Add(fromParameters[i], toParameters[i]); }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;

            if (ParameterReplacements.TryGetValue(node, out replacement))
            { node = replacement; }

            return base.VisitParameter(node);
        }
    }
}
