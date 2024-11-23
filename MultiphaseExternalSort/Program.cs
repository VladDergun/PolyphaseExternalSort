using MultiphaseExternalSort.Methods;

class RandomIntGenerator
{
    private const string inputFile = "unsorted.txt";
    public static void GenerateRandomInputFile(int numberOfElements)
    {
        Random random = new Random();
        using var writer = new StreamWriter(inputFile);

        for (int i = 0; i < numberOfElements; i++)
        {
            writer.WriteLine(random.Next(1, 10000001));
        }
    }

}

class Program
{
    static void Main()
    {
        RandomIntGenerator.GenerateRandomInputFile(1024*100);
        var watch = System.Diagnostics.Stopwatch.StartNew();
        //NormalPolyphaseSort.Sort();
        //ModifiedPolyphasesort.Sort();
        watch.Stop();
        Console.WriteLine("Sorting finished. Sorting time:" + watch.Elapsed.TotalSeconds);
    }
}
