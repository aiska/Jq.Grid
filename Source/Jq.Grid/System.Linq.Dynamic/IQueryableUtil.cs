using System;
using System.Linq.Expressions;
namespace System.Linq.Dynamic
{
	internal static class IQueryableUtil<T>
	{
		public static int Count(IQueryable Source)
		{
			return Source.OfType<T>().AsQueryable<T>().Count<T>();
		}
		public static IQueryable Page(IQueryable Source, int PageSize, int PageIndex)
		{
			return Source.OfType<T>().AsQueryable<T>().Skip(PageSize * (PageIndex - 1)).Take(PageSize);
		}
	}
	internal static class IQueryableUtil<T, PT>
	{
		public static IQueryable Sort(IQueryable source, string sortExpression, bool Ascending)
		{
			ParameterExpression parameterExpression = null;
			Expression<Func<T, PT>> keySelector = Expression.Lambda<Func<T, PT>>(Expression.Convert(Expression.Property(parameterExpression, sortExpression), typeof(PT)), new ParameterExpression[]
			{
				parameterExpression
			});
			if (Ascending)
			{
				return source.OfType<T>().AsQueryable<T>().OrderBy(keySelector);
			}
			return source.OfType<T>().AsQueryable<T>().OrderByDescending(keySelector);
		}
		public static IQueryable Contains(IQueryable Source, string PropertyName, string SearchClause)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "item");
			MemberExpression memberExpression = Expression.Property(parameterExpression, PropertyName);
			Expression.Convert(memberExpression, typeof(object));
			ConstantExpression constantExpression = Expression.Constant(SearchClause, typeof(string));
			MethodCallExpression body = Expression.Call(memberExpression, "Contains", new Type[0], new Expression[]
			{
				constantExpression
			});
			Expression<Func<T, bool>> predicate = Expression.Lambda<Func<T, bool>>(body, new ParameterExpression[]
			{
				parameterExpression
			});
			return Source.OfType<T>().AsQueryable<T>().Where(predicate);
		}
	}
}
