using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Project_for_confirence
{
    public class ValueForGenotype
    {
        public int key;
        public int value;
        public int place_on_computer;

        public ValueForGenotype(int key, int value, List<int> intervals)
        {
            this.key = key;
            this.value = value;
            for (int i = 0; i < intervals.Count; i++)
            {
                if (value < intervals[i])
                {
                    place_on_computer = i;
                    break;
                }
            }
        }
    }
    public class Individual
    {
        public static int countOfIndividuals = 0;
        public string name;
        public List<ValueForGenotype> genotype;
        int min_max_criteria;

        int N;
        int M;
        public Individual(List<int> intervals, int M, int a, int b, int computer_storage)
        {
            countOfIndividuals++;
            genotype = new List<ValueForGenotype>();
            this.name = $"Особь{countOfIndividuals}";

            this.N = intervals.Count;
            this.M = M;
            var randomKey = new Random();
            var randomValue = new Random();
            Thread.Sleep(1);
            for (int i = 0; i < M; i++)
            {
                genotype.Add(
                    new ValueForGenotype(
                        randomKey.Next(a, b),
                        randomValue.Next(0, computer_storage),
                        intervals
                    ));
            }
        }
        public Individual(List<int> intervals, int M, int a, int b, int computer_storage, List<ValueForGenotype> genotype1)
        {
            countOfIndividuals++;
            genotype = new List<ValueForGenotype>();
            this.name = $"Особь{countOfIndividuals}";

            this.N = intervals.Count;
            this.M = M;
            this.genotype.AddRange(genotype1);
        }

        public int GetMinValue()
        {
            min_max_criteria = 0;
            int[] P = new int[N];
            for (int i = 0; i < genotype.Count; i++)
            {
                P[genotype[i].place_on_computer] += genotype[i].key;
            }

            min_max_criteria = P.Max();
            return min_max_criteria;
        }
    }

    class Node
    {
        public string name;
        public List<Individual> Generation;
        public string best_individual_name;
        public int best_individual_value;

        public Node(string name, List<Individual> Generation, string best_individual_name, int best_individual_value)
        {
            this.name = name;
            this.Generation = Generation;
            this.best_individual_name = best_individual_name;
            this.best_individual_value = best_individual_value;
        }
    }
    public class GoldbergAlg
    {
        int N;
        int M; int a;
        int b;
        int k;
        int pk = 0;
        int pm = 0;
        int kpovtor = 5;
        int computer_storage = 256;
        List<int> intervals = new List<int>();

        List<Individual> Generation;



        List<Node> history = new List<Node>();


        bool flag;
        int counter_of_povt;

        public GoldbergAlg(int N, int M, int a, int b, int k, int pk = 0, int pm = 0, int kpovtor = 5)
        {
            this.N = N;
            this.M = M;
            this.a = a;
            this.b = b;
            this.k = k;
            this.pk = pk;
            this.pm = pm;
            this.kpovtor = kpovtor;

        }

        public int SolveWithCM(Method method, int randomCount, int CM1Count, int CM2Count, int CM3Count)
        {
            history.Clear();
            GenerateIntervals();

            GenerateGenotypeByCM(method.T, randomCount, CM1Count, CM2Count, CM3Count);
            int counter_of_phase = 1;
            flag = true; //Флаг необходимости выполнения
            Phase(counter_of_phase, 0);
            counter_of_phase++;
            while (flag)
            {
                Phase(counter_of_phase, history[history.Count - 1].best_individual_value);
                counter_of_phase++;
            }
            return history[history.Count - 1].best_individual_value;

        }
        public int SolveRandom()
        {
            history.Clear();
            Random rnd = new Random();

            int[,] T = new int[M, N];

            for (int i = 0; i < M; i++)
            {
                var x = rnd.Next(a, b);
                for (int j = 0; j < N; j++)
                {
                    T[i, j] = x;
                }
            }
            GenerateIntervals();

            GenerateGenotype(T);
            int counter_of_phase = 1;
            flag = true; //Флаг необходимости выполнения
            Phase(counter_of_phase, 0);
            counter_of_phase++;
            while (flag)
            {
                Phase(counter_of_phase, history[history.Count - 1].best_individual_value);
                counter_of_phase++;
            }
            return history[history.Count - 1].best_individual_value;

        }
        public int SolveByT(Method method)
        {
            history.Clear();
            GenerateIntervals();


            GenerateGenotype(method.T);
            int counter_of_phase = 1;
            flag = true; //Флаг необходимости выполнения
            Phase(counter_of_phase, 0);
            counter_of_phase++;
            while (flag)
            {
                Phase(counter_of_phase, history[history.Count - 1].best_individual_value);
                counter_of_phase++;
            }
            return history[history.Count - 1].best_individual_value;
        }
        public int SolveByP(Method method)
        {
            history.Clear();
            GenerateIntervals();

            GenerateGenotype(method.P);
            int counter_of_phase = 1;
            flag = true; //Флаг необходимости выполнения
            Phase(counter_of_phase, 0);
            counter_of_phase++;
            while (flag)
            {
                Phase(counter_of_phase, history[history.Count - 1].best_individual_value);
                counter_of_phase++;
            }
            return history[history.Count - 1].best_individual_value;
        }

        private void GenerateGenotype()
        {
            Generation = new List<Individual>();

            for (int i = 0; i < k; i++)
            {
                var individual = new Individual(intervals, M, a, b, computer_storage);
                Generation.Add(individual);
            }

        }

        private void GenerateGenotype(int[,] T)
        {
            Generation = new List<Individual>();
            var rand = new Random();
            for (int i = 0; i < k; i++)
            {

                List<ValueForGenotype> genotype_by_T = new List<ValueForGenotype>();
                for (int index = 0; index < M; index++)
                {
                    genotype_by_T.Add(new ValueForGenotype(T[index, 0], rand.Next(0, computer_storage), intervals));

                }

                var individual = new Individual(intervals, M, a, b, computer_storage, genotype_by_T);
                Generation.Add(individual);
            }

        }
        private void GenerateGenotype(List<int>[] P)
        {
            Generation = new List<Individual>();
            var rand = new Random();
            for (int i = 0; i < k; i++)
            {
                List<ValueForGenotype> genotype_by_T = new List<ValueForGenotype>();
                for (int index = 0; index < N; index++)
                {
                    int interval = 0;
                    if (index == 0)
                    {
                        interval = intervals[0] / 2;
                    }
                    else
                    {
                        interval = (intervals[index] - (intervals[index - 1] + 1)) / 2 + intervals[index - 1];
                    }
                    foreach (var p in P[index])
                    {
                        genotype_by_T.Add(new ValueForGenotype(p, interval, intervals));

                    }

                }

                var individual = new Individual(intervals, M, a, b, computer_storage, genotype_by_T);
                Generation.Add(individual);
            }

        }
        private void GenerateGenotypeByCM(int[,] T, int randomCount, int CM1Count, int CM2Count, int CM3Count)
        {
            Generation = new List<Individual>();
            var rand = new Random();
            Method method = new Method(N, M, T);
            method.SolveCM1();
            var P = new List<int>[N];
            for (int i = 0; i < CM1Count; i++)
            {

                P = new List<int>[N];
                for (int j = 0; j < N; j++)
                {
                    P[j] = new List<int>();
                    foreach (var item in method.P[j])
                    {
                        P[j].Add(item);
                    }
                }
                List<ValueForGenotype> genotype_by_T = new List<ValueForGenotype>();
                for (int index = 0; index < M; index++)
                {

                    for (int indexOnP = 0; indexOnP < N; indexOnP++)
                    {
                        if (P[indexOnP].Contains(T[index, 0]))
                        {
                            int interval = 0;
                            if (indexOnP == 0)
                            {
                                interval = intervals[0] / 2;
                            }
                            else
                            {
                                interval = (intervals[indexOnP] - (intervals[indexOnP-1] + 1)) / 2 + intervals[indexOnP - 1];
                            }
                            genotype_by_T.Add(new ValueForGenotype(T[index, 0], interval, intervals));
                            P[indexOnP].Remove(T[index, 0]);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
                var individual = new Individual(intervals, M, a, b, computer_storage, genotype_by_T);
                Generation.Add(individual);

            }

            method.SolveCM2();
            P = new List<int>[N];
            for (int i = 0; i < CM2Count; i++)
            {

                P = new List<int>[N];
                for (int j = 0; j < N; j++)
                {
                    P[j] = new List<int>();
                    foreach (var item in method.P[j])
                    {
                        P[j].Add(item);
                    }
                }
                List<ValueForGenotype> genotype_by_T = new List<ValueForGenotype>();
                for (int index = 0; index < M; index++)
                {

                    for (int indexOnP = 0; indexOnP < N; indexOnP++)
                    {
                        if (P[indexOnP].Contains(T[index, 0]))
                        {
                            int interval = 0;
                            if (indexOnP == 0)
                            {
                                interval = intervals[0] / 2;
                            }
                            else
                            {
                                interval = (intervals[indexOnP] - (intervals[indexOnP - 1] + 1)) / 2 + intervals[indexOnP - 1];
                            }
                            genotype_by_T.Add(new ValueForGenotype(T[index, 0], interval, intervals));
                            P[indexOnP].Remove(T[index, 0]);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
                var individual = new Individual(intervals, M, a, b, computer_storage, genotype_by_T);
                Generation.Add(individual);

            }

            method.SolveCM3();
            P = new List<int>[N];
            for (int i = 0; i < CM3Count; i++)
            {

                P = new List<int>[N];
                for (int j = 0; j < N; j++)
                {
                    P[j] = new List<int>();
                    foreach (var item in method.P[j])
                    {
                        P[j].Add(item);
                    }
                }
                List<ValueForGenotype> genotype_by_T = new List<ValueForGenotype>();
                for (int index = 0; index < M; index++)
                {

                    for (int indexOnP = 0; indexOnP < N; indexOnP++)
                    {
                        if (P[indexOnP].Contains(T[index, 0]))
                        {
                            int interval = 0;
                            if (indexOnP == 0)
                            {
                                interval = intervals[0] / 2;
                            }
                            else
                            {
                                interval = (intervals[indexOnP] - (intervals[indexOnP - 1] + 1)) / 2 + intervals[indexOnP - 1];
                            }
                            genotype_by_T.Add(new ValueForGenotype(T[index, 0], interval, intervals));
                            P[indexOnP].Remove(T[index, 0]);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
                var individual = new Individual(intervals, M, a, b, computer_storage, genotype_by_T);
                Generation.Add(individual);

            }
            for (int i = 0; i < randomCount; i++)
            {

                List<ValueForGenotype> genotype_by_T = new List<ValueForGenotype>();
                for (int index = 0; index < M; index++)
                {
                    genotype_by_T.Add(new ValueForGenotype(T[index, 0], rand.Next(0, computer_storage), intervals));

                }

                var individual = new Individual(intervals, M, a, b, computer_storage, genotype_by_T);
                Generation.Add(individual);
            }

        }


        private void GenerateIntervals()
        {
            intervals.Clear();
            Individual.countOfIndividuals = 0;
            counter_of_povt = 0;
            int step = computer_storage / N; // шаг, на который нужно делить промежуток

            int end = 0; // конечное значение текущего отрезка

            for (int i = 0; i < N; i++)
            {
                end += step; // добавляем шаг к конечному значению
                if (i == N - 1) // если это последний отрезок
                {
                    end = computer_storage - 1; // то конечное значение равно верхней границе
                }

                intervals.Add(end);// выводим результат

            }
        }

        public void Phase(int phase_id, int pred_best_value)
        {


            Individual elite = Generation.OrderBy(i => i.GetMinValue()).FirstOrDefault(); // находим элемент с минимальным ключом
            if (elite != null)
            { // если найден элемент с минимальным ключом
                Generation.Remove(elite); // удаляем его из текущего местоположения
                Generation.Insert(0, elite); // перемещаем на первое место
            }
            Node node = new Node($"Поколение {phase_id}", Generation, elite.name, elite.GetMinValue());

            history.Add(node);

            if (node.best_individual_value == pred_best_value)
            {
                ++counter_of_povt;
            }
            else
            {
                counter_of_povt = 0;
            }
            if (counter_of_povt == kpovtor)
            {
                flag = false;
                return;
            }
            Random rand2 = new Random();
            
            for (int indexOfFirstIndividual = 1; indexOfFirstIndividual < k; indexOfFirstIndividual++)
            {

                if (rand2.Next(0, 100) <= pk)
                {
                    Random rand = new Random();
                    int indexOfSecondIndividual = rand.Next(0, k);
                    var child = Crossover(indexOfFirstIndividual, indexOfSecondIndividual);
                    if (child.GetMinValue() < Generation[indexOfFirstIndividual].GetMinValue())
                    {
                        Generation[indexOfFirstIndividual] = child;
                    }
                    else if (child.GetMinValue() < Generation[indexOfSecondIndividual].GetMinValue())
                    {
                        Generation[indexOfSecondIndividual] = child;
                    }
                }
                else if (rand2.Next(0, 100) <= pm)
                {
                                        
                    var genotypeTmp = Mutation(Generation[indexOfFirstIndividual].genotype);

                    int[] P = new int[N];
                    for (int i = 0; i < genotypeTmp.Count; i++)
                    {
                        P[genotypeTmp[i].place_on_computer] += genotypeTmp[i].key;
                    }

                    var min_max_criteria = P.Max();
                    
                    if (min_max_criteria < Generation[indexOfFirstIndividual].GetMinValue())
                    {
                        Generation[indexOfFirstIndividual].genotype = genotypeTmp;
                    }
                }

            }
            
        }

        private Individual Crossover(int indexOfFirstIndividual, int indexOfSecondIndividual)
        {

            Random rand1 = new Random();
            Random rand2 = new Random();

            int point_for_cross = rand1.Next(0, M);

            Thread.Sleep(1);
            var genForP1 = new List<ValueForGenotype>();
            var genForP2 = new List<ValueForGenotype>();
            for (int i = 0; i < point_for_cross; i++)
            {
                genForP1.Add(new ValueForGenotype(
                    Generation[indexOfFirstIndividual].genotype[i].key,
                    Generation[indexOfFirstIndividual].genotype[i].value,
                    intervals
                    ));
                genForP2.Add(new ValueForGenotype(
                    Generation[indexOfSecondIndividual].genotype[i].key,
                    Generation[indexOfSecondIndividual].genotype[i].value,
                    intervals
                    ));

            }
            for (int i = point_for_cross; i < M; i++)
            {
                genForP1.Add(new ValueForGenotype(
                    Generation[indexOfSecondIndividual].genotype[i].key,
                    Generation[indexOfSecondIndividual].genotype[i].value,
                    intervals
                    ));
                genForP2.Add(new ValueForGenotype(
                    Generation[indexOfFirstIndividual].genotype[i].key,
                    Generation[indexOfFirstIndividual].genotype[i].value,
                    intervals
                    ));
            }

            Individual p1 = new Individual(intervals, M, a, b, computer_storage, genForP1);
            Individual p2 = new Individual(intervals, M, a, b, computer_storage, genForP2);

            if (rand2.Next(0, 100) <= pm)
            {
                p1.genotype = Mutation(p1.genotype);
            }
            if (rand2.Next(0, 100) <= pm)
            {
                p2.genotype = Mutation(p2.genotype);
                p2.genotype = Mutation(p2.genotype);
            }

            if (p1.GetMinValue() <= p2.GetMinValue())
            {
                return p1;
            }
            else
            {
                return p2;
            }
        }

        public List<ValueForGenotype> Mutation(List<ValueForGenotype> genotypeForMut)
        {
            Random rand = new Random();
            Random rand1 = new Random();
            int indexOfValue = rand.Next(0, M);
            int indexOfBit = rand1.Next(0, 8);


            genotypeForMut[indexOfValue] = new ValueForGenotype(genotypeForMut[indexOfValue].key,
                InvertBit(genotypeForMut[indexOfValue].value, indexOfBit), intervals);
            return genotypeForMut;
        }

        public static int InvertBit(int value, int bit_number)
        {
            int mask = 1 << bit_number;
            return value ^ mask;
        }
    }
}
