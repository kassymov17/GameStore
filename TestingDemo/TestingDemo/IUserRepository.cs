using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestingDemo
{
    public interface IUserRepository
    {
        void Add(User newUser);
        User FetchByLoginName(string loginName);
        void SubmitChanges();
    }
}
