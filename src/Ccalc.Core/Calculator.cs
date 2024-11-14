namespace Ccalc.Core;

/// <summary>
/// Класс, вычисляющий математические выражения
/// </summary>
public class Calculator
{
    private readonly INotationConverter _notationConverter;

    private readonly IExpressionEvaluator _evaluator;

    public Calculator(INotationConverter notationConverter, IExpressionEvaluator evaluator)
    {
        ArgumentNullException.ThrowIfNull(notationConverter);
        ArgumentNullException.ThrowIfNull(evaluator);

        _notationConverter = notationConverter;
        _evaluator = evaluator;
    }

    /// <summary>
    /// Вычисляет результат математического выражения
    /// </summary>
    /// <param name="mathExpression">Математическое выражение</param>
    public EvaluationResult Evaluate(string mathExpression)
    {
        var convertionResult = _notationConverter.ToPostfix(mathExpression);

        if (convertionResult.IsSuccessful is false)
            return EvaluationResult.CreateError(convertionResult.ErrorMessage ?? "");

        var evaluationResult = _evaluator.Evaluate(convertionResult.Result ?? "");

        return evaluationResult;
    }

    /// <summary>
    /// Создаёт экземпляр калькулятора
    /// </summary>
    /// <param name="supportedOperations">Список поддерживаемых математических операций</param>
    public static Calculator CreateDefaultCalculator(List<Operator> supportedOperations)
    {
        var parser = new MathExpressionParser();

        var converter = new ShuntingYardNotationConverter(supportedOperations, parser);
        var evaluator = new RPNExpressionEvaluator(supportedOperations, parser);

        return new Calculator(converter, evaluator);
    }
}