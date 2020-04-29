using System;

namespace ConsoleApp1
{
    class Program
    {
        static int Reverse(int x)
        {
            int reverse = 0;

            while (x != 0)
            {
                reverse = reverse * 10 + x % 10;
                x = x / 10;
            }

            return reverse;
        }
        static void Main(string[] args)
        {
            Console.WriteLine(Reverse(123));
            Console.ReadLine();
        }
    }
}
