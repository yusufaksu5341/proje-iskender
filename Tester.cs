// Derleyiciyi susturmak için
#pragma warning disable CS8618
#pragma warning disable CS8605
#pragma warning disable CA2200
using System.Reflection;

namespace ProjeIskender;

class Tester
{
    private class Testables
    {
        public Type type;
        public MethodInfo[] methods;
    }

    public bool ignoreErrors = true;
    private Testables[] tests;
    
    public void Init()
    {
        Console.WriteLine($"[TestInfo] Errors are ignored");
        List<Testables> testables = new List<Testables>();
        Assembly assembly = Assembly.GetExecutingAssembly();
        var testableClasses = assembly.GetTypes().Where(x => x.GetCustomAttribute<TestableClass>() != null);

        foreach (var x in testableClasses)
        {
            var methods = x.GetMethods().Where(x => x.GetCustomAttribute<TestCase>() != null).ToArray();
            var testInit = x.GetMethod("TestInit");
            if (testInit is not null)
            {
                try 
                {
                    testInit.Invoke(null, new object[] { });
                }
                catch (Exception e)
                {
                    Console.Write("[Test] ");
                    ColoredWrite("Error", ConsoleColor.Red);
                    Console.WriteLine($" on {x.Name} -> TestInit! Message: {e.Message}");
                }
            }
            testables.Add(new Testables()
            {
                type = x,
                methods = methods
            });
        }

        tests = testables.ToArray();
    }

    // Kullanım Dışı!
    public static void Run(Type type, bool ignoreErrors = true)
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
            try 
            {
                if ((bool)x.Invoke(null, new object[] { }))
                {
                    Console.WriteLine($"[Test] {className} {methodName} Passed!");
                    passedCount++;
                    continue;
                }
                Console.WriteLine($"[Test] {className} {methodName} Failed!");
            } 
            catch (Exception e) 
            {
                if (ignoreErrors) 
                {
                    Console.Write($"[Test] {className} {methodName} ");
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Error!");
                    Console.ResetColor();
                }
                else
                {
                    throw e;
                }
            }
        }
        Console.WriteLine($"In all {className} tests, {passedCount}/{methods.Count()} Passed!");
    }

    // Kullanım Dışı!
    public static void Run<T>(bool ignoreErrors = true)
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
                try 
                {
                    if ((bool)method.Invoke(null, new object[] { }))
                    {
                        Console.Write($"[Test] {className} -> {methodName} ");
                        ColoredWriteLine("Passed!", ConsoleColor.Green);
                        passedCount++;
                        continue;
                    }
                    Console.Write($"[Test] {className} -> {methodName} ");
                    ColoredWriteLine("Failed!", ConsoleColor.Red);
                }
                catch (Exception e) 
                {
                    if (ignoreErrors) 
                    {
                        Console.Write($"[Test] {className} -> {methodName} ");
                        ColoredWriteLine("Error!", ConsoleColor.DarkRed);
                    }
                    else 
                    {
                        throw e;
                    }
                }
            }
            Console.WriteLine($"In all {className} tests, {passedCount}/{x.methods.Count()} Passed!");
        }
    }

    private static void ColoredWriteLine(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private static void ColoredWrite(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
}
