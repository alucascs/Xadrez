using System;
using tabuleiro;


namespace Xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set;}
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada{ get; set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            ColocarPecas();
            Terminada = false;
        }

        public void ExecultaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
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

        public  void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if(!Tab.peca(origem).PodeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudaJogador()
        {
            if(JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        private void ColocarPecas()
        {
            Tab.ColocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('c', 1).toPosition());
            Tab.ColocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('c', 2).toPosition());
            Tab.ColocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('d', 2).toPosition());
            Tab.ColocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('e', 2).toPosition());
            Tab.ColocarPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('e', 1).toPosition());
            Tab.ColocarPeca(new Rei(Cor.Branca, Tab), new PosicaoXadrez('d', 1).toPosition());

            Tab.ColocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('c', 7).toPosition());
            Tab.ColocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('c', 8).toPosition());
            Tab.ColocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('d', 7).toPosition());
            Tab.ColocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('e', 7).toPosition());
            Tab.ColocarPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('e', 8).toPosition());
            Tab.ColocarPeca(new Rei(Cor.Preta, Tab), new PosicaoXadrez('d', 8).toPosition());
        }
    }
}
