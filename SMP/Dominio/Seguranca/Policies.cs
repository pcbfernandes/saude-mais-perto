using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMP.Dominio.Seguranca
{
    public static class Policies
    {
        public const string IsAdmin = "IsAdmin";

        public static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Administradores")
                                                   .Build();
        }
    }
}
