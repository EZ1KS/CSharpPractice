using System;

public class Program
{
    public static void Main()
    {
        string result = CompoundInterestCalculator.CalculateCompoundInterest(1000, 3, 10);
        Console.WriteLine(result);

        // Чтобы консоль не закрылась сразу при запуске без отладчика:
        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}