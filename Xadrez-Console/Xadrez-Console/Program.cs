using System;
using System.Security.Cryptography.X509Certificates;
using Xadrez_Console;
using Xadrez_Console.Tabuleiro;
using Xadrez_Console.Xadrez;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {


            PosicaoXadrez pos = new PosicaoXadrez('a', 1);
            Console.WriteLine(pos.toPosicao());

        }
    }
}