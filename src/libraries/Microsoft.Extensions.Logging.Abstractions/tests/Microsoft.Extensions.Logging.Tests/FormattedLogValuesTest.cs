// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Moq;
using Xunit;

namespace Microsoft.Extensions.Logging.Tests
{
    public class FormattedLogValuesTest
    {
        [Fact]
        public void User_Method_Calling_Logging_Delegate_From_LoggerMessage_Define_Should_Call_Logger_Log()
        {
            var logger = new Mock<ILogger<TestProvider>>();
            string expected = Guid.NewGuid().ToString();

            var provider = new TestProvider(logger.Object);
            provider.DoSomething(expected);

            logger.Verify(l => l.Log(LogLevel.Information, 1, null, expected));
        }

        private sealed class TestProvider
        {
            private static readonly Action<ILogger, string, Exception> s_logInfo;

            private readonly ILogger<TestProvider> _logger;

            static TestProvider()
            {
                s_logInfo = LoggerMessage.Define<string>(
                    LogLevel.Information,
                    1,
                    "{0}");
            }

            public TestProvider(ILogger<TestProvider> logger)
            {
                _logger = logger;
            }

            public void DoSomething(string message)
            {
                s_logInfo(_logger, message, null);
            }
        }
    }
}
