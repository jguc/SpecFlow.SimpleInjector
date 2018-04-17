# SpecFlow.SimpleInjector
SpecFlow plugin for using SimpleInjector as a dependency injection framework for step definitions.


Currently supports
* SpecFlow v2.2
* SimpleInjector v3.0 or above

License: Apache (https://github.com/jguc/SpecFlow.SimpleInjector/blob/master/LICENSE)

NuGet: https://www.nuget.org/packages/SpecFlow.SimpleInjector

[![Build status](https://ci.appveyor.com/api/projects/status/17s8ujeigojldjfn/branch/master?svg=true)](https://ci.appveyor.com/project/jguc/specflow-simpleinjector/branch/master)
[![NuGet version](https://badge.fury.io/nu/SpecFlow.SimpleInjector.svg)](https://badge.fury.io/nu/SpecFlow.SimpleInjector)

## Usage

Install plugin from NuGet into your SpecFlow project.

    PM> Install-Package SpecFlow.SimpleInjector
  
Create a static method somewhere in the SpecFlow project (recommended to put it into the `Support` folder) that returns an SimpleInjector `Container` and tag it with the `[ScenarioDependencies]` attribute. Configure your dependencies for the scenario execution within the method.

A typical dependency builder method probably looks like this:

    [ScenarioDependencies]
    public static Container CreateContainer()
    {
      // create container with the runtime dependencies
      var builder = Dependencies.CreateContainer();

      //TODO: add customizations, stubs required for testing
	  builder.Verify();
      
      return builder;
    }

## Plugin based on 

http://gasparnagy.com/2016/08/specflow-tips-customizing-dependency-injection-with-autofac/

## Release history

#### v1.1.0

* Support for SpecFlow v2.2 ([PR#2](https://github.com/jguc/SpecFlow.SimpleInjector/pull/2) by [davidvesely](https://github.com/davidvesely))
* Updated test project to reflect version changes (thanks to [dariusz-wozniak](https://github.com/dariusz-wozniak))

#### v1.0.2

* First release supporting SpecFlow v2.1
