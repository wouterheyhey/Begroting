﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebApi.Models
{
    public class ExternalLoginData
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string UserName { get; set; }
        public string ExternalAccessToken { get; set; }

        public static ExternalLoginData FromIdentity (ClaimsIdentity identity)
        {
            if (identity ==null)
            {
                return null;
            }

            Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (providerKeyClaim == null || string.IsNullOrEmpty(providerKeyClaim.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
            {
                return null;
            }

            if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
            {
                return null;
            }

            return new ExternalLoginData
            {
                LoginProvider = providerKeyClaim.Issuer,
                ProviderKey = providerKeyClaim.Value,
                UserName = identity.FindFirst(ClaimTypes.Name).Value,
                ExternalAccessToken = identity.FindFirst("ExternalAccessToken").Value
            };
        }

    }
}