using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.Collections;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace Utilities
{
    public class ActiveDirectoryHelper
    {




        public static List<string> GetUserGroups(string ldapAddress, string queryUsername, string queryPassword, string requestUsername, string defaultGroup)
        {
            List<string> groups = new List<string>();
            DirectoryEntry oDE = null;
            //oDE = new DirectoryEntry("LDAP://adalya03.local", "genadib", "gb1477", AuthenticationTypes.Secure);
            if (string.IsNullOrEmpty(defaultGroup))
                oDE = new DirectoryEntry(ldapAddress, queryUsername, queryPassword, AuthenticationTypes.Secure);
            else
            {

                return new List<string> { defaultGroup };

                //DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer");
                //DirectoryEntry admGroup = localMachine.Children.Find("administrators", "group");
                //object members = admGroup.Invoke("members", null);

                //foreach (object groupMember in (IEnumerable)members)
                //{
                //    DirectoryEntry member = new DirectoryEntry(groupMember);
                //    if (Environment.UserName.Equals(member.Name))
                //    {
                //        groups.Add("administrators");
                //        return groups;
                //    }

                //}
                //return groups;
            } 

            

            if (oDE != null)
            {

                var srch = new DirectorySearcher(oDE, "(sAMAccountName=" + requestUsername + ")");
                SearchResult res = null;
                try
                {
                    res = srch.FindOne();
                }
                catch
                {
                    return null;
                }
                if (null != res)
                {
                    DirectoryEntry obUser = new DirectoryEntry(res.Path);
                    // Invoke Groups method.
                    object obGroups = obUser.Invoke("Groups");
                    foreach (object ob in (IEnumerable)obGroups)
                    {
                        // Create object for each group.
                        DirectoryEntry obGpEntry = new DirectoryEntry(ob);
                        if (obGpEntry.Name.IndexOf("=") > -1)
                        {
                            string[] array = obGpEntry.Name.Split("=".ToCharArray());
                            groups.Add(array[array.Length - 1]);
                        }
                    }
                }
            }
            return groups;
        }
    }
}
