using System;
using System.Diagnostics;
using System.Web.Mvc;
using Messages.Commands;
using NServiceBus.Azure.Transports.WindowsAzureServiceBus;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var data = string.Format("SafeRoleEnvironment.CurrentRoleName={0} SafeRoleEnvironment.CurrentRoleInstanceId={1} ",
                SafeRoleEnvironment.CurrentRoleName,
                SafeRoleEnvironment.CurrentRoleInstanceId);

            Trace.WriteLine("-+-+-+-+ Sending message to worker from " +data);

            MvcApplication.Bus.Send(new PokeWorker
            {
                LocalDateTime = DateTime.Now,
                Data = "sent command from webapp. Details below \n" + data
            });

            return View();
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