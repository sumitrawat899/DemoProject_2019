using Data;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class SecurityUserManager
    {
        private IRepository<SecurityUser> repoSecurityUser;
        public SecurityUserManager()
        {
            this.repoSecurityUser = new Repository<SecurityUser>(new DemoEntityContext());
        }

        public void AddUser(SecurityUser entity)
        {
            repoSecurityUser.Add(entity);
        }
        public bool IsEmailExist(string Email)
        {
            return repoSecurityUser.IsEntityExists(user => user.Email == Email);
        }
        public SecurityUser GetUserByEmail(string Email)
        {
            return repoSecurityUser.Find(user => user.Email == Email);
        }

    }
}
