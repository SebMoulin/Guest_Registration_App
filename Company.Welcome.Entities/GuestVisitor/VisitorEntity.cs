using System;
using SQLite.Net;

namespace Company.Welcome.Entities.GuestVisitor
{
    public class VisitorEntity
    {
        [SQLite.Net.Attributes.PrimaryKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public int Arrival { get; set; }
        public int Departure { get; set; }
        public string TekContact { get; set; }
        public string ImageBytesString { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
    }
}