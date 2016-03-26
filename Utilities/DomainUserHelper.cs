using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace Utilities
{
    public class DomainUserHelper
    {
       
       private PrincipalContext insPrincipalContext = null;
       public DomainUserHelper()
        {
            insPrincipalContext = new PrincipalContext(ContextType.Machine);
            //PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Domain, "adalya03.local", "DC=MyDomain,DC=com"); //domaine bağlanma
            //PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Machine,"TAMERO","administrator","password");//lokal bilgisayara bir kullanıcı ile bağlanma
        }

       public DomainUserHelper(string domain,string dc)
       {
           insPrincipalContext = new PrincipalContext(ContextType.Domain, domain, dc); //domaine bağlanma
           //PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Domain, "MyDomain", "DC=MyDomain,DC=com"); 
       }
       public DomainUserHelper(string machine, string user, string password)
       {
           insPrincipalContext = new PrincipalContext(ContextType.Machine, machine, user, password);
           //PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Machine, "TAMERO", "administrator", "password");
       }


        public List<string> GetUserGroups(string user)
        {
            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(insPrincipalContext,user);
            List<string> results = new List<string>();
            PrincipalSearchResult<Principal> groups = userPrincipal.GetGroups();// userPrincipal.GetAuthorizationGroups();
            foreach (Principal g in groups)
            {
               results.Add(g.Name);
            }
            return results;
        }
        public List<string> GetUserGroups()
        {
            string strUsr = CurrentSettingsHelper.GetLoginUserName();// WindowsIdentity.GetCurrent().Name;
            
            return GetUserGroups(strUsr);
        }
        
    }
}

