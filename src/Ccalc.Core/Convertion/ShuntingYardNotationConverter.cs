namespace Ccalc.Core.Convertion;

/// <summary>
/// Класс для преобразования математических выражений в инфиксной
/// нотации в постфиксную (польскую) с помощью алгоритма shunting yard
/// </summary>
public class ShuntingYardNotationConverter : INotationConverter
{
    public ConvertionResult ToPostfix(string infixExpression)
    {
        if (string.IsNullOrWhiteSpace(infixExpression))
            ConvertionResult.CreateError("Expression can not be empty");

        throw new NotImplementedException();
    }
}