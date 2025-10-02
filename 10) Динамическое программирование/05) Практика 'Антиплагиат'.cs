using System;
using System.Collections.Generic;
using System.Linq;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var results = new List<ComparisonResult>();
            
            for (int i = 0; i < documents.Count; i++)
                for (int j = i + 1; j < documents.Count; j++)
                {
                    var doc1 = documents[i];
                    var doc2 = documents[j];
                    var distance = ComputeDistance(doc1, doc2);
                    results.Add(new ComparisonResult(doc1, doc2, distance));
                }
            
            return results;
        }

        private double ComputeDistance(List<string> seq1, List<string> seq2)
        {
            var dp = new double[seq1.Count + 1, seq2.Count + 1];
            
			for (int i = 0; i <= seq1.Count; i++)
							dp[i, 0] = i;
			for (int j = 0; j <= seq2.Count; j++)
							dp[0, j] = j;

            for (int i = 1; i <= seq1.Count; i++)
                for (int j = 1; j <= seq2.Count; j++)
                {
                    var replaceCost = TokenDistanceCalculator.GetTokenDistance(seq1[i-1], seq2[j-1]);
                    dp[i, j] = Math.Min(
                        Math.Min(
                            dp[i-1, j] + 1,    // delete
                            dp[i, j-1] + 1),   // insert
                        dp[i-1, j-1] + replaceCost);
                }

            return dp[seq1.Count, seq2.Count];
        }
    }
}