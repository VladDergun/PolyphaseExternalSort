namespace MultiphaseExternalSort.Methods
{
    internal class NormalPolyphaseSort
    {
        private const string InputFile = "unsorted.txt";
        private const string OutputFile = "sorted.txt";
        private const int NUMBER_OF_CHUNKS = 2;

        public static void Sort()
        {
            try
            {
                if (!File.Exists(InputFile))
                {
                    Console.WriteLine("Input file does not exist.");
                    return;
                }

                var lines = File.ReadAllLines(InputFile).ToList();

                var chunks = SplitIntoChunks(lines);

                var sortedChunks = SortChunksInMemory(chunks);
                Merge(sortedChunks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during sorting: {ex.Message}");
            }
        }

        private static List<List<int>> SplitIntoChunks(List<string> lines)
        {
            var numbers = lines.Select(line => int.Parse(line)).ToList();

            int chunkSize = (int)Math.Ceiling((double)numbers.Count / NUMBER_OF_CHUNKS);

            var chunks = new List<List<int>>();

            for (int i = 0; i < NUMBER_OF_CHUNKS; i++)
            {
                var chunk = numbers.Skip(i * chunkSize).Take(chunkSize).ToList();
                chunks.Add(chunk);
            }

            return chunks;
        }


   
        private static List<List<int>> SortChunksInMemory(List<List<int>> chunks)
        {
            foreach (var chunk in chunks)
            {
                chunk.Sort();
            }

            return chunks;
        }

        private static void Merge(List<List<int>> sortedChunks)
        {
            var priorityQueue = new PriorityQueue<(int value, int chunkIndex, int elementIndex), int>();

            for (int i = 0; i < sortedChunks.Count; i++)
            {
                if (sortedChunks[i].Any()) 
                {
                    priorityQueue.Enqueue((sortedChunks[i][0], i, 0), sortedChunks[i][0]);
                }
            }

            var result = new List<int>();

            while (priorityQueue.Count > 0)
            {
                var (value, chunkIndex, elementIndex) = priorityQueue.Dequeue();
                result.Add(value);

                if (elementIndex + 1 < sortedChunks[chunkIndex].Count)
                {
                    var nextValue = sortedChunks[chunkIndex][elementIndex + 1];
                    priorityQueue.Enqueue((nextValue, chunkIndex, elementIndex + 1), nextValue); 
                }
            }

            try
            {
                File.WriteAllLines(OutputFile, result.Select(x => x.ToString()));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to output file: {ex.Message}");
            }
        }

    }
}
