using SimpleInjector;
using SpecFlow.SimpleInjector;

namespace MyCalculator.Specs.Support
{
    public static class TestDependencies
    {
        [ScenarioDependencies]
        public static Container CreateContainerBuilder()
        {
            // create container with the runtime dependencies
            var builder = Dependencies.CreateContainerBuilder();

            //TODO: add customizations, stubs required for testing

            return builder;
        }
    }
}
