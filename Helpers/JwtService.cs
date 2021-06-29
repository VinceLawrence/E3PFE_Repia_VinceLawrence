using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Api.Helpers
{
    public class JwtService
    {
        private string secureKey = "this is a very secure key"; //Default key to use for jwt encryption
        public string Generate(int id)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey)); //Getting a security symmetric key using an enconded default secured key
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature); //Specifying the symmetric key and the algorithm for encryption
            var header = new JwtHeader(credentials); //Using the credentials for the header of the jwt

            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1)); //Passing the issuer and expiration to the payload

            var securityToken = new JwtSecurityToken(header, payload); //The token by combining the header and the payload

            return (new JwtSecurityTokenHandler().WriteToken(securityToken)); //Returning the security token
        }

        public JwtSecurityToken Verify(string jwt) //Used for verifying if the credentials are correct
        {
            var tokenHandler = new JwtSecurityTokenHandler(); //Creating a new jwtsecurity handler
            var key = Encoding.ASCII.GetBytes(secureKey); 
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            },
                out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}
