using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestingDemo
{
    public class DefaultUserRepository:IUserRepository
    {
        public void Add(User newUser)
        {
            //
        }

        public User FetchByLoginName(string loginName)
        {
            return new User() { LoginName = loginName };
        }

        public void SubmitChanges()
        {
            //
        }
    }
}
