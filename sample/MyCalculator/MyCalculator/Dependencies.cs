using System;
using System.Linq;
using SimpleInjector;

namespace MyCalculator
{
    public static class Dependencies
    {
        public static Container CreateContainerBuilder()
        {
            var builder = new Container();

            builder.Register<ICalculator, Calculator>(Lifestyle.Singleton);

            return builder;
        }
    }
}
