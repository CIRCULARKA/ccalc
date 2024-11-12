namespace Ccalc.Core.Evaluation;

/// <summary>
/// Результат операции вычисления математического выражения
/// </summary>
public class EvaluationResult
{
    /// <summary>
    /// Результат вычисления
    /// </summary>
    public double? Result { get; init; }

    /// <summary>
    /// Индикатор успешности операции
    /// </summary>
    public bool IsSuccessful { get; init; }

    /// <summary>
    /// Сообщение об ошибке, если операция была неуспешна
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Создаёт результат, сигнализирующий об успешности операции
    /// </summary>
    /// <param name="result">Результат операции</param>
    public static EvaluationResult CreateSuccess(double result)
    {
        return new EvaluationResult
        {
            Result = result,
            IsSuccessful = true,
            ErrorMessage = null
        };
    }

    /// <summary>
    /// Создаёт объект, сигнализирующий об ошибке операции
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public static EvaluationResult CreateError(string message)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(message);

        return new EvaluationResult
        {
            Result = null,
            IsSuccessful = false,
            ErrorMessage = message
        };
    }
}