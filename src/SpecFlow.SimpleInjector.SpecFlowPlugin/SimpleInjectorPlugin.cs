using BoDi;
using SimpleInjector;
using SpecFlow.SimpleInjector;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.UnitTestProvider;

[assembly: RuntimePlugin(typeof(SimpleInjectorPlugin))]

namespace SpecFlow.SimpleInjector
{
    public class SimpleInjectorPlugin : IRuntimePlugin
    {
        private static object _registrationLock = new object();

        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters,
            UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            runtimePluginEvents.CustomizeGlobalDependencies += (sender, args) =>
            {
                // temporary fix for CustomizeGlobalDependencies called multiple times
                // see https://github.com/techtalk/SpecFlow/issues/948
                if (!args.ObjectContainer.IsRegistered<IContainerBuilderFinder>())
                {
                    // an extra lock to ensure that there are not two super fast threads re-registering the same stuff
                    lock (_registrationLock)
                    {
                        if (!args.ObjectContainer.IsRegistered<IContainerBuilderFinder>())
                        {
                            args.ObjectContainer
                                .RegisterTypeAs<SimpleInjectorTestObjectResolver, ITestObjectResolver>();
                            args.ObjectContainer.RegisterTypeAs<ContainerBuilderFinder, IContainerBuilderFinder>();
                        }
                    }

                    // workaround for parallel execution issue - this should be rather a feature in BoDi?
                    args.ObjectContainer.Resolve<IContainerBuilderFinder>();
                }
            };

            runtimePluginEvents.CustomizeScenarioDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterFactoryAs(() =>
                {
                    var containerBuilderFinder = args.ObjectContainer.Resolve<IContainerBuilderFinder>();
                    var createScenarioContainerBuilder = containerBuilderFinder.GetCreateScenarioContainerBuilder();
                    var container = createScenarioContainerBuilder();
                    RegisterSpecFlowDependencies(args.ObjectContainer, container);
                    container.Verify();
                    return container;
                });
            };
        }

        /// <summary>
        ///     Extracted from
        ///     https://github.com/techtalk/SpecFlow/blob/master/TechTalk.SpecFlow/Infrastructure/ITestObjectResolver.cs
        ///     The test objects might be dependent on particular SpecFlow infrastructure, therefore the implemented
        ///     resolution logic should support resolving the following objects (from the provided SpecFlow container):
        ///     <see cref="ScenarioContext" />, <see cref="FeatureContext" />, <see cref="TestThreadContext" /> and
        ///     <see cref="IObjectContainer" /> (to be able to resolve any other SpecFlow infrastructure). So basically
        ///     the resolution of these classes has to be forwarded to the original container.
        /// </summary>
        /// <param name="objectContainer">SpecFlow DI container.</param>
        /// <param name="container">ASimpleInjector ContainerBuilder.</param>
        private static void RegisterSpecFlowDependencies(
            IObjectContainer objectContainer,
            Container container)
        {
            container.Register(() => objectContainer, Lifestyle.Singleton);
            container.Register(() => container.GetInstance<IObjectContainer>().Resolve<ScenarioContext>(),
                Lifestyle.Singleton);
            container.Register(() => container.GetInstance<IObjectContainer>().Resolve<FeatureContext>(),
                Lifestyle.Singleton);
            container.Register(() => container.GetInstance<IObjectContainer>().Resolve<TestThreadContext>(),
                Lifestyle.Singleton);
        }
    }
}