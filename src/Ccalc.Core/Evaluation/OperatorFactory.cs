namespace Ccalc.Core.Evaluation;

/// <summary>
/// Класс, содержащий информацию о математических операциях
/// </summary>
public class OperatorFactory
{
    private readonly List<Operator> _availableOperators;

    public OperatorFactory(List<Operator> availableOperators)
    {
        ArgumentNullException.ThrowIfNull(availableOperators);

        if (availableOperators.Any() is false)
            new ArgumentException("No operators specified", nameof(availableOperators));

        _availableOperators = availableOperators;
    }

    /// <summary>
    /// Получает математическую операцию из её названия
    /// </summary>
    /// <param name="operationName">Название математической операции</param>
    /// <returns>
    /// Математическая операция, соответствующая названию операции. Если операция не найдена,
    /// возвращает <see langword="null" />
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Если математическая операция с указанным названием не найдена
    /// </exception>
    public Operator? GetOperator(string operationName)
    {
        foreach (var @operator in _availableOperators)
            if (@operator.IsOperator(operationName)) return @operator;

        return null;
    }
}