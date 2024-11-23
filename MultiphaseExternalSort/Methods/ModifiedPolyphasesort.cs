namespace MultiphaseExternalSort.Methods
{
    public class ModifiedPolyphasesort
    {
        private const int MAX_RAM_PER_CHUNK = 100 * 1024 * 1024;
        private const int MAX_CHUNK_ELEMENTS = MAX_RAM_PER_CHUNK / sizeof(int);
        private const string TempDirectory = "temp";
        private const string inputFile = "unsorted.txt";
        private const string outputFile = "sorted.txt";

        public static void Sort()
        {
            if (!Directory.Exists(TempDirectory))
            {
                Directory.CreateDirectory(TempDirectory);
            }

            var tempFiles = SplitAndSort();
            Merge(tempFiles);
        }

        private static List<string> SplitAndSort()
        {
            var tempFiles = new List<string>();
            var lines = new List<int>();
            string? line;

            using (var reader = new StreamReader(inputFile))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(int.Parse(line));

                    if (lines.Count >= MAX_CHUNK_ELEMENTS)
                    {
                        lines.Sort();
                        var tempFilePath = Path.Combine(TempDirectory, $"{Guid.NewGuid()}.txt");
                        File.WriteAllLines(tempFilePath, lines.Select(x => x.ToString()));

                        tempFiles.Add(tempFilePath);
                        lines.Clear();
                    }
                }
            }

            if (lines.Count > 0)
            {
                lines.Sort();
                var tempFilePath = Path.Combine(TempDirectory, $"{Guid.NewGuid()}.txt");
                File.WriteAllLines(tempFilePath, lines.Select(x => x.ToString()));
                tempFiles.Add(tempFilePath);
            }

            return tempFiles;
        }
        private static void Merge(List<string> tempFiles)
        {
            var priorityQueue = new PriorityQueue<(int value, StreamReader reader), int>();
            var fileReaders = new List<StreamReader>();

            foreach (var tempFile in tempFiles)
            {
                var reader = new StreamReader(tempFile);
                fileReaders.Add(reader);

                var line = reader.ReadLine();
                if (line != null)
                {
                    int value = int.Parse(line);
                    priorityQueue.Enqueue((value, reader), value);
                }
            }
            using (var writer = new StreamWriter(outputFile))
            {
                while (priorityQueue.Count > 0)
                {
                    var (minValue, reader) = priorityQueue.Dequeue();
                    writer.WriteLine(minValue);

                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        int newValue = int.Parse(line);
                        priorityQueue.Enqueue((newValue, reader), newValue);
                    }
                }
            }

            foreach (var reader in fileReaders)
            {
                reader.Close();
            }

            foreach (var file in tempFiles)
            {
                File.Delete(file);
            }
        }
    }
}
