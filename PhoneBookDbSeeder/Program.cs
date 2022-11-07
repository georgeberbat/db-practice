namespace PhoneBookDbSeeder
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            return SeedRunner.Seed(args);
        }
    }
}