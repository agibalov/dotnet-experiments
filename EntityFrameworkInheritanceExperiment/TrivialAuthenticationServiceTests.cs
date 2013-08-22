using System;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DTO;
using NUnit.Framework;

namespace EntityFrameworkInheritanceExperiment
{
    public class TrivialAuthenticationServiceTests : AbstractAuthenticationServiceTests
    {
        [Test]
        public void ThereAreNoUsersByDefault()
        {
            Assert.That(Service.GetUserCount(), Is.EqualTo(0));
        }

        [Test]
        public void CanSignUpWithEmailAndPassword()
        {
            var user = Service.SignUpWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
            Assert.That(user.UserId, Is.GreaterThan(0));
            Assert.That(user.AuthenticationMethods, Has.Length.EqualTo(1));
            Assert.That(user.AuthenticationMethods.Single(), Is.InstanceOf<EmailPasswordAuthenticationMethodDTO>());
            Assert.That(user.AuthenticationMethods.OfType<EmailPasswordAuthenticationMethodDTO>().Single().AuthenticationMethodId, Is.GreaterThan(0));
            Assert.That(user.AuthenticationMethods.OfType<EmailPasswordAuthenticationMethodDTO>().Single().Email, Is.EqualTo("loki2302@loki2302.me"));
            Assert.That(user.AuthenticationMethods.OfType<EmailPasswordAuthenticationMethodDTO>().Single().Password, Is.EqualTo("qwerty"));
        }

        [Test]
        public void CannotSignUpWithTheSameEmailAndPasswordMoreThanOnce()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CannotSignInWithoutSignUp()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanSignInAfterSigningUp()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void SigningInWithSameEmailAndPasswordMoreThanOnceResultsInSameUser()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanSignUpWithGoogle()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanSignInWithGoogle()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanSignUpWithFacebook()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanSignInWithFacebook()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanSignUpWithTwitter()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanSignInWithTwitter()
        {
            throw new NotImplementedException();
        }
    }
}
