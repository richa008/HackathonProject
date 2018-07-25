using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MirysList.Models;
using MirysList.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MirysList.Authorization
{
    public class FbLoginValidationHandler : AuthorizationHandler<FbLoginRequirement>
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FbLoginRequirement requirement)
        {
            if (context.Resource is AuthorizationFilterContext mvc)
            {
                // extract the authorization header from the request
                string authHeaderValue = string.Empty;
                if (mvc.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValues))
                {
                    authHeaderValue = authHeaderValues[0];
                }

                // return 401 if auth header is not found
                if (string.IsNullOrWhiteSpace(authHeaderValue))
                {
                    context.Fail();
                    // SetResponse(mvc, 401, Resources.NoAuthHeaderInRequest);    
                    return Task.CompletedTask;
                }

                //string authCode = string.Empty;
                //if (mvc.HttpContext.Request.Headers.TryGetValue("AuthCode", out StringValues authCodeValues))
                //{
                //    authCode = authCodeValues[0];
                //}
                
                /*
                 * We expect the OAuth signed_request of the logged in Facebook user, in the Authorization header
                 * We split the encoded signature and the encoded payload and check if the hmac of the payload matched the received signature
                 * 
                 * Then, we validate the access token to:
                 * Verify that it is a valid Facebook OAuth access token
                 * To get the user and app details
                 * 
                 * The above steps prove that the request has arrived from a valid logged in facebook user.
                 */

                // split the signature and payload on a '.'
                //string[] pieces = authHeaderValue.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                //if (pieces.Length != 2)
                //{
                //    context.Fail();
                //    // mvc.Result = new UnauthorizedResult();
                //    SetResponse(mvc, 401, Resources.InvalidAuthHeader);
                //    return Task.CompletedTask;
                //}

                // decode the payload to get the user access token, user id and the hmac algorithm used in the signature
                //string decodedPayload = Base64UrlDecode(pieces[1]);
                //FbRequestPayload fbPayload = JsonConvert.DeserializeObject<FbRequestPayload>(decodedPayload);

                //if (!fbPayload.Algorithm.Equals("HMAC-SHA256", StringComparison.InvariantCultureIgnoreCase))
                //{
                //    context.Fail();
                //    SetResponse(mvc, 401, "Hash algorithm not supported");
                //    return Task.CompletedTask;
                //}

                // verify if the hmac of the payload matches the received signature
                // bool isValid = ValidateSignedRequest(pieces[0], pieces[1]);
                //if (!isValid)
                //{
                //    context.Fail();
                //    SetResponse(mvc, 401, Resources.InvalidAuthHeader);
                //    return Task.CompletedTask;
                //}

                // exchange the access code for a user access token
                //string requestUriFormat = $"https://graph.facebook.com/v3.0/oauth/access_token?client_id={{0}}&redirect_uri={{1}}&client_secret={{2}}&code={{3}}";

                string clientId = "2112677595641957";
                
                //string clientSecret = "";
                //string redirectUri = "https://localhost:44334/"; // the client app's URL
                //string encodedUrl = WebUtility.UrlEncode(redirectUri);
                ////string requestUri = string.Format(requestUriFormat, clientId, redirectUri, clientSecret, fbPayload.AuthCode);
                //string requestUri = string.Format(requestUriFormat, clientId, redirectUri, clientSecret, authCode);

                //HttpRequestMessage accessTokenRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
                //HttpResponseMessage response = HttpClient.SendAsync(accessTokenRequest).Result;
                //string content = response.Content.ReadAsStringAsync().Result;
                //FbCodeExchangeParsed parsedCodeExchange = JsonConvert.DeserializeObject<FbCodeExchangeParsed>(content);

                // Verify the received access token and retrieve user_id and app information
                // TODO: PUT THIS IN KV!!
                string appAccessToken = "2112677595641957|z0TQzEk--Ibx3C71BJnS1Rcg0xA";
                // string debugTokenUrl = $"https://graph.facebook.com/v3.0/debug_token?input_token={parsedCodeExchange.AccessToken}&access_token={appAccessToken}";
                string debugTokenUrl = $"https://graph.facebook.com/v3.0/debug_token?input_token={authHeaderValue}&access_token={appAccessToken}";

                HttpResponseMessage response = HttpClient.GetAsync(debugTokenUrl).Result;
                string content = response.Content.ReadAsStringAsync().Result;
                JObject parsedJson = JObject.Parse(content);
                FbAccessTokenParsed parsedAccessToken = null;
                if (parsedJson != null)
                {
                    JToken dataToken = parsedJson.GetValue("data");
                    if (dataToken != null)
                    {
                        parsedAccessToken = JsonConvert.DeserializeObject<FbAccessTokenParsed>(dataToken.ToString());
                    }
                }

                // Added for debugging
                //context.Succeed(requirement);
                //return Task.CompletedTask;

                if (parsedAccessToken == null || string.IsNullOrWhiteSpace(parsedAccessToken.UserId) || string.IsNullOrWhiteSpace(parsedAccessToken.AppId))
                {
                    context.Fail();
                    // SetResponse(mvc, 401, Resources.InvalidAuthHeader);
                    return Task.CompletedTask;
                }

                if (!string.Equals(parsedAccessToken.AppId, clientId))
                {
                    context.Fail();
                    // SetResponse(mvc, 401, Resources.InvalidAuthHeader);
                    return Task.CompletedTask;
                }

                mvc.HttpContext.User = new Principal { Id = parsedAccessToken.UserId };
                Thread.CurrentPrincipal = mvc.HttpContext.User;

                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            else
            {
                throw new Exception("MVC context does not exist");
            }
        }

        private static string Base64UrlDecode(string input)
        {
            byte[] decodedInputBytes = WebEncoders.Base64UrlDecode(input);
            string decodedInput = Encoding.UTF8.GetString(decodedInputBytes);
            return decodedInput;
        }

        private static string Base64UrlEncode(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            string encodedInput = WebEncoders.Base64UrlEncode(inputBytes);
            return encodedInput;
        }

        private bool ValidateSignedRequest(string expectedSignature, string payload)
        {
            // TODO: PUT THIS IN KV!!!
            string appSecret = "d5d0b3fb2d34384637e089795a4c4186";
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
            byte[] keyBytes = Encoding.UTF8.GetBytes(appSecret);
            byte[] payloadHmacBytes;
            using (HMACSHA256 algorithm = new HMACSHA256(keyBytes))
            {
                algorithm.ComputeHash(payloadBytes);
                payloadHmacBytes = algorithm.Hash;
            }

            // compare hex representations of both the expected signature and the computed hash
            string payloadHmacHex = BitConverter.ToString(payloadHmacBytes).Replace("-", "").ToLower();
            string signatureHex = BitConverter.ToString(WebEncoders.Base64UrlDecode(expectedSignature)).Replace("-", "").ToLower();

            return payloadHmacHex == signatureHex;
        }

        private void SetResponse(AuthorizationFilterContext mvc, int statusCode, string responseMessage)
        {
            if (mvc == null)
            {
                throw new ArgumentNullException(nameof(mvc));
            }

            mvc.Result = new JsonResult(responseMessage);
            mvc.HttpContext.Response.StatusCode = 401;
            mvc.HttpContext.Response.ContentType = "application/json";
            // mvc.HttpContext.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseMessage)));
        }
    }
}
