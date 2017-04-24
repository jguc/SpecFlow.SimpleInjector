using System;
using SimpleInjector;

namespace MyCalculator.REPL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("MyCalculator REPL -- A SpecFlow Demo app by Gaspar Nagy, http://gasparnagy.com");
            Console.WriteLine();
            Console.WriteLine("  Enter number to stack operands or '+'/'*' operators to perform a calculation.");
            Console.WriteLine("  Enter 'q' to quit.");
            Console.WriteLine();

            var container = Dependencies.CreateContainerBuilder();

            var calculator = container.GetInstance<ICalculator>();
            ReplLoop(calculator);

        }

        private static void ReplLoop(ICalculator calculator)
        {
            while (true)
            {
                try
                {
                    Console.Write("$ ");
                    var command = Console.ReadLine();
                    switch (command)
                    {
                        case "+":
                        case "add":
                            calculator.Add();
                            PrintResult(calculator);
                            break;
                        case "*":
                        case "multiply":
                            calculator.Multiply();
                            PrintResult(calculator);
                            break;
                        case "q":
                        case "quit":
                            Console.WriteLine("Quit!");
                            return;
                        default:
                            int operand;
                            if (!int.TryParse(command, out operand))
                            {
                                Console.WriteLine("Invalid command!");
                                break;
                            }
                            calculator.Enter(operand);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void PrintResult(ICalculator calculator)
        {
            Console.WriteLine($"Result: {calculator.Result}");
        }
    }
}
