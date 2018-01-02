using System;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using Newtonsoft.Json.Serialization;

namespace GhabzinoBot
{
    public sealed class SnakeCaseNamingAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            controllerSettings.Formatters.Insert(0, new JsonMediaTypeFormatter { SerializerSettings = { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } } });
        }
    }
}