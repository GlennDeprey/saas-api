using System.Reflection;

namespace Saas.Modules.Events.Application;

public class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
