using System;
using System.Linq;
using System.IO;
using static System.Net.WebRequestMethods;
using GeoCoordinatePortable;


namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            logger.LogInfo("Log initialized");
            var lines = System.IO.File.ReadAllLines(csvPath);

            if (lines.Length == 0) 
            {
                Exception exception = new Exception("Csv file is empty");
                logger.LogError("Error: ", exception);

            }
            if (lines.Length == 1) 
            {
                Exception exception = new Exception("CSV file has only one line");
                logger.LogWarning($"Warning line length : {lines.Length}");
            }

            var parser = new TacoParser();

            var locations = lines.Select(parser.Parse).ToArray();

            Console.WriteLine();
            Console.WriteLine();
            logger.LogInfo($"Locations parsed. number of locations = {locations.Length}");

            ITrackable tacoBellA = null;
            ITrackable tacoBellB = null;
            double distance = 0.0;

            for (int i = 0; i < locations.Length; i++) 
            {
                var corA = locations[i].Location;

                for (int j = 0; j < locations.Length; j++) 
                {
                   if (j != i) 
                    {
                        var corB = locations[j].Location;
                        GeoCoordinate geo = new GeoCoordinate(corA.Latitude, corA.Longitude);

                        var furthest = geo.GetDistanceTo(new GeoCoordinate(corB.Latitude, corB.Longitude));

                        if (furthest > distance) 
                        {
                            distance = furthest;
                            tacoBellA = locations[i];
                            tacoBellB = locations[j];
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"{tacoBellA.Name} and {tacoBellB.Name} are the furthest from each other");
        }
    }
}
