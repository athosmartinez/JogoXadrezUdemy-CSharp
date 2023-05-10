using System;
using Xadrez_Console.Tabuleiro;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Tabuleiro tabuleiro = new Tabuleiro(8, 8);
            Console.WriteLine(tabuleiro);

        }
    }
}