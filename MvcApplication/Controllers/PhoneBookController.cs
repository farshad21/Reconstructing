using Models;

using Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class PhoneBookController : Controller
    {
        PhoneBookService _service;
        public PhoneBookController()
        {

            _service = new PhoneBookService();
        }

        public ActionResult GetContacts()
        {
            List<Contact> contacts = _service.GetContacts();
            return Json(contacts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContactById(string Id)
        {
            Contact contacts = _service.GetContactById(Id);
            return Json(contacts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Contact model)
        {
            var result = _service.SaveContact(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteById(string Id)
        {
            var result = _service.DeleteContact(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}