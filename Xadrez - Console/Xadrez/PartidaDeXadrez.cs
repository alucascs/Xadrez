using System;
using tabuleiro;


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

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public void ExecultaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecultaMovimento(origem, destino);
            Turno++;
            MudaJogador();
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
            if (!Tab.peca(origem).PodeMoverPara(destino))
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

        private void ColocarNovaPeca(char coluna, int linha, Peca peca)
            {
                Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosition());
                Pecas.Add(peca);
            }

            private void ColocarPecas()
            {
                ColocarNovaPeca('c', 1, new Torre(Cor.Branca, Tab));
                ColocarNovaPeca('c', 2, new Torre(Cor.Branca, Tab));
                ColocarNovaPeca('d', 2, new Torre(Cor.Branca, Tab));
                ColocarNovaPeca('e', 1, new Torre(Cor.Branca, Tab));
                ColocarNovaPeca('e', 2, new Torre(Cor.Branca, Tab));
                ColocarNovaPeca('d', 1, new Rei(Cor.Branca, Tab));

                ColocarNovaPeca('c', 8, new Torre(Cor.Preta, Tab));
                ColocarNovaPeca('c', 7, new Torre(Cor.Preta, Tab));
                ColocarNovaPeca('d', 7, new Torre(Cor.Preta, Tab));
                ColocarNovaPeca('e', 8, new Torre(Cor.Preta, Tab));
                ColocarNovaPeca('e', 7, new Torre(Cor.Preta, Tab));
                ColocarNovaPeca('d', 8, new Rei(Cor.Preta, Tab));
            }
        }
    }
