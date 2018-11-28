namespace ModernBOSShopApp.Secret
{
    public class FastScanResult
    {
        public string Name { private set; get; }
        public string TimeTook { private set; get; }

        public FastScanResult(string name, string timeTook)
        {
            Name = name;
            TimeTook = timeTook;
        }
    }
}