using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace TestingDemo
{
    class AdminController:Controller
    {
        private IUserRepository repository;

        public AdminController(IUserRepository repo)
        {
            repository = repo;
        }

        public ActionResult ChangeLoginName(string oldName,string newName)
        {
            User user = repository.FetchByLoginName(oldName);
            user.LoginName = newName;
            repository.SubmitChanges();
            return View();
        }
    }
}
