namespace Ccalc.Core.Evaluation;

/// <summary>
/// Представляет математический оператор
/// </summary>
public class Operator
{
    private readonly Func<double, double, double> _logic;

    /// <summary>
    /// Создаёт оператор на основании его строкового представления и логики, по которой он будет работать
    /// </summary>
    /// <param name="sign">Строковое представление оператора</param>
    /// <param name="logic">Логика работы оператора</param>
    /// <exception>
    /// cref="ArgumentNullException">Если <paramref name="sign" /> или <paramref name="logic" /> равны <see langword="null" />
    /// </exception>
    public Operator(
        string sign,
        int precedence,
        OperatorAssociativity associativity,
        Func<double, double, double> logic)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(sign);
        ArgumentNullException.ThrowIfNull(logic);

        Sign = sign.ToLower().Trim();
        Precendence = precedence;
        Associativity = associativity;

        _logic = logic;
    }

    /// <summary>
    /// Строковое представление оператора
    /// </summary>
    public string Sign { get; private init; }

    /// <summary>
    /// Приоритетность оператора. Имеет смысл в сравнении с другими операторами
    /// </summary>
    public int Precendence { get; private init; }

    /// <summary>
    /// Порядок чтения оператора
    /// </summary>
    public OperatorAssociativity Associativity { get; private init; }

    public double Execute(double firstOperand, double secondOperand) =>
        _logic(firstOperand, secondOperand);

    /// <summary>
    /// Является ли токен этим оператором
    /// </summary>
    /// <param name="@operator">Название оператора, который нужно проверить</param>
    /// <returns>Является ли название оператора этим оператором</param>
    public bool IsOperator(string operatorName)
    {
        if (operatorName is null) return false;
        return operatorName.ToLower().Trim() == Sign;
    }
}