using System.Reflection;

namespace ProjeIskender;

class Tester
{
    private class Testables
    {
        public Type type;
        public MethodInfo[] methods;
    }

    private Testables[] tests;
    
    public void Init()
    {
        List<Testables> testables = new List<Testables>();
        Assembly assembly = Assembly.GetExecutingAssembly();
        var testableClasses = assembly.GetTypes().Where(x => x.GetCustomAttribute<TestableClass>() != null);

        foreach (var x in testableClasses)
        {
            var methods = x.GetMethods().Where(x => x.GetCustomAttribute<TestCase>() != null).ToArray();
            var testInit = x.GetMethod("TestInit");
            if (testInit is not null)
            {
                testInit.Invoke(null, new object[] { });
            }
            testables.Add(new Testables()
            {
                type = x,
                methods = methods
            });
        }

        tests = testables.ToArray();
    }

    public static void Run(Type type)
    {
        if (type.GetCustomAttribute<TestableClass>() == null)
        {
            return;
        }

        var methods = type.GetMethods().Where(x => x.GetCustomAttribute<TestCase>() != null);
        var testInit = type.GetMethods().FirstOrDefault(x => x.GetCustomAttribute<TestInit>() != null);
        if (testInit is null)
        {
            testInit = type.GetMethod("TestInit");
        }
        if (testInit is not null)
        {
            testInit.Invoke(null, new object[] { });
        }

        var className = type.Name;
        int passedCount = 0;
        foreach (var x in methods)
        {
            var methodName = x.Name;
            if ((bool)x.Invoke(null, new object[] { }))
            {
                Console.WriteLine($"[Test] {className} {methodName} Passed!");
                passedCount++;
                continue;
            }
            Console.WriteLine($"[Test] {className} {methodName} Failed!");
        }
        Console.WriteLine($"In all {className} tests, {passedCount}/{methods.Count()} Passed!");
    }

    public static void Run<T>()
    {
        Run(typeof(T));
    }

    public void RunAll()
    {
        foreach (var x in tests)
        {
            var className = x.type.Name;
            int passedCount = 0;
            foreach (var method in x.methods)
            {
                var methodName = method.Name;
                if ((bool)method.Invoke(null, new object[] { }))
                {
                    Console.WriteLine($"[Test] {className} {methodName} Passed!");
                    passedCount++;
                    continue;
                }
                Console.WriteLine($"[Test] {className} {methodName} Failed!");
            }
            Console.WriteLine($"In all {className} tests, {passedCount}/{x.methods.Count()} Passed!");
        }
    }
}
