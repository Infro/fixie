﻿using System;
using System.Reflection;
using TestDriven.Framework;

namespace Fixie.TestDriven
{
    public class TdNetRunner : ITestRunner
    {
        public TestRunState RunAssembly(ITestListener testListener, Assembly assembly)
        {
            return Run(testListener, runner => runner.RunAssembly(assembly));
        }

        public TestRunState RunNamespace(ITestListener testListener, Assembly assembly, string ns)
        {
            return Run(testListener, runner => runner.RunNamespace(assembly, ns));
        }

        public TestRunState RunMember(ITestListener testListener, Assembly assembly, MemberInfo member)
        {
            var method = member as MethodInfo;
            if (method != null)
                return Run(testListener, runner => runner.RunMethod(method));

            var type = member as Type;
            if (type != null)
                return Run(testListener, runner => runner.RunTypes(type));

            return TestRunState.Error;
        }

        public TestRunState Run(ITestListener testListener, Func<Runner, Result> run)
        {
            var listener = new TestDrivenListener(testListener);
            var runner = new Runner(listener);
            var result = run(runner);

            if (result.Total == 0)
                return TestRunState.NoTests;

            if (result.Failed > 0)
                return TestRunState.Failure;

            return TestRunState.Success;
        }
    }
}