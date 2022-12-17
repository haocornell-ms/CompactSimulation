
namespace Compaction
{
    internal class Program
    {
        static readonly int TotalLevels = 11;
        static int[] levelFileCountLimit = new int[TotalLevels];
        static int[] levelFileCount = new int[TotalLevels];
        static readonly int FileSizeMb = 64; // MB
        
        static void Main(string[] args)
        {
            levelFileCountLimit[0] = 4;
            int magnifyFactor = 10;
            for (int i=1;i<levelFileCountLimit.Length;i++)
            {
                levelFileCountLimit[i] = magnifyFactor;
                magnifyFactor *= 10;

                levelFileCount[i] = 0;
            }

            int totalSize = 0;
            // only simulate add
            // print out total size
            // every time, generate 4 64MB file

            long readSize = 0; // mb
            long writeSize = 0; // mb

            int sizeToWriteMb = 500 * 1000 * 1000; // 500TB
            int loopCount = sizeToWriteMb / FileSizeMb / levelFileCountLimit[0];
            for (int i = 0; i < loopCount; i++ )
            {
                totalSize += levelFileCountLimit[0] * FileSizeMb;
                readSize += levelFileCount[1] * FileSizeMb;
                writeSize += (levelFileCount[1] + levelFileCountLimit[0]) * FileSizeMb;

                levelFileCount[1] += levelFileCountLimit[0];

                int level = 1;
                while(level < TotalLevels -1 && levelFileCount[level] > levelFileCountLimit[level])
                {
                    //overflow. merge with next level
                    int extra = levelFileCount[level] - levelFileCountLimit[level];
                    levelFileCount[level] = levelFileCountLimit[level];

                    int extraTraffic = extra * 11 * FileSizeMb;
                    readSize += extraTraffic;
                    writeSize +=  extraTraffic;

                    levelFileCount[level + 1] += extra;
                    level++;
                }
                if (level == TotalLevels -1 && levelFileCount[level] > levelFileCountLimit[level]) {
                    Console.WriteLine($"level overflow. level = {level}, file count at this level is {levelFileCount[level]}");
                }
                Console.WriteLine($"total size is {totalSize} MB");
            }

            Console.WriteLine($"read size is {readSize} MB");
            Console.WriteLine($"write size is {writeSize} MB");
            Console.WriteLine($"total size is {totalSize} MB");
            for(int i=0;i<TotalLevels;i++)
            {
                Console.WriteLine($"level file size {levelFileCount[i] * FileSizeMb} MB");
            }
        }
    }
}