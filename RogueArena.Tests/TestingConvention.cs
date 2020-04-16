namespace RogueArena.Tests
{
    using Fixie;

    public class TestingConvention : Discovery, Execution
    {
        public TestingConvention()
        {
            Classes.Where(x => x.Name.StartsWith("When") || x.Name.EndsWith("Tests"));
        }

        public void Execute(TestClass testClass)
        {
            if (testClass.Type.Name.StartsWith("When"))
            {
                ExecuteIntegration(testClass);
            }
            else if (testClass.Type.Name.EndsWith("Tests"))
            {
                ExecuteUnit(testClass);
            }
        }

        public void ExecuteIntegration(TestClass testClass)
        {
            var instance = testClass.Construct();

            testClass.RunCases(@case => { @case.Execute(instance); });

            instance.Dispose();
        }

        public void ExecuteUnit(TestClass testClass)
        {
            testClass.RunCases(
                @case =>
                {
                    var instance = testClass.Construct();
                    @case.Execute(instance);
                    instance.Dispose();
                });
        }
    }
}
