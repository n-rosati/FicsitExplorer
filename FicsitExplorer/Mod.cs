using System;

namespace FicsitExplorer
{
    public class Mod
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { set; get; }
        public Uri LogoURL { get; set; }
        public long Downloads { get; set; }
        public string LastUpdated { set; get; } //TODO: Make this an actual time; Convert from mm/dd/yyyy
        public string DownloadURL { set; get; } //TODO: Make this an array of other versions of the mod
    }
}