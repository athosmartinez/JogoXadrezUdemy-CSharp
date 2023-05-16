using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xadrez_Console.Tabuleiro;

namespace Xadrez_Console.Xadrez
{
    internal class PartidaDeXadrez
    {
        public Xadrez_Console.Tabuleiro.Tabuleiro tabuleiro { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
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

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);

            turno++;
            mudaJogador();
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (tabuleiro.peca(pos) == null)
            {
                throw new TabuleiroExecption("Não existe peça na posição de origem escolhida.");
            }
            if (jogadorAtual != tabuleiro.peca(pos).cor)
            {
                throw new TabuleiroExecption("A peça de origem escolhida não é sua!");
            }
            if (!tabuleiro.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroExecption("Não há movimentos possiveis para está peça!");
            }
        }

        public void ValidarPoscaioDestino(Posicao origem, Posicao destino)
        {
            if (!tabuleiro.peca(origem).podeMoverPara(destino)){
                throw new TabuleiroExecption("Posição de destinho inválida!");
            }
        }
        private void mudaJogador()
        {
            if (jogadorAtual == Cor.White)
            {
                jogadorAtual = Cor.Black;
            }
            else
            {
                jogadorAtual = Cor.White;
            }
        }

        private void colocarPecas()
        {
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.White), new PosicaoXadrez('c', 1).toPosicao());
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.White), new PosicaoXadrez('c', 2).toPosicao());
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.White), new PosicaoXadrez('d', 2).toPosicao());
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.White), new PosicaoXadrez('e', 1).toPosicao());
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.White), new PosicaoXadrez('e', 2).toPosicao());
            tabuleiro.colocarPeca(new Rei(tabuleiro, Cor.White), new PosicaoXadrez('d', 1).toPosicao());

            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.Black), new PosicaoXadrez('c', 7).toPosicao());
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.Black), new PosicaoXadrez('c', 8).toPosicao());
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.Black), new PosicaoXadrez('d', 7).toPosicao());
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.Black), new PosicaoXadrez('e', 7).toPosicao());
            tabuleiro.colocarPeca(new Torre(tabuleiro, Cor.Black), new PosicaoXadrez('e', 8).toPosicao());
            tabuleiro.colocarPeca(new Rei(tabuleiro, Cor.Black), new PosicaoXadrez('d', 8).toPosicao());
        }
    }
}
