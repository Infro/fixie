﻿namespace Fixie.Tests
{
    public class SelfTestConvention : Convention
    {
        public SelfTestConvention()
        {
            Fixtures
                .Where(fixtureClass => fixtureClass.IsNestedPrivate)
                .NameEndsWith("Fixture");

            Cases
                .Where(method => method.Void() || method.Async())
                .ZeroParameters();
        }
    }
}