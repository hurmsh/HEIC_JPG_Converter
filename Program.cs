using ImageMagick;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string heicDirectory = "C:\\AppleImages\\HEIC\\";
        string outputDirectory = "C:\\AppleImages\\JPG\\";
        string processedDirectory = "C:\\AppleImages\\Processed\\";

        

        TimeSpan totalRunTime = TimeSpan.FromMinutes(4);
        TimeSpan delayTime = TimeSpan.FromSeconds(3);

        DateTime endTime = DateTime.Now.Add(totalRunTime);

        while (DateTime.Now < endTime)
        {
            string[] files = Directory.GetFiles(heicDirectory, "*.heic");
            try
            {
                foreach (var file in files)
                {
                    // Process each file
                    ProcessFile(file, outputDirectory, processedDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            await Task.Delay(delayTime);
        }
    }

    static void ProcessFile(string inputFilePath, string outputDirectory,string processedDirectory)
    {
        try
        {
            string fileName = Path.GetFileNameWithoutExtension(inputFilePath);
            string outputFilePath = Path.Combine(outputDirectory, $"{fileName}.jpg");
            string processedFilePath = Path.Combine(processedDirectory, Path.GetFileName(inputFilePath));

            

            // Load the HEIC file
            using (var image = new MagickImage(inputFilePath))
            {
                // Convert to JPEG and save
                image.Format = MagickFormat.Jpeg;
                image.Write(outputFilePath);
                Console.WriteLine($"Image successfully converted: {outputFilePath}");
            }
            // Move the original file to the "Processed" folder
            File.Move(inputFilePath, processedFilePath);
            Console.WriteLine($"File moved to processed folder: {processedFilePath}");
        }
        catch (Exception ex)
        {
            string ErrorPath = "C:\\AppleImages\\PROCESSED\\ERROR_" + Path.GetFileName(inputFilePath);
            File.Move(inputFilePath, ErrorPath);
            Console.WriteLine($"Error processing file {inputFilePath}: {ex.Message}");
        }
    }
}