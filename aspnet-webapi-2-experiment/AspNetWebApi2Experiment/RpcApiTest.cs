using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FluentValidation;
using NUnit.Framework;
using Ninject;
using Owin;
using RestSharp;

namespace AspNetWebApi2Experiment
{
    public class RpcApiTest : AbstractWebApiTest
    {
        [Test]
        public void CanGetSessionTokenWhenThereAreNoErrors()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("SignInWithEmailAndPassword", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
            request.AddBody(new SignInWithEmailAndPasswordRequestDto
                {
                    Email = "loki2302@loki2302.me",
                    Password = "qwerty"
                });
            var response = restClient.Execute<EffectiveResultDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.Data.Ok);
            Assert.IsNotNullOrEmpty(response.Data.Payload.SessionToken);
            Assert.IsNull(response.Data.FieldsInError);
        }

        [Test]
        public void CanGetValidationError()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("SignInWithEmailAndPassword", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(new SignInWithEmailAndPasswordRequestDto
            {
                Email = null,
                Password = "qwerty"
            });
            var response = restClient.Execute<EffectiveResultDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsFalse(response.Data.Ok);
            Assert.IsNull(response.Data.Payload);
            Assert.AreEqual(1, response.Data.FieldsInError.Count);
            Assert.AreEqual("Email", response.Data.FieldsInError.Single().Key);
            Assert.AreEqual(1, response.Data.FieldsInError.Single().Value.Count);
        }

        [Test]
        public void CanGetNonValidationError()
        {
            var restClient = new RestClient(BaseAddress);

            var request = new RestRequest("SignInWithEmailAndPassword", Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(new SignInWithEmailAndPasswordRequestDto
            {
                Email = "loki2302@loki2302.me",
                Password = "crash123"
            });
            var response = restClient.Execute<EffectiveResultDto>(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsFalse(response.Data.Ok);
            Assert.IsNull(response.Data.Payload);
            Assert.IsNull(response.Data.FieldsInError);
        }

        protected override void SetUpWebApi(IAppBuilder appBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "{controller}", new { action = "Process" });

            var kernel = new StandardKernel();

            AssemblyScanner
                .FindValidatorsInAssembly(typeof(Program).Assembly)
                .ForEach(validator => kernel
                    .Bind(validator.InterfaceType)
                    .To(validator.ValidatorType)
                    .InSingletonScope());

            httpConfiguration.Filters.Add(new ActionExecutionDecoratorFilterAttribute(kernel));

            appBuilder.UseWebApi(httpConfiguration);
        }

        public class SignInWithEmailAndPasswordController : ApiController
        {
            public AuthenticationResultDto Process(SignInWithEmailAndPasswordRequestDto request)
            {
                if (request.Password == "crash123")
                {
                    throw new Exception("CRASH!!!11");
                }

                return new AuthenticationResultDto
                    {
                        SessionToken = Guid.NewGuid().ToString()
                    };
            }
        }

        public class SignInWithEmailAndPasswordRequestDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class AuthenticationResultDto
        {
            public string SessionToken { get; set; }
        }

        public class ResultDto
        {
            public bool Ok { get; set; }
            public object Payload { get; set; }
            public IDictionary<string, List<string>> FieldsInError { get; set; }
        }

        public class EffectiveResultDto
        {
            public bool Ok { get; set; }
            public AuthenticationResultDto Payload { get; set; }
            public Dictionary<string, List<string>> FieldsInError { get; set; }
        }

        public class ActionExecutionDecoratorFilterAttribute : FilterAttribute, IActionFilter
        {
            private readonly IKernel _kernel;

            public ActionExecutionDecoratorFilterAttribute(IKernel kernel)
            {
                _kernel = kernel;
            }

            public async Task<HttpResponseMessage> ExecuteActionFilterAsync(
                HttpActionContext actionContext,
                CancellationToken cancellationToken,
                Func<Task<HttpResponseMessage>> continuation)
            {
                try
                {
                    ValidateRequest(actionContext);
                }
                catch (ValidationException e)
                {
                    var validationFailures = e.Errors;

                    var validationFailureGroups = validationFailures
                        .GroupBy(failure => failure.PropertyName)
                        .ToDictionary(
                            errorGroup => errorGroup.Key,
                            errorGroup => errorGroup.Select(g => g.ErrorMessage).ToList());
                    var result = new ResultDto
                        {
                            Ok = false,
                            Payload = null,
                            FieldsInError = validationFailureGroups
                        };

                    return actionContext.Request.CreateResponse(HttpStatusCode.OK, result);
                }

                try
                {
                    var response = await continuation();
                    var objectContent = (ObjectContent)response.Content;
                    var result = new ResultDto
                        {
                            Ok = true,
                            Payload = objectContent.Value
                        };

                    return actionContext.Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception e)
                {
                    return actionContext.Request.CreateResponse(HttpStatusCode.OK, new ResultDto
                        {
                            Ok = false
                        });
                }
            }

            private void ValidateRequest(HttpActionContext actionContext)
            {
                foreach (var key in actionContext.ActionArguments.Keys)
                {
                    var argument = actionContext.ActionArguments[key];
                    var argumentType = argument.GetType();
                    var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                    var validator = (IValidator)_kernel.TryGet(validatorType);
                    if (validator == null)
                    {
                        continue;
                    }

                    var validationResult = validator.Validate(argument);
                    if (validationResult.IsValid)
                    {
                        continue;
                    }

                    throw new ValidationException(validationResult.Errors);
                }
            }
        }

        public class SignInRequestValidator : AbstractValidator<SignInWithEmailAndPasswordRequestDto>
        {
            public SignInRequestValidator()
            {
                RuleFor(signInRequest => signInRequest.Email).GoodEmail();
                RuleFor(signInRequest => signInRequest.Password).GoodPassword();
            }
        }
    }

    public static class ValidationConfiguration
    {
        public static IRuleBuilder<T, string> GoodEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .EmailAddress()
                .Length(5, 128);
        }

        public static IRuleBuilder<T, string> GoodPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Length(6, 32);
        }
    }
}