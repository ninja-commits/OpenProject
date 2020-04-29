using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Prog
{
    public class Program
    {
        //public static bool IsConsecutives(List<int> array)
        //{
        //    for (var i = 1; i < array.Count; i++)
        //    {
        //        if (array[i] != array[i - 1] + 1)
        //            return false;
        //    }
               
        //    return true;
        //}
        //public static int Solution(int[] A)
        //{
 
        //    A = A.Distinct().ToArray();
        //    int count = 0;

        //    for (int i = 0; i < A.Length; i++)
        //    {
        //        var ls = A.ToList();
        //        ls.RemoveAt(i);
        //        //Console.WriteLine($"i = {i}");
        //        foreach (var nb in ls)
        //        {
        //            //Console.Write(nb);
        //        }

               
        //        if (IsConsecutives(ls))
        //            count++;
        //        //Console.Write(Environment.NewLine);
        //        //Console.Write($" count = {count}\n");
        //    }

        //    //Console.WriteLine(count);
        //    return count;
        //}

        public static int solution(int[][] A)
        {
            int[][] tmp = new int[A.Length][];
            int count = 1;

            for (int i = 0; i < A.Length; i++)
            {
                tmp[i] = new int [A[i].Length];
                for (var j = 0; j < A[i].Length; j++)
                {
                    tmp[i][j] = 0;
                }
            }

            tmp[0][0] = count;
            for (int i = 0; i < A.Length; i++)
            {
                for (var j = 0; j < A[i].Length; j++)
                {
                    int current = A[i][j];
                    if (i == 0 && j == 0)
                        continue;
                    if (i == 0)
                    {
                        if (A[0][j - 1] == current)
                            tmp[i][j] = tmp[i][j - 1];
                        else if (A[i + 1][j] == current)
                        {
                            tmp[i][j] = ++count;
                            tmp[i + 1][j] = tmp[i][j];
                        }
                        else
                            tmp[i][j] = ++count;
                        continue;
                    }

                    if (j == 0)
                    {
                        if (A[i - 1][j] == current)
                            tmp[i][j] = tmp[i - 1][j];
                        else if (A[i][j + 1] == current)
                        {
                            if (tmp[i][j + 1] == 0)
                                tmp[i][j + 1] = ++count;
                            tmp[i][j] = tmp[i][j + 1];

                        }
                        else
                            tmp[i][j] = ++count;
                        continue;
                    }

                    if (A[i][j - 1] == current)
                        tmp[i][j] = tmp[i][j - 1];
                    else if (A[i - 1][j] == current)
                        tmp[i][j] = tmp[i - 1][j];
                    else if (i + 1 != A.Length && A[i + 1][j] == current)
                    {
                        tmp[i][j] = ++count;
                        tmp[i + 1][j] = tmp[i][j];
                    }
                    else if (j + 1 != A[i].Length && A[i][j + 1] == current)
                    {
                        if (tmp[i][j] == 0)
                            ++count;
                        tmp[i][j] = tmp[i][j + 1];
                    }
                    else
                        tmp[i][j] = ++count;
                }
            }


            for (int i = 0; i < tmp.Length; i++)
            {
                for (var j = 0; j < tmp[i].Length; j++)
                    Console.Write($" {tmp[i][j]}");
                Console.Write(Environment.NewLine);
            }
            
            var ls = new List<int>();

            for (int i = 0; i < tmp.Length; i++)
                for (var j = 0; j < tmp[i].Length; j++)
                    ls.Add(tmp[i][j]);

            var lstmp = ls.Distinct();
            Console.WriteLine(lstmp.Count());
            return lstmp.Count();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
