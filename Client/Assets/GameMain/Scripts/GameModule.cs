using System;
using LGF.MVC;

public class GameModule
{
    public static void InitController()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.BaseType == typeof(LGFController))
                {
                    var controller = Activator.CreateInstance(type) as LGFController;
                    if (controller != null) controller.Init();
                }
            }
        }
    }
}