using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace CH.Test.MiddlewareTests
{
    public class TestEndMiddleware : OwinMiddleware
    {
        public bool isInvoked = false;

        public TestEndMiddleware()
            : base(null)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            isInvoked = true;
            return Task.FromResult(0);
        }
    }
}
