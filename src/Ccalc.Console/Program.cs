if (args.Any() is false)
{
    Console.WriteLine("Usage: ccalc <expression>");
    return;
}

var supportedOperations = new List<Operator>
{
    new Operator(
        "+",
        1,
        OperatorAssociativity.Left,
        (a, b) => a + b),
    new Operator(
        "-",
        1,
        OperatorAssociativity.Left,
        (a, b) => a - b),
    new Operator(
        "*",
        2,
        OperatorAssociativity.Left,
        (a, b) => a * b),
    new Operator(
        "/",
        2,
        OperatorAssociativity.Left,
        (a, b) => a / b),
};

var expression = string.Join(" ", args);

var calculator = Calculator.CreateDefaultCalculator(supportedOperations);

var result = calculator.Evaluate(expression);

if (result.IsSuccessful)
    Console.WriteLine(result.Result);
else
    Console.WriteLine(result.ErrorMessage);