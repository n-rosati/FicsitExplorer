namespace FicsitExplorer
{
    public class Mod
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { set; get; }
        public string LogoURL { get; set; } //TODO: Decide if keep as string or change to URL
        public long Downloads { get; set; }
        public string LastUpdated { set; get; } //TODO: Make this an actual time
        public string DownloadURL { set; get; } //TODO: Make this an array of other versions of the mod
    }
}