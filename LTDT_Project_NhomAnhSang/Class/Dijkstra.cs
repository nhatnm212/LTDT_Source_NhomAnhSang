using System;
using System.Collections.Generic;
using System.Text;

namespace LTDT_Project_NhomAnhSang
{
    public class Dijkstra
    {
        public struct GRAPH
        {
            public int soDinh;
            public int[,] maTran;

            public GRAPH(int soDinh)
            {
                this.soDinh = soDinh;
                this.maTran = new int[100, 100];
            }
        }

        public bool[] ThuocT = new bool[100];
        public int[] Length = new int[100];
        public int[] LastV = new int[100];

        public void DuyetDijkstra(GRAPH g, int x, int y)
        {
            for (int i = 0; i < g.soDinh; i++)
            {
                ThuocT[i] = false;
                Length[i] = int.MaxValue;
                LastV[i] = -1;
            }
            Length[x] = 0;

            int v = x;
            while (v != -1)
            {
                ThuocT[v] = true;
                for (int k = 0; k < g.soDinh; k++)
                {
                    if (g.maTran[v, k] != 0 && !ThuocT[k] && (Length[k] == -1 || Length[k] > Length[v] + g.maTran[v, k]))
                    {
                        Length[k] = Length[v] + g.maTran[v, k];
                        LastV[k] = v;
                    }
                }
                v = -1;
                for (int i = 0; i < g.soDinh; i++)
                {
                    if (!ThuocT[i] && Length[i] != int.MaxValue)
                    {
                        if (v == -1 || Length[v] > Length[i])
                        {
                            v = i;
                        }
                    }
                }
            }
        }

        public string ketQuaChay(GRAPH g, int x, int y)
        {
            DuyetDijkstra(g, x, y);

            int[] DuongDi = new int[100];
            int v = y;
            int i;
            int id = 0;
            while (v != x)
            {
                DuongDi[id] = v;
                v = LastV[v];
                id++;
            }
            DuongDi[id] = x;
            string tmp = "";
            string kq = "";
            for (i = id; i > 0; i--)
                tmp = tmp + Convert.ToString(DuongDi[i] + 1) + " -> ";
            kq = tmp + Convert.ToString(DuongDi[i] + 1); 
            return kq;
        }
        public string luuv(GRAPH g, int x, int y)
        {
            DuyetDijkstra(g, x, y);

            int[] DuongDi = new int[100];
            //int[] LastV = new int[100];

            int v = y;
            int i;
            int id = 0;
            while (v != x)
            {
                DuongDi[id] = v;
                v = LastV[v];
                id++;
            }
            DuongDi[id] = x;
            string tmp = "";
            string kq = "";
            for (i = id; i > 0; i--)
                tmp = tmp + Convert.ToString(DuongDi[i]) + "\r\n"; 
            kq = tmp + Convert.ToString(DuongDi[i]); 
            return kq;
        }
        public string luuu(GRAPH g, int x, int y)
        {
            DuyetDijkstra(g, x, y);

            int[] DuongDi = new int[100];
            //int[] LastV = new int[100];

            int v = y;
            int i;
            int id = 0;
            while (v != x)
            {
                DuongDi[id] = v;
                v = LastV[v];
                id++;
            }
            DuongDi[id] = x;
            string tmp = "";
            string kq = "";
            for (i = id; i > 0; i--)
                tmp = tmp + Convert.ToString(DuongDi[i+1]) + "\r\n"; 
            kq = tmp + Convert.ToString(DuongDi[i+1]); 
            return kq;
        }
        public string tongDuongDi(GRAPH g, int x, int y)
        {
            DuyetDijkstra(g, x, y);
            return Convert.ToString(Length[y]); 
        }
    }
}
