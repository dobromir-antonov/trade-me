using SharedKernel.Infrastructure;
using Assembly = System.Reflection.Assembly;

namespace ArchitectureTests;

public abstract class BaseTest
{
    protected static IEnumerable<Assembly> DomainAssemblies => _modules.Select(m => m.GetDomainAssembly());
    protected static IEnumerable<Assembly> ApplicationAssemblies => _modules.Select(m => m.GetApplicationAssembly());
    protected static IEnumerable<Assembly> InfrastructureAssemblies => _modules.Select(m => m.GetInfrasturctureAssembly());

    private static List<IModule> _modules = GetModules(AppDomain.CurrentDomain.BaseDirectory);

    private static List<IModule> GetModules(string directoryPath)
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