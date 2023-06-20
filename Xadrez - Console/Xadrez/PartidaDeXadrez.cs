using System;
using tabuleiro;
using Xadrez;


namespace Xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool Xeque { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecultaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }

            // #Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao OrigemT1 = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao DestinoT1 = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(OrigemT1);
                T.IncrementarMovimentos();
                Tab.ColocarPeca(T, DestinoT1);
            }
            // #Roque Grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao OrigemT1 = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao DestinoT1 = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(OrigemT1);
                T.IncrementarMovimentos();
                Tab.ColocarPeca(T, DestinoT1);
            }

            return pecaCapturada;
        }

        public void DesfazerMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarMovimentos();
            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);

            // #Roque Pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao OrigemT1 = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao DestinoT1 = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(DestinoT1);
                T.DecrementarMovimentos();
                Tab.ColocarPeca(T, OrigemT1);
            }
            // #Roque Grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao OrigemT1 = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao DestinoT1 = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(DestinoT1);
                T.DecrementarMovimentos();
                Tab.ColocarPeca(T, OrigemT1);
            }
        }
        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecultaMovimento(origem, destino);
            if (EstaEmXeque(JogadorAtual))
            {
                DesfazerMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em Xeque!");
            }
            if (EstaEmXeque(Adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }
            if (TesteXequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição escolhida!");
            }
            if (JogadorAtual != Tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!Tab.peca(pos).ExisteMovimentos())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in Capturadas)
            {
                if (peca.Cor == cor)
                {
                    aux.Add(peca);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca peca in Pecas)
            {
                if (peca.Cor == cor)
                {
                    aux.Add(peca);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor Adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
                return Cor.Branca;
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca aux in PecasEmJogo(cor))
            {
                if (aux is Rei)
                {
                    return aux;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não existe rei da cor " + cor + " no tabuleiro!");
            }
            foreach (Peca aux in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = aux.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca aux in PecasEmJogo(cor))
            {
                bool[,] mat = aux.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = aux.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecultaMovimento(origem, destino);
                            bool TesteXeque = EstaEmXeque(cor);
                            DesfazerMovimento(origem, destino, pecaCapturada);
                            if (!TesteXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosition());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Cor.Branca, Tab));
            ColocarNovaPeca('h', 1, new Torre(Cor.Branca, Tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Branca, Tab));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Branca, Tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Branca, Tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Branca, Tab));
            ColocarNovaPeca('d', 1, new Rainha(Cor.Branca, Tab));
            ColocarNovaPeca('e', 1, new Rei(Cor.Branca, Tab, this));
            ColocarNovaPeca('a', 2, new Peao(Cor.Branca, Tab));
            ColocarNovaPeca('b', 2, new Peao(Cor.Branca, Tab));
            ColocarNovaPeca('c', 2, new Peao(Cor.Branca, Tab));
            ColocarNovaPeca('d', 2, new Peao(Cor.Branca, Tab));
            ColocarNovaPeca('e', 2, new Peao(Cor.Branca, Tab));
            ColocarNovaPeca('f', 2, new Peao(Cor.Branca, Tab));
            ColocarNovaPeca('g', 2, new Peao(Cor.Branca, Tab));
            ColocarNovaPeca('h', 2, new Peao(Cor.Branca, Tab));

            ColocarNovaPeca('a', 8, new Torre(Cor.Preta, Tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preta, Tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Preta, Tab));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Preta, Tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Preta, Tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Preta, Tab));
            ColocarNovaPeca('d', 8, new Rainha(Cor.Preta, Tab));
            ColocarNovaPeca('e', 8, new Rei(Cor.Preta, Tab, this));
            ColocarNovaPeca('a', 7, new Peao(Cor.Preta, Tab));
            ColocarNovaPeca('b', 7, new Peao(Cor.Preta, Tab));
            ColocarNovaPeca('c', 7, new Peao(Cor.Preta, Tab));
            ColocarNovaPeca('d', 7, new Peao(Cor.Preta, Tab));
            ColocarNovaPeca('e', 7, new Peao(Cor.Preta, Tab));
            ColocarNovaPeca('f', 7, new Peao(Cor.Preta, Tab));
            ColocarNovaPeca('g', 7, new Peao(Cor.Preta, Tab));
            ColocarNovaPeca('h', 7, new Peao(Cor.Preta, Tab));
        }
    }
}
