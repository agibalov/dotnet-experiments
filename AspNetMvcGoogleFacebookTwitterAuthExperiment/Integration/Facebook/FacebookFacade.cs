using Facebook;

namespace AspNetMvcGoogleFacebookTwitterAuthExperiment.Integration.Facebook
{
    public class FacebookFacade
    {
        private readonly string _facebookApplicationId;
        private readonly string _facebookApplicationSecret;

        public FacebookFacade(
            string facebookApplicationId,
            string facebookApplicationSecret)
        {
            _facebookApplicationId = facebookApplicationId;
            _facebookApplicationSecret = facebookApplicationSecret;
        }

        public FacebookUserInfo GetUserInfo(string accessToken)
        {
            var facebookClient = new FacebookClient
            {
                AppId = _facebookApplicationId,
                AppSecret = _facebookApplicationSecret,
                AccessToken = accessToken
            };

            dynamic me = facebookClient.Get("me?fields=first_name,last_name,id,email");

            return new FacebookUserInfo
                {
                    UserId = me.id,
                    Email = me.email,
                    FirstName = me.first_name,
                    LastName = me.last_name
                };
        }
    }
}