using System;
using System.Linq;
namespace GaussAlgorithm;
public class Solver
{
    public double[] Solve(double[][] matrix, double[] freeMembers)
    {
        int rows = matrix.Length;
        if (rows == 0) return new double[0];
        int cols = matrix[0].Length;

        double[][] augmentedMatrix = matrix
            .Select((row, i) => row.Concat(new[] { freeMembers[i] }).ToArray())
            .ToArray();

        int currentRow = 0;
        for (int currentCol = 0; currentCol < cols && currentRow < rows; currentCol++)
        {
            int pivotRow = FindPivotRow(augmentedMatrix, currentRow, currentCol);
            if (pivotRow == -1) continue;

            SwapRows(augmentedMatrix, currentRow, pivotRow);
            EliminateBelow(augmentedMatrix, currentRow, currentCol);
            currentRow++;
        }

        CheckForNoSolution(augmentedMatrix, matrix, freeMembers, cols);

        return BackSubstitution(augmentedMatrix, cols, currentRow);
    }

    private int FindPivotRow(double[][] matrix, int startRow, int col)
    {
        for (int i = startRow; i < matrix.Length; i++)
            if (Math.Abs(matrix[i][col]) > 1e-9) return i;
        return -1;
    }

    private void SwapRows(double[][] matrix, int row1, int row2)
    {
        if (row1 == row2) return;
        (matrix[row2], matrix[row1]) = (matrix[row1], matrix[row2]);
    }

    private void EliminateBelow(double[][] matrix, int row, int col)
    {
        double pivotValue = matrix[row][col];
        if (Math.Abs(pivotValue) < 1e-9) return;

        for (int i = row + 1; i < matrix.Length; i++)
        {
            double factor = matrix[i][col] / pivotValue;
            for (int j = col; j < matrix[i].Length; j++)
                matrix[i][j] -= factor * matrix[row][j];
        }
    }

    private void CheckForNoSolution(
        double[][] augmentedMatrix, 
        double[][] originalMatrix, 
        double[] freeMembers, 
        int cols
    )
    {
        foreach (var row in augmentedMatrix)
        {
            bool allZero = row.Take(cols).All(x => Math.Abs(x) < 1e-9);
            if (allZero && Math.Abs(row[cols]) > 1e-9)
                throw new NoSolutionException(
                    originalMatrix, 
                    freeMembers, 
                    augmentedMatrix.Select(r => r.Take(cols).ToArray()).ToArray() // Исправлено
                );
        }
    }

    private double[] BackSubstitution(double[][] matrix, int cols, int rank)
    {
        double[] solution = new double[cols];
        for (int i = rank - 1; i >= 0; i--)
        {
            int pivotCol = Array.FindIndex(matrix[i], x => Math.Abs(x) > 1e-9);
            if (pivotCol == -1) continue;

            double sum = matrix[i][cols];
            for (int j = pivotCol + 1; j < cols; j++)
                sum -= matrix[i][j] * solution[j];

            solution[pivotCol] = sum / matrix[i][pivotCol];
        }
        return solution;
    }
}