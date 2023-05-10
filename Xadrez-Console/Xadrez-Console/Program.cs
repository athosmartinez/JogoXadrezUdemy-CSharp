using System;
using System.Security.Cryptography.X509Certificates;
using Xadrez_Console;
using Xadrez_Console.Tabuleiro;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Tabuleiro tabuleiro = new Tabuleiro(8, 8);
            Tela.imprimirTab(tabuleiro);
            Console.ReadLine(); 


        }
    }
}