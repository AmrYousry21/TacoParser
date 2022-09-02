using System;

namespace LoggingKata
{
    /// <summary>
    /// Parses a POI file to locate all the Taco Bells
    /// </summary>
    public class TacoParser
    {
        readonly ILog logger = new TacoLogger();
        
        public ITrackable Parse(string line)
        {
            logger.LogInfo("Begin parsing");

            var cells = line.Split(',');

            if (cells.Length < 3)
            {
                Exception exception = new Exception("something went wrong array length is less than 3");
                logger.LogError($"Error: ", exception);
                return null;
            }
       
            var latitude = double.Parse(cells[0]);
            var longitude = double.Parse(cells[1]);
            var name = cells[2];

            TacoBell store = new TacoBell();

            store.Name = name;
            Point point = new Point();
            point.Latitude = latitude;
            point.Longitude = longitude;
            store.Location = point;

            return store;
        }
    }
}