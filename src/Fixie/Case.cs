﻿namespace Fixie
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// A test case being executed, representing a single call to a test method.
    /// </summary>
    public class Case : BehaviorContext
    {
        readonly List<Exception> exceptions;

        public Case(Type testClass, MethodInfo caseMethod, params object[] parameters)
        {
            Parameters = parameters != null && parameters.Length == 0 ? null : parameters;
            Class = testClass;

            Method = caseMethod.TryResolveTypeArguments(parameters);

            Name = CaseNameBuilder.GetName(Class, Method, Parameters);

            exceptions = new List<Exception>();
        }

        /// <summary>
        /// Gets the name of the test case, including any input parameters.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the test class in which this test case is defined.
        /// </summary>
        public Type Class { get; }

        /// <summary>
        /// Gets the method that defines this test case.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// For parameterized test cases, gets the set of parameters to be passed into the test method.
        /// For zero-argument test methods, this property is null.
        /// </summary>
        public object[] Parameters { get; }

        /// <summary>
        /// Gets all of the exceptions that have contributed to this test case's failure.
        ///
        /// <para>
        /// A single test case could have multiple exceptions, for instance, if the
        /// test case method throws an exception, and a subsequent behavior such as
        /// test class Dispose() also throws an exception.
        /// </para>
        ///
        /// <para>
        /// The first encountered exception is considered the primary cause of the test
        /// failure, and secondary exceptions are included for diagnosing any subsequent
        /// complications.
        /// </para>
        /// </summary>
        public IReadOnlyList<Exception> Exceptions => exceptions;

        /// <summary>
        /// Include the given exception in the running test case's list of exceptions, indicating test case failure.
        /// </summary>
        public void Fail(Exception reason)
        {
            var wrapped = reason as PreservedException;

            if (wrapped != null)
                exceptions.Add(wrapped.OriginalException);
            else
                exceptions.Add(reason);
        }

        /// <summary>
        /// Clear all of the test case's exceptions, indicating test success.
        /// </summary>
        public void ClearExceptions()
        {
            exceptions.Clear();
        }

        /// <summary>
        /// The fixture (test class instance) under which the test case is executing.
        /// </summary>
        public Fixture Fixture { get; internal set; }

        /// <summary>
        /// The object returned by the invocation of the test case method.
        /// </summary>
        public object ReturnValue { get; internal set; }

        internal TimeSpan Duration { get; set; }
        internal string Output { get; set; }
    }
}
