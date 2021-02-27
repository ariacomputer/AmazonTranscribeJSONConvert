using System;
using System.IO;
using System.Text.Json;

namespace AmazonTranscribeJSONConvert
{
    enum ExitCode: int
    {
        Success = 0,
        FileNotFound = 1,
        InvalidPath = 2,
        ReadError = 3,
        ParseError = 4,
        GenericError = 127
    }
    class Program
    {
        static int Main(string[] arguments) 
        {
            foreach (String filePath in arguments)
            {
                if (filePath.IndexOfAny(Path.GetInvalidPathChars()) != -1) {
                    Console.WriteLine("Error: {0} is not a valid path", filePath);
                    return (int)ExitCode.InvalidPath;
                }
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Error: {0} not found", filePath);
                    return (int)ExitCode.FileNotFound;
                }
            }

            foreach (String filePath in arguments)
            { 
                try
                {
                    string fileContents = File.ReadAllText(filePath);
                    AmazonTranscribeJSONFile transFile = new AmazonTranscribeJSONFile(fileContents);
                    foreach(string transcript in transFile.Transcripts)
                    {
                        Console.WriteLine(transcript);
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Error: {0} not found", filePath);
                    return (int)ExitCode.FileNotFound;
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error reading {0}: {1}", filePath, ex.Message.ToString());
                    return (int)ExitCode.ReadError;
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Error: {0} does not appear to be a proper transcript file", filePath);
                    return (int)ExitCode.ParseError;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0} on {1}", ex.Message.ToString(), filePath);
                    return (int)ExitCode.GenericError;
                }
            }
            return (int)ExitCode.Success;
        }
    }
}
