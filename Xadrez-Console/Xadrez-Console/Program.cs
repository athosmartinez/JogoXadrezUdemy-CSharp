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

            Tabuleiro tabuleiro = new Tabuleiro(8, 8);

            tabuleiro.colocarPeca(new Rei(tabuleiro, Cor.Black), new Posicao(0, 0));
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.Black), new Posicao(1, 3));
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.Black), new Posicao(2, 5));

            Tela.imprimirTab(tabuleiro);
            Console.ReadLine();


        }
    }
}