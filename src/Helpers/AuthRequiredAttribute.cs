using Bytehide.CLI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bytehide.CLI.Helpers
{
    public enum AuthService
    {
        Shield,
        Secrets,
        ByteHide
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AuthRequiredAttribute : Attribute
    {
        internal AuthService Service;

        public AuthRequiredAttribute(AuthService service = AuthService.Shield)
        {
            Service = service;
        }
    }    
}
