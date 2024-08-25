using Models;

using Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            //PhoneBookService phoneBookService = new PhoneBookService();
            //List<Contact> contacts = phoneBookService.GetContacts();

            return View( );
        }


        public ActionResult GetContacts()
        {
            PhoneBookService phoneBookService = new PhoneBookService();
            List<Contact> contacts = phoneBookService.GetContacts();
            return Json(contacts, JsonRequestBehavior.AllowGet);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}