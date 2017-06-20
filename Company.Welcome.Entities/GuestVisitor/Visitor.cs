using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Company.Welcome.Commons;

namespace Company.Welcome.Entities.GuestVisitor
{
    public class Visitor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public string TekContact { get; set; }
        public ImageSource SignatureImage { get; set; }
        public Signature Signature { get; set; }
    }
}