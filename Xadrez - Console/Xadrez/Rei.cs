using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;

namespace Xadrez
{
    internal class Rei : Peca
    {

        private PartidaDeXadrez partida;
        public Rei(Cor cor, Tabuleiro tab, PartidaDeXadrez partida) : base(cor, tab)
        {
            this.partida = partida;
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return p == null || p.Cor != Cor;
        }

        private bool TesteRoque(Posicao pos)
        {
            Peca p = Tab.peca(pos);
            return p != null && p is Torre && p.QtdMovimentos == 0;
        }
        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];
            Posicao pos = new Posicao(0, 0);
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            if(Tab.posicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            if (Tab.posicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (Tab.posicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            // #Roque
            if(QtdMovimentos == 0 && !partida.Xeque)
            {
                // #Roque pequeno
                Posicao PosT1 = new Posicao(Posicao.Linha, Posicao.Coluna + 3);
                if(TesteRoque(PosT1))
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna + 2);
                    if(Tab.peca(p1) == null && Tab.peca(p2) == null)
                    {
                        mat[Posicao.Linha, Posicao.Coluna + 2] = true;
                    }
                }
                // #Roque Grande
                Posicao PosT2 = new Posicao(Posicao.Linha, Posicao.Coluna - 4);
                if (TesteRoque(PosT2))
                {
                    Posicao p1 = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                    Posicao p2 = new Posicao(Posicao.Linha, Posicao.Coluna - 2);
                    Posicao p3 = new Posicao(Posicao.Linha, Posicao.Coluna - 3);
                    if (Tab.peca(p1) == null && Tab.peca(p2) == null && Tab.peca(p3) == null)
                    {
                        mat[Posicao.Linha, Posicao.Coluna - 2] = true;
                    }
                }

            }

            return mat;
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
