using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    //Creating data transfer objects register
    public class RegisterDto
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
    }
}
