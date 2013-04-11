using System;
using AOPValidationExperiment.Validation;

namespace AOPValidationExperiment
{
    [ValidateParameters]
    public class TestApi
    {
        public virtual void SignIn(
            [UserName] string userName, 
            [Password] string password)
        {
            Console.WriteLine("SignIn: {0}, {1}", userName, password);
        }

        public virtual void SignUp(
            [UserName] string userName, 
            [Password] string password)
        {
            throw new NotImplementedException();
        }

        public virtual void CreatePost(
            [PostTitle] string title, 
            [PostText] string text)
        {
            throw new NotImplementedException();
        }
    }
}