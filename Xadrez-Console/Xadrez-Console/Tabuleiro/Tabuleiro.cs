using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Xadrez_Console.Tabuleiro
{
    internal class Tabuleiro
    {

        public int Linha { get; set; }
        public int Colunas { get; set; }
        private Peca[,] pecas;


        public Tabuleiro(int linhas, int colunas)
        {
            this.Linha = linhas;
            this.Colunas = colunas;
            pecas = new Peca[linhas, colunas];
        }
    }
}
