using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ServiceBus;
using Microsoft.Azure.Management.ServiceBus.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace MattsWorld.TellUs.Api.Controllers
{
    [Route("topics")]
    public class TopicsController : Controller
    {
        private string _tenantId = "4d13380f-b2dc-401e-ade8-03a67801c676";
        private string _clientId = "b0eb183c-0004-4058-a90b-8feffdb2ced7";
        private string _clientSecret = "Bj8HugLBVdSlzAcJMjYDfuZZLMg56+csd3Tu7xN9aKs=";
        private string _subscriptionId = "42c164b1-f9ec-4bb0-acc7-854aa327428f";

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SBTopic>), 200)]
        public async Task<IActionResult> Get()
        {
            string token = await GetToken();

            var creds = new TokenCredentials(token);
            var sbClient = new ServiceBusManagementClient(creds)
            {
                SubscriptionId = _subscriptionId
            };

            var topics = new List<SBTopic>();

            var namespaces = await sbClient.Namespaces.ListAsync();
            foreach (var ns in namespaces)
            {
                var ts = await sbClient.Topics.ListByNamespaceAsync("MattsWorld-TellUs", ns.Name);

                foreach (var topic in ts)
                {
                    topics.Add(topic);
                }
            }
            
            return Ok(topics);
        }


        private async Task<string> GetToken()
        {
            var context = new AuthenticationContext($"https://login.microsoftonline.com/{_tenantId}");
            var result = await context.AcquireTokenAsync("https://management.core.windows.net/", new ClientCredential(_clientId, _clientSecret));
            return result.AccessToken;
        }
    }
}
