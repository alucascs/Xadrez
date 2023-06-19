using System;
using tabuleiro;


namespace Xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set;}
        private int turno;
        private Cor JogadorAtual;
        public bool Terminada{ get; set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            turno = 1;
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
