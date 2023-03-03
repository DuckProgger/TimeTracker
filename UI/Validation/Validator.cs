using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Controls;
using UI.Infrastructure;

namespace UI.Validation;

/// <summary>
/// Класс, отвечающий за валидации во ViewModel.
/// </summary>
internal class Validator
{
    private readonly IList<ValidationData> rules;

    public Validator()
    {
        rules = new List<ValidationData>();
    }


    /// <summary>
    /// Добавить валидацию.
    /// </summary>
    /// <param name="nameExpression">Выражение, указывающее на свойство.</param>
    /// <param name="validationRule">Добавляемое правило проверки.</param>
    public void Add(Expression<Func<object>> nameExpression, ValidationRule validationRule)
    {
        rules.Add(new ValidationData(nameExpression, validationRule));
    }


    /// <summary>
    /// Валидирует конкретное свойство ViewModel.
    /// </summary>
    /// <param name="propertyName">Проверяемое свойство.</param>
    /// <returns>Сообщение об ошибке для свойства. По умолчанию это пустая строка.</returns>
    public string Validate(string propertyName)
    {
        var relevantRules = rules.Where(r => r.Name == propertyName);

        foreach (ValidationData relevantRule in relevantRules)
        {
            var validationResult = relevantRule.Rule.Validate(relevantRule.Value, CultureInfo.CurrentCulture);
            if (!validationResult.IsValid)
            {
                return validationResult.ErrorContent?.ToString() ?? string.Empty;
            }
        }

        return string.Empty;
    }


    /// <summary>
    /// Произвести все валидации.
    /// </summary>
    /// <returns>true, если проверка прошла успешно; иначе false.</returns>
    public bool ValidateAll()
    {
        return rules.Aggregate(true, (success, rule) => success && Validate(rule.Name) == string.Empty);
    }

    public bool HasPropertyName(string propertyName) => rules.Any(r => r.Name == propertyName);


    /// <summary>
    /// Класс, содержащий данные для валидации.
    /// </summary>
    private class ValidationData
    {
        /// <summary>
        /// Gets the name expression to validate.
        /// </summary>
        private readonly Func<object> getPropertyValueFunc;

        /// <summary>
        /// Создать новый экземпляр класса <see cref="Validator"/>.
        /// </summary>
        /// <param name="nameExpression">Выражение, указывающее на свойство.</param>
        /// <param name="rule">Добавляемое правило проверки.</param>
        public ValidationData(Expression<Func<object>> nameExpression, ValidationRule rule)
        {
            Name = nameExpression.GetName();
            getPropertyValueFunc = nameExpression.Compile();
            Rule = rule;
        }

        /// <summary>
        /// Название свойства.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public object Value => getPropertyValueFunc();

        /// <summary>
        /// Правило валидации.
        /// </summary>
        public ValidationRule Rule { get; }
    }
}