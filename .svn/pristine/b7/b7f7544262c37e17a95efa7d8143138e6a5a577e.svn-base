using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WikiRh.Extensions
{
    public static class CheckGroup
    {
        public static bool IsInGroup(this ClaimsPrincipal User, string GroupName)
        {
            var groups = new List<string>();

            var wi = (WindowsIdentity)User.Identity;
            if (wi.Groups != null)
            {
                foreach (var group in wi.Groups)
                {
                    groups.Add(group.Translate(typeof(NTAccount)).ToString());
                }
                foreach (string group in GroupName.Split(","))
                {
                    if (groups.Contains(group))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
