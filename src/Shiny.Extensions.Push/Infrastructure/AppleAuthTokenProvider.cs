using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;


namespace Shiny.Extensions.Push.Infrastructure
{
    //https://stackoverflow.com/questions/43870237/how-to-implement-apple-token-based-push-notifications-using-p8-file-in-c
    public class AppleAuthTokenProvider : IAppleAuthTokenProvider
    {
        readonly Dictionary<string, JwtToken> tokens = new Dictionary<string, JwtToken>();
        readonly object syncLock = new object();


        public virtual string GetAuthToken(AppleConfiguration config)
        {
            if (!this.tokens.ContainsKey(config.TeamId) || this.tokens[config.TeamId].Expires < DateTime.UtcNow)
            {
                lock (this.syncLock)
                {
                    if (!this.tokens.ContainsKey(config.TeamId) || this.tokens[config.TeamId].Expires < DateTime.UtcNow)
                    {
                        var tokenValue = this.CreateJwtToken(config);
                        this.tokens.Add(config.TeamId, new JwtToken(tokenValue, DateTime.UtcNow.AddMinutes(config.JwtExpiryMinutes))); // cannot be less than 20 or great than 60
                    }
                }
            }
            return this.tokens[config.TeamId].Value;
        }


        protected virtual string CreateJwtToken(AppleConfiguration config)
        {
            var privateKey = this.GetECDsa(config.Key);
            var securityKey = new ECDsaSecurityKey(privateKey) { KeyId = config.KeyId };
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                IssuedAt = DateTime.Now,
                Issuer = config.TeamId,
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();
            var encodedToken = handler.CreateEncodedJwt(descriptor);
            return encodedToken;
        }


        protected ECDsa GetECDsa(string key)
        {
            var reader = new StringReader(key);
            var pemReader = new PemReader(reader);
            var ecPrivateKeyParameters = (ECPrivateKeyParameters)pemReader.ReadObject();

            var q = ecPrivateKeyParameters.Parameters.G.Multiply(ecPrivateKeyParameters.D).Normalize();
            var qx = q.AffineXCoord.GetEncoded();
            var qy = q.AffineYCoord.GetEncoded();
            var d = ecPrivateKeyParameters.D.ToByteArrayUnsigned();

            // Convert the BouncyCastle key to a Native Key.
            var msEcp = new ECParameters { Curve = ECCurve.NamedCurves.nistP256, Q = { X = qx, Y = qy }, D = d };
            return ECDsa.Create(msEcp);
        }
    }
}
