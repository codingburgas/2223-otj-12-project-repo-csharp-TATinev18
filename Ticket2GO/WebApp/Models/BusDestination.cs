using System;

namespace WebApp.Models
{
    public class BusDestination
    {
        public Guid BusId { get; set; }
        public Bus Bus { get; set; }

        public Guid DestinationId { get; set; }
        public Destination Destination { get; set; }
    }
}