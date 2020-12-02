using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Orleans.Security.IntegrationTests.Configuration;
using Orleans.Security.IntegrationTests.GrainsForTests;

namespace Orleans.Security.IntegrationTests
{
    [Author("Karen Tazayan"), Category("IntegrationTest")]
    [SingleThreaded, TestFixture]
    public class AuthorizationTests
    {
        [Description(@"User or Client must have all the roles specified.")]
        [Test]
        public async Task GrainCall_WithCombinedRolesWithRequiredClaims_ShouldPass()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.AliceSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            var privateData = await grain.TakeForCombinedRoles("Some private data.");

            // Assert
            privateData.Should().Be("Some private data.");
        }

        [Description(@"Authorization must fail if email not verified.")]
        [Test]
        public void GrainCall_WithEmailVerifiedRequirementWhenFalse_ShouldThrowException()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.BobSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            async Task<string> Action()
            {
                return await grain.TakeForEmailVerifiedPolicy("Some private data.");
            }

            // Assert
            Assert.ThrowsAsync<NotAuthorizedException>(Action);
        }

        [Description(@"Authorization must be a success if the email successfully verified.")]
        [Test]
        public async Task GrainCall_WithEmailVerifiedRequirementWhenTrue_ShouldPass()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.AliceSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            var privateData = await grain.TakeForEmailVerifiedPolicy("Some private data.");

            // Assert
            privateData.Should().Be("Some private data.");
        }

        [Description(@"Any call to a grain method protected by FemaleAdminPolicy when required credentials 
absent should fail.")]
        [Test]
        public void GrainCall_WithFemaleAdminPolicyWithoutRequiredCredentials_ShouldThrowException()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.BobSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            // Assert
            Assert.ThrowsAsync<NotAuthorizedException>(async () =>
            {
                await grain.TakeForFemaleAdminPolicy("Some private data.");
            });
        }

        [Description(@"Any call to a grain method protected by FemaleAdminPolicy with required credentials 
should be success.")]
        [Test]
        public async Task GrainCall_WithFemaleAdminPolicyWithRequiredClaims_ShouldPass()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.AliceSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            var privateData = await grain.TakeForFemaleManagerPolicy("Some private data.");

            // Assert
            Assert.AreEqual("Some private data.", privateData);
        }

        [Description(@"Any call to a protected grain method without required credentials 
should throw an NotAuthorizedException.")]
        [Test]
        public void GrainCall_WithFemaleManagerPolicyWithoutRequiredClaims_ShouldThrowException()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.BobSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            // Assert
            Assert.ThrowsAsync<NotAuthorizedException>(async () =>
            {
                await grain.TakeForFemaleManagerPolicyAndAdminRole("Some private data.");
            });
        }

        [Description(@"Any call to a protected grain method with required credentials should be passed.")]
        [Test]
        public async Task GrainCall_WithFemaleManagerPolicyWithRequiredCredentials_ShouldPass()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.AliceSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            var privateData = await grain.TakeForFemaleManagerPolicy("Some private data.");

            // Assert
            Assert.AreEqual("Some private data.", privateData);
        }

        [Description(@"Any call to a protected grain method without required credentials
should throw an NotAuthorizedException.")]
        [Test]
        public void GrainCall_WithoutRequiredCredentials_ShouldThrowException()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.BobSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            // Assert
            Assert.ThrowsAsync<NotAuthorizedException>(async () =>
            {
                await grain.TakePrivateData("Some private data.");
            });
        }

        [Description(@"Any call to a protected grain method with required credentials should be passed.")]
        [Test]
        public async Task GrainCall_WithRequiredCredentials_ShouldPass()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.AliceSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            var privateData = await grain.TakePrivateData("Some private data.");

            // Assert
            Assert.AreEqual("Some private data.", privateData);
        }

        [Description(@"Any call to a grain method when AllowAnonymousAttribute applied should be successful.")]
        [Test]
        public async Task GrainCallWhenAllowAnonymousApplied_WithAnyCredentials_ShouldPass()
        {
            // Arrange
            FakeAccessTokenVerifier.LoggedInUser = LoggedInUser.AliceSmith;
            var client = TestClientBuilder.Client;
            var grain = client.GetGrain<IAuthorizationTestGrain>("Test");

            // Act
            var privateData = await grain.TakePublicData("Some private data.");

            // Assert
            Assert.AreEqual("Some private data.", privateData);
        }
    }
}