using System;
using System.Collections.Generic;
namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            // матрицa LCS подпоследовательностей
            int[,] opt = new int[first.Count + 1, second.Count + 1];

            // opt
            for (int i = 1; i <= first.Count; i++)
            {
                for (int j = 1; j <= second.Count; j++)
                {
                    if (first[i - 1] == second[j - 1])
                        opt[i, j] = opt[i - 1, j - 1] + 1;
                    else
                        opt[i, j] = Math.Max(opt[i - 1, j], opt[i, j - 1]);
                }
            }

            // подпоследовательность
            return RestoreLCS(opt, first, second);
        }

        private static List<string> RestoreLCS(int[,] opt, List<string> first, List<string> second)
        {
            List<string> result = new List<string>();
            int i = first.Count, j = second.Count;

            while (i > 0 && j > 0)
            {
                if (first[i - 1] == second[j - 1])
                {
                    result.Add(first[i - 1]);
                    i--;
                    j--;
                }
                else if (opt[i - 1, j] >= opt[i, j - 1])
                    i--;
                else
                    j--;
            }

            result.Reverse(); //
            return result;
        }
    }
}