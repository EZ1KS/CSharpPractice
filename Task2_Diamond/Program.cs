using System;

public class Program
{
    public static void Main()
    {
        // Тест для N = 5
        Console.WriteLine("Ромб для N = 5:");
        DiamondDrawer.Draw(5);

        // Дополнительно – с запросом ввода (не обязательно, но удобно)
        Console.WriteLine("\nВведите нечётное число для своего ромба (или 0 для выхода):");
        if (int.TryParse(Console.ReadLine(), out int n) && n > 0 && n % 2 != 0)
        {
            DiamondDrawer.Draw(n);
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}