using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceWebApi.Helpers
{
    public class LevenshteinAlgo
    {
        /// <summary>
    /// 
    /// </summary>
    /// <param name="string1"></param>
    /// <param name="string2"></param>
    /// <returns></returns>
        public  int ComputeDistance(string string1, string string2)
        {
            if (string1.Length == 0)
            {
                return string2.Length;
            }
            if (string2.Length == 0)
            {
                return string1.Length;
            }

            var distance = new int[string1.Length + 1, string2.Length + 1];
            // Initialization of matrix with row size string1Length and columns size string2Length
            for (var i = 0; i <= string1.Length; i++)
            {
                distance[i, 0] = i;
            }

            for (var j = 0; j <= string2.Length; j++)
            {
                distance[0, j] = j;
            }

            // Calculate rows and collumns distances
            for (var i = 1; i <= string1.Length; i++)
            {
                for (var j = 1; j <= string2.Length; j++)
                {
                    var cost = (string2[j - 1] == string1[i - 1]) ? 0 : 1;
                    distance[i, j] = Min(
                        distance[i - 1, j] + 1,  //insert
                        distance[i, j - 1] + 1, //remove
                        distance[i - 1, j - 1] + cost //replace
                    );
                }
            }
            return distance[string1.Length, string2.Length];
        }

        private static int Min(int e1, int e2, int e3) => Math.Min(Math.Min(e1, e2), e3);
    }
}
