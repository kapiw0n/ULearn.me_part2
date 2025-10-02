using System.Numerics;
namespace Tickets;

public class TicketsTask
{
    public static BigInteger Solve(int ticketHalfLength, int totalTicketSum)
    {
        // total sum is odd
        if (totalTicketSum % 2 != 0)
            return 0;

        int halfTicketSum = totalTicketSum / 2;
        BigInteger[] combinations = InitializeCombinations(halfTicketSum);

        //for 1/2
        for (int i = 0; i < ticketHalfLength; i++)
        {
            combinations = CalculateNewCombinations(combinations, halfTicketSum);
        }

        //square of the nums
        return combinations[halfTicketSum] * combinations[halfTicketSum];
    }

    private static BigInteger[] InitializeCombinations(int halfTicketSum)
    {
        BigInteger[] combinations = new BigInteger[halfTicketSum + 1];
        combinations[0] = 1; // One way to make sum 0
        return combinations;
    }

    private static BigInteger[] CalculateNewCombinations(BigInteger[] currentCombinations, int halfTicketSum)
    {
        BigInteger[] newCombinations = new BigInteger[halfTicketSum + 1];

        //  combinations for the current 1/2
        for (int currentSum = 0; currentSum <= halfTicketSum; currentSum++)
        {
            for (int digit = 0; digit <= 9; digit++)
            {
//
                if (currentSum + digit <= halfTicketSum)
                    newCombinations[currentSum + digit] += currentCombinations[currentSum];
            }
        }

        return newCombinations;
    }
}