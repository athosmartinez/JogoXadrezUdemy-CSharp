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
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.terminada)
                {
                    Console.Clear();
                    Tela.imprimirTab(partida.tabuleiro);
                    Console.WriteLine();
                    Console.Write("Origem > ");
                    Posicao origem = Tela.letPosicaoXadrez().toPosicao();
                    Console.Write("Destino > ");
                    Posicao destino = Tela.letPosicaoXadrez().toPosicao();
                    partida.executaMovimento(origem, destino);
                }


            }
            catch (TabuleiroExecption e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}