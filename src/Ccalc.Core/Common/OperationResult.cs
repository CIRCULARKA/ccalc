namespace Ccalc.Core.Common;

/// <summary>
/// Результат выполнения некой операции
/// </summary>
public abstract class OperationResult
{
    /// <summary>
    /// Индикатор успешности операции
    /// </summary>
    public bool IsSuccessful { get; protected set; }

    /// <summary>
    /// Сообщение об ошибке, если операция была неуспешна
    /// </summary>
    public string? ErrorMessage { get; protected set; }
}