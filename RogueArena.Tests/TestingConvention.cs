namespace RogueArena.Tests
{
    using Fixie;

    public class TestingConvention : Discovery, Execution
    {
        public TestingConvention()
        {
            Classes.Where(x => x.Name.StartsWith("When"));
        }

        public void Execute(TestClass testClass)
        {
            var instance = testClass.Construct();

            testClass.RunCases(@case =>
            {
                @case.Execute(instance);
            });

            instance.Dispose();
        }
    }
}
