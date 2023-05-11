using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xadrez_Console.Tabuleiro;

namespace Xadrez_Console.Xadrez
{
    internal class PartidaDeXadrez
    {
        public Xadrez_Console.Tabuleiro.Tabuleiro tabuleiro { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaDeXadrez()
        {
            tabuleiro = new Xadrez_Console.Tabuleiro.Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.White;
            terminada = false;
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.retirarPeca(origem);
            p.incrementarQntMovimentos();
            Peca capturada = tabuleiro.retirarPeca(destino);
            tabuleiro.colocarPeca(p, destino);
        }
        private void colocarPecas()
        {
            tabuleiro.colocarPeca(new Rei(tabuleiro, Cor.White), new PosicaoXadrez('c', 1).toPosicao());
            tabuleiro.colocarPeca(new Rei(tabuleiro, Cor.Black), new PosicaoXadrez('c', 8).toPosicao());
            tabuleiro.colocarPeca(new Rei(tabuleiro, Cor.Black), new PosicaoXadrez('c', 7).toPosicao());
            tabuleiro.colocarPeca(new Rei(tabuleiro, Cor.White), new PosicaoXadrez('c', 2).toPosicao());
        }
    }
}
