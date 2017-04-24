using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using MyCalculator.Specs.StepDefinitions;
using SpecFlow.SimpleInjector;
using TechTalk.SpecFlow;

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
            builder.Verify();

            return builder;
        }
    }
}
