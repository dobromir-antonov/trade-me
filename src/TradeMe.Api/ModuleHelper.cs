using SharedKernel.Infrastructure;
using System.Reflection;

namespace TradeMe.Api;

public static class ModuleHelper
{
    public static List<IModule> GetModules(string directoryPath)
    {
        var assemblies = Directory
            .GetFiles(directoryPath, "*.dll")
            .Select(Assembly.LoadFrom);

        var modules = new List<IModule>();

        foreach (var a in assemblies)
        {
            var moduleTypes = a
                .GetTypes()
                .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var moduleType in moduleTypes)
            {
                var module = Activator.CreateInstance(moduleType) as IModule;
                if (module != null)
                {
                    modules.Add(module);
                }
            }
        }

        return modules;
    }

}
