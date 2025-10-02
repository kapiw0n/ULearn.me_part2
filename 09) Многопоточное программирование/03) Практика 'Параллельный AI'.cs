using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    // get the next move 
    public Rocket GetNextMove(Rocket rocket)
    {
        var tasks = CreateTasks(rocket); // Create tasks
        var results = Task.WhenAll(tasks).GetAwaiter().GetResult(); // Wait for all
        var bestMove = results.OrderByDescending(r => r.Score).First(); // Get the best move
        return rocket.Move(bestMove.Turn, level); // Move the rocket
    }
    
    // create tasks for searching the best move
    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        var tasks = new List<Task<(Turn, double)>>(); 
        var iterationsPerThread = iterationsCount / threadsCount; // iterations per thread
        var remainingIterations = iterationsCount % threadsCount; // remaining iterations

        // Create tasks for each thread
        for (var i = 0; i < threadsCount; i++)
        {
            var currentIterations = iterationsPerThread + (i == threadsCount - 1 ? remainingIterations : 0); //
            var randomSeed = random.Next();
            
            // Add a new task to the list
            tasks.Add(Task.Run(() => 
                SearchBestMove(
                    rocket, 
                    new Random(randomSeed), 
                    currentIterations // number of iterations
                )));
        }
        
        return tasks; // returnlist of tasks
    }
}
