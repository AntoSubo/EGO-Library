//// Models/Sources.cs
//namespace EGO_Library.Models
//{
//    public class Source
//    {
//        public string Location { get; set; }
//        public string Type { get; set; } // MirrorDungeon, Fusion, Event, etc.
//        public int Floor { get; set; }
//        public double DropRate { get; set; }

//        public Source(string location, string type, int floor = 0, double dropRate = 0)
//        {
//            Location = location;
//            Type = type;
//            Floor = floor;
//            DropRate = dropRate;
//        }

//        public override string ToString()
//        {
//            return $"{Location} (Floor {Floor})";
//        }
//    }
//}