using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            tabuleiro = new Xadrez_Console.Tabuleiro.Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.White;
            terminada = false;
            vulneravelEnPassant = null;
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

            //jogada especial - roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca t = tabuleiro.retirarPeca(origemT);
                t.incrementarQntMovimentos();
                tabuleiro.colocarPeca(t, destinoT);
            }

            //jogada especial - roque grande 
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca t = tabuleiro.retirarPeca(origemT);
                t.incrementarQntMovimentos();
                tabuleiro.colocarPeca(t, destinoT);
            }
            //jogada especial - en passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.cor == Cor.White)
                    {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tabuleiro.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);

                }
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
            if (testeXequeMate(adversario(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }
            Peca p = tabuleiro.peca(destino);
            //jogada especial - en passant
            if (p is Peao && (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else { vulneravelEnPassant = null; }
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

            //jogada especial - roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca t = tabuleiro.retirarPeca(destinoT);
                t.decrementarQntMovimentos();
                tabuleiro.colocarPeca(t, origemT);
            }

            //jogada especial - roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca t = tabuleiro.retirarPeca(destinoT);
                t.decrementarQntMovimentos();
                tabuleiro.colocarPeca(t, origemT);
            }
            //jogada especial - en passant 
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tabuleiro.retirarPeca(destino);
                    Posicao posP;
                    if (p.cor == Cor.White)
                    {
                        posP = new Posicao(3, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.coluna);
                    }
                    tabuleiro.colocarPeca(peao, posP);
                }
            }
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
            if (!tabuleiro.peca(origem).movimentoPossivel(destino))
            {
                throw new TabuleiroExecption("Posição de destino inválida!");
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

        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tabuleiro.linhas; i++)
                {
                    for (int j = 0; j < tabuleiro.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desFazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tabuleiro.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('a', 1, new Torre(tabuleiro, Cor.White));
            colocarNovaPeca('b', 1, new Cavalo(tabuleiro, Cor.White));
            colocarNovaPeca('c', 1, new Bispo(tabuleiro, Cor.White));
            colocarNovaPeca('d', 1, new Dama(tabuleiro, Cor.White));
            colocarNovaPeca('e', 1, new Rei(tabuleiro, Cor.White, this));
            colocarNovaPeca('f', 1, new Bispo(tabuleiro, Cor.White));
            colocarNovaPeca('g', 1, new Cavalo(tabuleiro, Cor.White));
            colocarNovaPeca('h', 1, new Torre(tabuleiro, Cor.White));
            colocarNovaPeca('a', 2, new Peao(tabuleiro, Cor.White, this));
            colocarNovaPeca('b', 2, new Peao(tabuleiro, Cor.White, this));
            colocarNovaPeca('c', 2, new Peao(tabuleiro, Cor.White, this));
            colocarNovaPeca('d', 2, new Peao(tabuleiro, Cor.White, this));
            colocarNovaPeca('e', 2, new Peao(tabuleiro, Cor.White, this));
            colocarNovaPeca('f', 2, new Peao(tabuleiro, Cor.White, this));
            colocarNovaPeca('g', 2, new Peao(tabuleiro, Cor.White, this));
            colocarNovaPeca('h', 2, new Peao(tabuleiro, Cor.White, this));

            colocarNovaPeca('a', 8, new Torre(tabuleiro, Cor.Black));
            colocarNovaPeca('b', 8, new Cavalo(tabuleiro, Cor.Black));
            colocarNovaPeca('c', 8, new Bispo(tabuleiro, Cor.Black));
            colocarNovaPeca('d', 8, new Dama(tabuleiro, Cor.Black));
            colocarNovaPeca('e', 8, new Rei(tabuleiro, Cor.Black, this));
            colocarNovaPeca('f', 8, new Bispo(tabuleiro, Cor.Black));
            colocarNovaPeca('g', 8, new Cavalo(tabuleiro, Cor.Black));
            colocarNovaPeca('h', 8, new Torre(tabuleiro, Cor.Black));
            colocarNovaPeca('a', 7, new Peao(tabuleiro, Cor.Black, this));
            colocarNovaPeca('b', 7, new Peao(tabuleiro, Cor.Black, this));
            colocarNovaPeca('c', 7, new Peao(tabuleiro, Cor.Black, this));
            colocarNovaPeca('d', 7, new Peao(tabuleiro, Cor.Black, this));
            colocarNovaPeca('e', 7, new Peao(tabuleiro, Cor.Black, this));
            colocarNovaPeca('f', 7, new Peao(tabuleiro, Cor.Black, this));
            colocarNovaPeca('g', 7, new Peao(tabuleiro, Cor.Black, this));
            colocarNovaPeca('h', 7, new Peao(tabuleiro, Cor.Black, this));


        }
    }
}
