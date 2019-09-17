using System;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) {
                Console.WriteLine("Please provide a single argument, the path to the json config file.");
            }
            try 
            {
                var crawlPath = args[0];
                var jsonContent = File.ReadAllText(crawlPath);
                var settings = JsonConvert.DeserializeObject<Settings>(jsonContent);
                var directoryPath = Path.GetDirectoryName(crawlPath);

                if (!File.Exists(settings.chromePath)) 
                {
                    throw new Exception ("Cannot find chrome at " + settings.chromePath);
                }

                foreach(var action in settings.actions)
                {
                    try 
                    {
                        var tempFilePath = Path.GetTempFileName();;

                        Process process = new Process();
                        // Configure the process using the StartInfo properties.
                        process.StartInfo.FileName = settings.chromePath;

                        process.StartInfo.Arguments = 
                            $"--headless --disable-gpu --incognito "  +
                            $"--window-size={action.size.x},{action.size.y} " + 
                            $"--screenshot=\"{tempFilePath}\" " +
                            action.url;

                        //Console.Write("\"" + settings.chromePath + "\" ");
                        //Console.WriteLine(process.StartInfo.Arguments);

                        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        process.StartInfo.WorkingDirectory = directoryPath;

                        process.Start();
                        System.Threading.Thread.Sleep(10000);

                        try 
                        {
                            process.Kill();
                        } 
                        catch (Exception) 
                        {
                        }

                        if (process.ExitCode != 0)
                        {
                            throw new Exception("chrome exit code returned " + process.ExitCode);
                        }

                        if (File.Exists(tempFilePath))
                        {
                            try 
                            {
                                using (var image = Image.Load(tempFilePath))
                                {
                                    image.Mutate
                                    (
                                        x => x
                                            .Crop
                                            (
                                                new SixLabors.Primitives.Rectangle
                                                (
                                                    action.crop.topLeftCorner.x,
                                                    action.crop.topLeftCorner.y,
                                                    action.crop.size.x, 
                                                    action.crop.size.y
                                                )   
                                            )
                                    );

                                    image.Save(action.path);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }

                            File.Delete(tempFilePath);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
