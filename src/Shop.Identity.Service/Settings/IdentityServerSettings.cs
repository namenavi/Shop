//using IdentityServer4.Models;
using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;

namespace Shop.Identity.Service.Settings
{
    public class IdentityServerSettings
    {
        public IReadOnlyCollection<ApiScope> ApiScopes { get; init; }
        public IReadOnlyCollection<ApiResource> ApiResources { get; init; }
        public IReadOnlyCollection<Client> Clients { get; init; }
        public IReadOnlyCollection<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                ////это означает, что всякий раз, когда пользователь запрашивает идентификационный токен, мы говорим, что он также должен вернуть утверждение ролей.
                new IdentityResource("roles", new[] {"role"})
            };
    }
}
