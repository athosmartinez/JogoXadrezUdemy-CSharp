﻿using System;
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
                    try
                    {

                        Console.Clear();

                        Tela.imprimirTab(partida.tabuleiro);
                        Console.WriteLine();
                        Console.WriteLine("Turno: " + partida.turno);
                        Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);
                        Console.Write("Origem > ");
                        Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                        partida.ValidarPosicaoDeOrigem(origem);

                        bool[,] posicoesPossiveis = partida.tabuleiro.peca(origem).movimentosPossiveis();

                        Console.Clear();
                        Tela.imprimirTab(partida.tabuleiro, posicoesPossiveis);
                        Console.WriteLine();
                        Console.Write("Destino > ");
                        Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
                        partida.ValidarPoscaioDestino(origem, destino);
                        partida.realizaJogada(origem, destino);
                    }
                    catch (TabuleiroExecption e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
            }
            catch (TabuleiroExecption e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}