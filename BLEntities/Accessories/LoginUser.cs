using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLEntities.Accessories
{
    public class LoginUser
    {
        private Operator userOperator = null;

        public int IdUser { get; set; }
        public bool IsSuperViser { get; set; }
        public string UserName { get; set; }

        public Operator UserOperator 
        { 
           get 
           {
            if (userOperator == null)
              {
                userOperator = new Operator();
                
              }
            return userOperator; 
            }
            set 
            {
                userOperator = value; 
            }
        }
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public string  FirmName { get; set; }
        public string  Telephone { get; set; }

        public List<Operator> ActiveDirectotyOperatorList { get; set; }
        
    }
}
