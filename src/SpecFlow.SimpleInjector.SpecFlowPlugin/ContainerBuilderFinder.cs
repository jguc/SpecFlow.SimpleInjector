using System;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using TechTalk.SpecFlow.Bindings;

namespace SpecFlow.SimpleInjector
{
    public class ContainerBuilderFinder : IContainerBuilderFinder
    {
        private readonly IBindingRegistry _bindingRegistry;
        private readonly Lazy<Func<Container>> _createScenarioContainerBuilder;

        public ContainerBuilderFinder(IBindingRegistry bindingRegistry)
        {
            this._bindingRegistry = bindingRegistry;
            _createScenarioContainerBuilder = new Lazy<Func<Container>>(FindCreateScenarioContainerBuilder, true);
        }

        public Func<Container> GetCreateScenarioContainerBuilder()
        {
            var builder = _createScenarioContainerBuilder.Value;
            if (builder == null)
                throw new InvalidOperationException("Unable to find scenario dependencies! Mark a static method that returns a SimpleInjector.Container with [ScenarioDependencies]!");
            return builder;
        }

        protected virtual Func<Container> FindCreateScenarioContainerBuilder()
        {
            var assemblies = _bindingRegistry.GetBindingAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(m => Attribute.IsDefined((MemberInfo) m, typeof(ScenarioDependenciesAttribute))))
                    {
                        return () => (Container)methodInfo.Invoke(null, null);
                    }
                }
            }
            return null;

            //return (assemblies
            //    .SelectMany(assembly => assembly.GetTypes(), (_, type) => type)
            //    .SelectMany(
            //        type => type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(m => Attribute.IsDefined(m, typeof (ScenarioDependenciesAttribute))),
            //        (_, methodInfo) => (Func<ContainerBuilder>) (() => (ContainerBuilder) methodInfo.Invoke(null, null)))).FirstOrDefault();
        }
    }
}