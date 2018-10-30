using System;

namespace hyphenApp
{
    public class Redemption
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Points { get; set; }
        public string Source { get; set; }
        public bool Selected { get; set; }

        public Redemption()
        {
        }
    }
}