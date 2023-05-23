using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_for_confirence
{
    public class Method
    {
        int N, M;
        public int[,] T;
        public List<int>[] P;
        public int[,] Tcopy;
        public Method(int _N, int _M, int min, int max)
        {
            Random rnd = new Random();
            N = _N;
            M = _M;
            T = new int[M, N];
            P = new List<int>[N];
            for (int i = 0; i < N; i++)
            {
                P[i] = new List<int>();
            }
            Tcopy = new int[M, N];


            Console.WriteLine("\n:");
            for (int i = 0; i < M; i++)
            {
                var x = rnd.Next(min, max);
                for (int j = 0; j < N; j++)
                {
                    T[i, j] = x;

                    // Console.Write(T[i, j] + " ");
                }
                // Console.WriteLine("]\n");
            }
            Array.Copy(T, Tcopy, T.Length);
        }
        public Method(int _N, int _M, int[,] T)
        {

            N = _N;
            M = _M;
            this.T = T;
            P = new List<int>[N];
            for (int i = 0; i < N; i++)
            {
                P[i] = new List<int>();
            }
            Tcopy = new int[M, N];


            Array.Copy(T, Tcopy, T.Length);
        }
        private int CalculateCP() //Решение критическим путём
        {
            for (int i = 0; i < M; i++)
            {
                int index = 0;
                int min = int.MaxValue;
                for (int j = 0; j < N; j++)
                {
                    if (min > Tcopy[i, j] + P[j].Sum())
                    {
                        min = Tcopy[i, j] + P[j].Sum();
                        index = j;
                    }
                }
                P[index].Add(Tcopy[i, index]);

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: {P[k].Sum()}| ");

            }
            Console.WriteLine("Max:" + maxx);
            return maxx;
        }
        public int SolveCM1() //случайный критический путь
        {
            #region FirstSort
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);



            return CalculateCP();
            #endregion
        }
        public int SolveCM2() //критический путь по убыванию
        {
            #region FirstSort
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);

            SortFromHighToLow(ref Tcopy);

            return CalculateCP();
            #endregion
        }
        public int SolveCM3()  //критический путь по возрастанию
        {
            #region FirstSort
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);

            SortFromLowToHigh(ref Tcopy);

            return CalculateCP();
            #endregion
        }
        public int SolvePashkeev() //Пашкеев по убыв
        {
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);

            SortFromHighToLow(ref Tcopy);
            int index = 0;
            while (index < M)
            {
                if (P[0].Sum() <= P[N - 1].Sum())
                {
                    for (int i = 0; i < N; i++)
                    {
                        if (index < M)
                        {
                            P[i].Add(GetNumb(Tcopy, index));
                            index++;
                        }
                        else
                        {
                            break;
                        }

                    }
                }
                else
                {
                    for (int i = N - 1; i >= 0; i--)
                    {
                        if (index < M)
                        {
                            P[i].Add(GetNumb(Tcopy, index));
                            index++;
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: {P[k].Sum()}| ");

            }
            Console.WriteLine("Max:" + maxx);
            return maxx;
        }
        public int SolveKobakAlg() //Алгоритм Кобака
        {
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);

            float sum = 0;
            for (int i = 0; i < M; i++)
            {
                sum += GetNumb(Tcopy, i);
            }
            int n = Convert.ToInt32(Math.Ceiling(sum / N));


            SortFromHighToLow(ref Tcopy);


            HashSet<int> usedIndex = new HashSet<int>();
            HashSet<int> usedP = new HashSet<int>();
            List<int> indexes = new List<int>();
            List<int> tasks = new List<int>();
            for (int i = 0; i < M; i++)
            {
                tasks.Add(GetNumb(Tcopy, i));
                indexes.Add(i);
            }


            int index = 0;
            bool flag = true;
            while (index < M)
            {
                if (!indexes.Contains(index))
                {
                    index++;
                    continue;
                }
                if (usedIndex.Contains(index))
                {
                    continue;
                }
                flag = false;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() + tasks[index] <= n)
                    {
                        P[i].Add(tasks[index]);
                        flag = true;
                        break;
                    }
                    else
                    {
                        for (int k = index; k < M; k++)
                        {
                            if (indexes.Contains(k))
                            {
                                if (P[i].Sum() + tasks[k] <= n)
                                {
                                    indexes.Remove(k);
                                    P[i].Add(tasks[k]);
                                    flag = true;
                                    break;
                                }
                            }

                        }
                    }
                    usedIndex.Add(i);
                }

                if (!flag)
                {
                    usedIndex.Clear();
                    n++;
                    continue;
                }
                index++;
            }
            int count = 1;
            //foreach (var p in P)
            //{
            //    Console.Write($"p{count}: ");
            //    foreach (var t in p)
            //    {
            //        Console.Write($"{t} ");
            //    }
            //    Console.Write("\n");
            //}
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: {P[k].Sum()}| ");

            }
            Console.WriteLine("Max:" + maxx);
            return maxx;
        }

        public int SolveDoubleKrone() //Двойной Крон
        {
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);
            Random indexGenerator = new Random();
            for (int i = 0; i < M; i++)
            {
                P[indexGenerator.Next(N)].Add(GetNumb(Tcopy, i));
            }
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;
        }
        public int SolveDoubleKroneWithCM1() //Крон со случайным критическим путём
        {
            SolveCM1();
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;
        }
        public int SolveDoubleKroneWithCM2() //Крон с критическим путём по убыванию
        {
            SolveCM2();
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;
        }
        public int SolveDoubleKroneWithCM3() //Крон с критическим путём по возрастанию 
        {
            SolveCM3();
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;
        }
        public int SolveDoubleKroneWithPashkeev() //Крон с Пашкеевым
        {
            SolvePashkeev();
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;
        }
        public int SolveDoubleKroneWithKobak() //Крон с Кобаком
        {
            SolveKobakAlg();
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;

        }
        public int SolveDoubleKroneWithRandomOneProccesor() //Алгоритм Крона с одним процом случаное распределение
        {
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);

            for (int i = 0; i < M; i++)
            {
                P[0].Add(GetNumb(Tcopy, i));
            }
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;
        }
        public int SolveDoubleKroneWithHighToLowOneProccesor() //Алгоритм Крона с одним процом по убыванию
        {
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);

            SortFromHighToLow(ref Tcopy);
            for (int i = 0; i < M; i++)
            {
                P[0].Add(GetNumb(Tcopy, i));
            }
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;
        }
        public int SolveDoubleKroneWithLowToHighOneProccesor() //Алгоритм Крона с одним процом по возрастанию
        {
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);

            SortFromLowToHigh(ref Tcopy);
            for (int i = 0; i < M; i++)
            {
                P[0].Add(GetNumb(Tcopy, i));
            }
            bool flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }
            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return maxx;
        }

        public (int, int) SolveTripleKrone() //Тройной Крон
        {
            for (int i = 0; i < N; i++)
            {
                P[i].Clear();
            };
            Array.Copy(T, Tcopy, T.Length);
            Random indexGenerator = new Random();
            for (int i = 0; i < M; i++)
            {
                P[indexGenerator.Next(N)].Add(GetNumb(Tcopy, i));
            }
            bool flag = true;
            int flagOfThirt = 0;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }


            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            if (P[indexMax][i] < D)
                            {
                                flagOfThirt = 1;
                                flag = true;
                                P[indexAverage].Add(P[indexMax][i]);
                                P[indexMax].RemoveAt(i);
                                flagBreak = true;
                                break;

                            }
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;

                }


            }

            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            for (int j = 0; j < P[indexAverage].Count; j++)
                            {
                                if (P[indexMax][i] - P[indexAverage][j] < D && P[indexMax][i] - P[indexAverage][j] > 0)
                                {
                                    flagOfThirt = 1;
                                    flag = true;
                                    int log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    var tmp = P[indexAverage][j];
                                    P[indexAverage][j] = P[indexMax][i];
                                    P[indexMax][i] = tmp;
                                    log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    flagBreak = true;
                                    break;
                                }
                            }
                            if (flagBreak) break;
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;

                }


            }

            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return (maxx, flagOfThirt);
        }
        public (int, int) SolveTripleKroneWithPashkeev() //Крон с Кобаком
        {
            SolvePashkeev();
            bool flag = true;
            int flagOfThirt = 0;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }


            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            if (P[indexMax][i] < D)
                            {
                                flag = true;
                                flagOfThirt = 1;
                                P[indexAverage].Add(P[indexMax][i]);
                                P[indexMax].RemoveAt(i);
                                flagBreak = true;
                                break;

                            }
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;

                }


            }

            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            for (int j = 0; j < P[indexAverage].Count; j++)
                            {
                                if (P[indexMax][i] - P[indexAverage][j] < D && P[indexMax][i] - P[indexAverage][j] > 0)
                                {
                                    flagOfThirt = 1;
                                    flag = true;
                                    int log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    var tmp = P[indexAverage][j];
                                    P[indexAverage][j] = P[indexMax][i];
                                    P[indexMax][i] = tmp;
                                    log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    flagBreak = true;
                                    break;
                                }
                            }
                            if (flagBreak) break;
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;

                }


            }

            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return (maxx, flagOfThirt);

        }
        public (int, int) SolveTripleKroneWithKobak() //Крон с Кобаком
        {
            SolveKobakAlg();
            bool flag = true;
            int flagOfThirt = 0;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }


            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            if (P[indexMax][i] < D)
                            {
                                flag = true;
                                flagOfThirt = 1;
                                P[indexAverage].Add(P[indexMax][i]);
                                P[indexMax].RemoveAt(i);
                                flagBreak = true;
                                break;

                            }
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;

                }


            }

            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            for (int j = 0; j < P[indexAverage].Count; j++)
                            {
                                if (P[indexMax][i] - P[indexAverage][j] < D && P[indexMax][i] - P[indexAverage][j] > 0)
                                {
                                    flagOfThirt = 1;
                                    flag = true;
                                    int log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    var tmp = P[indexAverage][j];
                                    P[indexAverage][j] = P[indexMax][i];
                                    P[indexMax][i] = tmp;
                                    log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    flagBreak = true;
                                    break;
                                }
                            }
                            if (flagBreak) break;
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;

                }


            }

            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return (maxx, flagOfThirt);

        }
        public (int, int) SolveTripleKroneWithCM2() //Тройной Крон по убыв
        {
            SolveCM2();
            bool flag = true;
            int flagOfThirt = 0;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }


            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            if (P[indexMax][i] < D)
                            {
                                flag = true;
                                flagOfThirt = 1;
                                P[indexAverage].Add(P[indexMax][i]);
                                P[indexMax].RemoveAt(i);
                                flagBreak = true;
                                break;

                            }
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;
                }
            }
            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            for (int j = 0; j < P[indexAverage].Count; j++)
                            {
                                if (P[indexMax][i] - P[indexAverage][j] < D && P[indexMax][i] - P[indexAverage][j] > 0)
                                {
                                    flagOfThirt = 1;
                                    flag = true;
                                    int log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    var tmp = P[indexAverage][j];
                                    P[indexAverage][j] = P[indexMax][i];
                                    P[indexMax][i] = tmp;
                                    log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    flagBreak = true;
                                    break;
                                }
                            }
                            if (flagBreak) break;
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;

                }


            }

            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return (maxx, flagOfThirt);
        }
        public (int, int) SolveTripleKroneWithCM3() //Тройной Крон по возр
        {
            SolveCM3();
            bool flag = true;
            int flagOfThirt = 0;
            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    if (P[indexMax][i] < D)
                    {
                        flag = true;
                        P[indexMin].Add(P[indexMax][i]);
                        P[indexMax].RemoveAt(i);
                        break;

                    }
                }
            }
            flag = true;

            while (flag)
            {
                flag = false;
                int max = P[0].Sum();
                int min = P[0].Sum();
                int indexMax = 0;
                int indexMin = 0;

                for (int i = 1; i < N; i++)
                {
                    int x = P[i].Sum();
                    if (max < x)
                    {
                        max = x;
                        indexMax = i;
                    }
                    else if (min > x)
                    {
                        min = x;
                        indexMin = i;
                    }
                }
                int D = max - min;
                bool flagBreak = false;
                for (int i = 0; i < P[indexMax].Count; i++)
                {
                    for (int j = 0; j < P[indexMin].Count; j++)
                    {
                        if (P[indexMax][i] - P[indexMin][j] < D && P[indexMax][i] - P[indexMin][j] > 0)
                        {
                            flag = true;
                            var tmp = P[indexMin][j];
                            P[indexMin][j] = P[indexMax][i];
                            P[indexMax][i] = tmp;
                            flagBreak = true;
                            break;
                        }
                    }
                    if (flagBreak) break;
                }

            }


            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            if (P[indexMax][i] < D)
                            {
                                flag = true;
                                flagOfThirt = 1;
                                P[indexAverage].Add(P[indexMax][i]);
                                P[indexMax].RemoveAt(i);
                                flagBreak = true;
                                break;

                            }
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;
                }
            }
            flag = true;
            while (flag)
            {

                flag = false;
                SortForP(ref P);
                int indexStartMax = N - 1;
                for (int i = 0; i < N; i++)
                {
                    if (P[i].Sum() == P[indexStartMax].Sum())
                    {
                        indexStartMax = i;
                        break;
                    }
                }
                for (int indexMax = indexStartMax; indexMax < N; indexMax++)
                {

                    bool flagBreak = false;
                    for (int indexAverage = 0; indexAverage < indexStartMax; indexAverage++)
                    {
                        int max = P[indexMax].Sum();
                        int average = P[indexAverage].Sum();
                        int D = max - average;

                        for (int i = 0; i < P[indexMax].Count; i++)
                        {
                            for (int j = 0; j < P[indexAverage].Count; j++)
                            {
                                if (P[indexMax][i] - P[indexAverage][j] < D && P[indexMax][i] - P[indexAverage][j] > 0)
                                {
                                    flagOfThirt = 1;
                                    flag = true;
                                    int log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    var tmp = P[indexAverage][j];
                                    P[indexAverage][j] = P[indexMax][i];
                                    P[indexMax][i] = tmp;
                                    log = P[indexAverage].Sum();
                                    log = P[indexMax].Sum();
                                    flagBreak = true;
                                    break;
                                }
                            }
                            if (flagBreak) break;
                        }
                        if (flagBreak) break;
                    }
                    if (flagBreak) break;

                }


            }

            int maxx = P[0].Sum();
            for (int i = 1; i < N; i++)
            {
                int x = P[i].Sum();
                if (maxx < x)
                {
                    maxx = x;
                }
            }
            for (int k = 0; k < N; k++)
            {
                Console.Write($"p{k + 1}: ");
                for (int i = 0; i < P[k].Count; i++)
                {
                    Console.Write($"{P[k][i]}|");
                }
                Console.Write($"|Summ:{P[k].Sum()}|\n");

            }
            Console.Write($"pmax: {maxx}\n");
            return (maxx, flagOfThirt);
        }
        private void SortFromHighToLow(ref int[,] tasks) //сортировка по убыванию
        {
            for (var i = 1; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (tasks[i, j] != 0)
                    {
                        var key = tasks[i, j];
                        var k = i;
                        while ((k > 0) && (GetNumb(tasks, k - 1) < key))
                        {
                            Swap(ref tasks, k - 1, k);
                            k--;
                        }
                        break;
                    }
                    else continue;
                }
            }

        }
        private void SortFromLowToHigh(ref int[,] tasks) //сортировка по возрастанию
        {
            for (var i = 1; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (tasks[i, j] != 0)
                    {
                        var key = tasks[i, j];
                        var k = i;
                        while ((k > 0) && (GetNumb(tasks, k - 1) > key))
                        {
                            Swap(ref tasks, k - 1, k);
                            k--;
                        }
                        break;
                    }
                    else continue;
                }
            }
            Console.WriteLine();
        }
        private void Swap(ref int[,] someT, int x1, int x2)
        {
            for (var i = 0; i < N; i++)
            {
                var tmp = someT[x1, i];
                someT[x1, i] = someT[x2, i];
                someT[x2, i] = tmp;
            }

        }
        int GetNumb(int[,] someT, int i)
        {
            for (int j = 0; j < N; j++)
            {
                if (someT[i, j] != 0)
                {
                    return someT[i, j];
                }
                else continue;
            }
            return -1;
        }
        static void Swap(ref List<int>[] array, int i, int j) //Swap for P
        {
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        static void SortForP(ref List<int>[] inArray)
        {
            List<int> x;
            int j;
            for (int i = 1; i < inArray.Length; i++)
            {
                x = inArray[i];
                j = i;
                while (j > 0 && inArray[j - 1].Sum() > x.Sum())
                {
                    Swap(ref inArray, j, j - 1);
                    j -= 1;


                }
                inArray[j] = x;
            }
        }
    }
}
