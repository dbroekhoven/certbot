namespace Shared.Models
{
    public class TestDomain
    {
        public string Name { get; set; }
        public bool Passed { get; set; }
        public string Error { get; set; }

        public static TestDomain From(string name)
        {
            return new TestDomain
            {
                Name = name
            };
        }
    }
}