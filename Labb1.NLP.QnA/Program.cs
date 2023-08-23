using Labb1.NLP.QnA.QnA;
using Labb1.NLP.QnA.Speech;

namespace Labb1.NLP.QnA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "QnA";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            
            bool exitMenu = false;

            while (!exitMenu)
            {
                DisplayMenu();

                Console.Write("Enter your selection: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        QnAControll qna = new QnAControll();
                        qna.StartQnA();
                        break;
                    case "2":
                        Console.WriteLine("Exiting the menu.");
                        exitMenu = true;
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }

                if (!exitMenu)
                {
                    Console.Write("Press any key to exit...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║    Welcome to Alans home     ║");
            Console.WriteLine("╠══════════════════════════════╣");
            Console.WriteLine("║       Enter 1 for QnA        ║");
            Console.WriteLine("║                              ║");
            Console.WriteLine("║ 1. QnA               2. Exit ║");
            Console.WriteLine("╚══════════════════════════════╝");
        }
    }
}