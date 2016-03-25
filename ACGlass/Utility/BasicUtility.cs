using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility
{
    public static class BasicUtility
    {
        public static Random rander = new Random();

        public static bool throwCoin
        {
            get
            {
                return rander.Next(2) == 0;
            }
        }
        public static void Swap<T>(ref T left, ref T right)
        {
            T temp;
            temp = left;
            left = right;
            right = temp;
        }
        public static int[] array(params int[] i)
        {
            return i;
        }
        public static void checkBalance(ref int a, ref int b)
        {
            if (a > b)
            {
                int temp = a;
                a = b;
                b = temp;
            }
        }
        public static int[] Cfinder(int[] array, int count)
        {
            if (count > array.Length)
                return null;
            else if (count == array.Length)
                return array;
            else if (count > array.Length / 2)
            {
                int reduce = array.Length - count;
                List<int> bag = new List<int>(array);
                for (int i = 0; i < reduce; i++)
                {
                    int index = BasicUtility.rander.Next(bag.Count);
                    bag.RemoveAt(index);
                }
                return bag.ToArray();
            }
            else
            {
                List<int> bag = new List<int>(array);
                int[] result = new int[count];
                for (int i = 0; i < result.Length; i++)
                {
                    int index = BasicUtility.rander.Next(bag.Count);
                    result[i] = bag[index];
                    bag.RemoveAt(index);
                }
                return result;
            }
        }
        public static int powerSelect(double[][] em, double[] target)
        {
            double picker = 0;
            List<int> cand = new List<int>();
            double[] distance = new double[em.Length];
            for (int i = 0; i < em.Length; i++)
            {
                double[] node = em[i];
                distance[i] = 2 - (Math.Abs(target[0] - node[0]) + Math.Abs(target[1] - node[1]));
                if (distance[i] > picker)
                    picker = distance[i];
            }
            picker = BasicUtility.rander.NextDouble() * picker;
            for (int i = 0; i < distance.Length; i++)
                if (distance[i] >= picker)
                    cand.Add(i);
            int index = BasicUtility.rander.Next(cand.Count);
            return cand[index];
        }
    }
}
