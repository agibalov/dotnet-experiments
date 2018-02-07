using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiHelpPageExperiment.Models;

namespace WebApiHelpPageExperiment.Controllers
{
    public class MyApiController : ApiController
    {
        [Description("Returns something useful sometimes")]
        [HttpGet]
        [ActionName("GetThing")]
        public int GetThing(
            [Description("Well... Id of something. Have no idea.")] int id)
        {
            return 123;
        }

        [Description("Saves things. Sometimes.")]
        [HttpPost]
        [ActionName("SaveThing")]
        public void SaveThing(
            [Description("Thing identifier")] int id, 
            [Description("Thing description")] string description)
        {
        }

        [Description("Retrieves user by given id")]
        [HttpGet]
        [ActionName("GetUser")]
        public User GetUser(
            [Description("User identifier")] int userId)
        {
            return null;
        }

        [Description("Save user")]
        [HttpPost]
        [ActionName("SaveUser")]
        public User SaveUser(
            [Description("Id of something, I don't care")] int somethingId, 
            [Description("User details")] User user)
        {
            return null;
        }
    }
}
