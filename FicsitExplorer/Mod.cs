namespace FicsitExplorer
{
    public class Mod
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string LogoURL { get; set; }
        public long Downloads { get; set; }
        public string LastUpdated { get; set; } //TODO: Make this an actual time; Convert from mm/dd/yyyy
        public string DownloadURL { get; set; } //TODO: Make this an array of other versions of the mod
    }
}