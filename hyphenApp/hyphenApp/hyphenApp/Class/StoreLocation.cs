using System;

namespace hyphenApp
{
    public class StoreLocation
    {
        public string GPS { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Distance { get; set; }
        public string Source { get; set; }
        public bool Selected { get; set; }

        public StoreLocation()
        {
        }
    }
}