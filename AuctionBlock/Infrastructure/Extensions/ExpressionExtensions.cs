using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AuctionBlock.Infrastructure.Extensions
{
    public static class ExpressionExtensions
    {
        public static PropertyInfo GetProperty<TPropertyType>(this 
            Expression<Func<TPropertyType>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static PropertyInfo GetProperty<TObjectType, TPropertyType>(this 
            Expression<Func<TObjectType, TPropertyType>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    return (PropertyInfo) ((MemberExpression) ((UnaryExpression) body).Operand).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}