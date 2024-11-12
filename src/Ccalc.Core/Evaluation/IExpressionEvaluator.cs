namespace Ccalc.Core.Evaluation;

/// <summary>
/// Интерфейс объекта, который получает результат из математических выражений
/// </summary>
public interface IExpressionEvaluator
{
    /// <summary>
    /// Расчитывает результат математического выражения
    /// </summary>
    /// <param name="expression">Математическое выражение</param>
    /// <returns>Результат математического выражения</param>
    public EvaluationResult Evaluate(string expression);
}