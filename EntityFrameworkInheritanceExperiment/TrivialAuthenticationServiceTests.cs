﻿using System;
using System.Linq;
using EntityFrameworkInheritanceExperiment.DTO;
using EntityFrameworkInheritanceExperiment.Service;
using EntityFrameworkInheritanceExperiment.Service.Exceptions;
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
            Assert.That(user.AuthenticationMethods, Has.Count.EqualTo(1));
            Assert.That(user.AuthenticationMethods.Single(), Is.InstanceOf<EmailAuthenticationMethodDTO>());
            Assert.That(user.AuthenticationMethods.OfType<EmailAuthenticationMethodDTO>().Single().AuthenticationMethodId, Is.GreaterThan(0));
            Assert.That(user.AuthenticationMethods.OfType<EmailAuthenticationMethodDTO>().Single().Email, Is.EqualTo("loki2302@loki2302.me"));
            Assert.That(user.AuthenticationMethods.OfType<EmailAuthenticationMethodDTO>().Single().Password, Is.EqualTo("qwerty"));
        }

        [Test]
        [ExpectedException(typeof(EmailAlreadyUsedException))]
        public void CannotSignUpWithTheSameEmailAndPasswordMoreThanOnce()
        {
            Service.SignUpWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
            Service.SignUpWithEmailAndPassword("loki2302@loki2302.me", "qwerty1");
        }

        [Test]
        [ExpectedException(typeof(EmailNotRegisteredException))]
        public void CannotSignInWithoutSignUp()
        {
            Service.SignInWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
        }

        [Test]
        public void CanSignInAfterSigningUp()
        {
            var user1 = Service.SignUpWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
            var user2 = Service.SignInWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
            Assert.That(user1.UserId, Is.EqualTo(user2.UserId));
        }

        [Test]
        [ExpectedException(typeof(IncorrectPasswordException))]
        public void CannotSignInWithIncorrectPassword()
        {
            Service.SignUpWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
            Service.SignInWithEmailAndPassword("loki2302@loki2302.me", "qwerty1");
        }

        [Test]
        public void SigningInWithSameEmailAndPasswordMoreThanOnceResultsInSameUser()
        {
            var user1 = Service.SignUpWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
            
            var user2 = Service.SignInWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
            Assert.That(user1.UserId, Is.EqualTo(user2.UserId));

            var user3 = Service.SignInWithEmailAndPassword("loki2302@loki2302.me", "qwerty");
            Assert.That(user1.UserId, Is.EqualTo(user3.UserId));
        }

        [Test]
        public void CanSignUpWithGoogle()
        {
            var user = Service.AuthenticateWithGoogle("google1", "loki2302@loki2302.me");
            Assert.That(user.UserId, Is.GreaterThan(0));
            Assert.That(user.AuthenticationMethods, Has.Count.EqualTo(1));
            Assert.That(user.AuthenticationMethods.Single(), Is.InstanceOf<GoogleAuthenticationMethodDTO>());
            Assert.That(user.AuthenticationMethods.OfType<GoogleAuthenticationMethodDTO>().Single().AuthenticationMethodId, Is.GreaterThan(0));
            Assert.That(user.AuthenticationMethods.OfType<GoogleAuthenticationMethodDTO>().Single().Email, Is.EqualTo("loki2302@loki2302.me"));
            Assert.That(user.AuthenticationMethods.OfType<GoogleAuthenticationMethodDTO>().Single().GoogleUserId, Is.EqualTo("google1"));
        }

        [Test]
        public void CanSignInWithGoogle()
        {
            var user1 = Service.AuthenticateWithGoogle("google1", "loki2302@loki2302.me");
            var user2 = Service.AuthenticateWithGoogle("google1", "loki2302@loki2302.me");
            Assert.That(user1.UserId, Is.EqualTo(user2.UserId));
        }

        [Test]
        public void CanSignUpWithFacebook()
        {
            var user = Service.AuthenticateWithFacebook("facebook1", "loki2302@loki2302.me");
            Assert.That(user.UserId, Is.GreaterThan(0));
            Assert.That(user.AuthenticationMethods, Has.Count.EqualTo(1));
            Assert.That(user.AuthenticationMethods.Single(), Is.InstanceOf<FacebookAuthenticationMethodDTO>());
            Assert.That(user.AuthenticationMethods.OfType<FacebookAuthenticationMethodDTO>().Single().AuthenticationMethodId, Is.GreaterThan(0));
            Assert.That(user.AuthenticationMethods.OfType<FacebookAuthenticationMethodDTO>().Single().Email, Is.EqualTo("loki2302@loki2302.me"));
            Assert.That(user.AuthenticationMethods.OfType<FacebookAuthenticationMethodDTO>().Single().FacebookUserId, Is.EqualTo("facebook1"));
        }

        [Test]
        public void CanSignInWithFacebook()
        {
            var user1 = Service.AuthenticateWithFacebook("facebook1", "loki2302@loki2302.me");
            var user2 = Service.AuthenticateWithFacebook("facebook1", "loki2302@loki2302.me");
            Assert.That(user1.UserId, Is.EqualTo(user2.UserId));
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
