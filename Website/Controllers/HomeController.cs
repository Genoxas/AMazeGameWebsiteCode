using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*
 * Author: Brian Tat
 * Description: Used to display Views like the Home Page and API Help
 */

namespace Website.Controllers
{
  //HomeController Class
  public class HomeController : Controller
  {

    //ActionResult for the Home Page
    public ActionResult Index()
    {
      ViewBag.Title = "Home Page";

      return View();
    }

    //ActionResult for the API Code Page
    public ActionResult APIHelp()
    {
        ViewBag.Title = "API Help";

        return View();
    }
  }
}
