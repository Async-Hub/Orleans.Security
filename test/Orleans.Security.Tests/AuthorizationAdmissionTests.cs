using System;
using NSubstitute;
using Xunit;
using System.Reflection;
using System.Reflection.Emit;
using Orleans.Security.Authorization;

namespace Orleans.Security.Tests
{
    public class AuthorizationAdmissionTests
    {
        [Fact]
        public void WhenAllowAnonymousAttributeAppliedToTheGrainMethodAuthorizationShouldBeSkipped()
        {
            var grainCallContext = Substitute.For<IGrainCallContext>();
            var methodInfo = Substitute.For<MethodInfo>();
            grainCallContext.InterfaceMethod.Returns(methodInfo);

            grainCallContext.InterfaceMethod
                .GetCustomAttributes<AllowAnonymousAttribute>()
                .Returns(new[] {new AllowAnonymousAttribute()});

            Assert.False(AuthorizationAdmission.IsRequired(grainCallContext));
        }
    }
}
