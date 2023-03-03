using System.Linq.Expressions;

namespace UI.Infrastructure;

/// <summary>
/// Класс, содежащий методы расширения к <see cref="Expression"/>.
/// </summary>
internal static class ExpressionExtensions
{
    /// <summary>
    /// Получить имя свойства по выражению.
    /// </summary>
    /// <typeparam name="T">Тип свойства.</typeparam>
    /// <param name="nameExpression">Выражение для получения имени свойства.</param>
    /// <returns>The name of the expression.</returns>
    public static string GetName<T>(this Expression<T> nameExpression)
    {
        UnaryExpression unaryExpression = nameExpression.Body as UnaryExpression;

        // Преобразовать выражение в MemberExpression
        MemberExpression memberExpression = unaryExpression != null ?
            (MemberExpression)unaryExpression.Operand :
            (MemberExpression)nameExpression.Body;

        return memberExpression.Member.Name;
    }
}