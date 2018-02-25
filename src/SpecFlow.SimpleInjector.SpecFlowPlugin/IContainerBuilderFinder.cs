using System;
using SimpleInjector;

namespace SpecFlow.SimpleInjector
{
    public interface IContainerBuilderFinder
    {
        Func<Container> GetCreateScenarioContainerBuilder();
    }
}