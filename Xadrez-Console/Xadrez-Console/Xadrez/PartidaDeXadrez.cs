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

        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }


        public PartidaDeXadrez()
        {
            tabuleiro = new Xadrez_Console.Tabuleiro.Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.White;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.retirarPeca(origem);
            p.incrementarQntMovimentos();
            Peca pecaCapturada = tabuleiro.retirarPeca(destino);
            tabuleiro.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual))
            {
                desFazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroExecption("Você não pode se colocar em xeque!");
            }

            if (estaEmXeque(adversario(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            turno++;
            mudaJogador();

        }

        public void desFazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tabuleiro.retirarPeca(destino);
            p.decrementarQntMovimentos();
            if (pecaCapturada != null)
            {
                tabuleiro.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tabuleiro.colocarPeca(p, origem);
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

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!tabuleiro.peca(origem).podeMoverPara(destino))
            {
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

        public HashSet<Peca> pecasCapturas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturas(cor));
            return aux;
        }

        private Cor adversario(Cor cor)
        {
            if (cor == Cor.White)
            {
                return Cor.Black;
            }
            else
            {
                return Cor.White;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroExecption("Não tem rei da cor " + cor + "no tabuleiro");
            }
            foreach (Peca x in pecasEmJogo(adversario(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tabuleiro.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('c', 1, new Torre(tabuleiro, Cor.White));
            colocarNovaPeca('c', 2, new Torre(tabuleiro, Cor.White));
            colocarNovaPeca('d', 1, new Rei(tabuleiro, Cor.White));
            colocarNovaPeca('e', 1, new Torre(tabuleiro, Cor.White));
            colocarNovaPeca('e', 2, new Torre(tabuleiro, Cor.White));
            colocarNovaPeca('d', 2, new Torre(tabuleiro, Cor.White));


            colocarNovaPeca('c', 7, new Torre(tabuleiro, Cor.Black));
            colocarNovaPeca('c', 8, new Torre(tabuleiro, Cor.Black));
            colocarNovaPeca('d', 8, new Rei(tabuleiro, Cor.Black));
            colocarNovaPeca('e', 7, new Torre(tabuleiro, Cor.Black));
            colocarNovaPeca('e', 8, new Torre(tabuleiro, Cor.Black));
            colocarNovaPeca('d', 7, new Torre(tabuleiro, Cor.Black));

        }
    }
}
