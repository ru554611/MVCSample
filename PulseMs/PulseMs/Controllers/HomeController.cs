using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PulseMs.Models;

namespace PulseMs.Controllers
{
    public class HomeController : Controller
    {
        MVCSampleContext db = new MVCSampleContext();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetView(string value)
        {
            string result = "";
            if (value == "1")
            {
                result = RenderPartialToString("PersonalInfo", db.Tables.FirstOrDefault(), this.ControllerContext);
            }
            else if (value == "2")
            {
                result = RenderPartialToString("AddressInfo", db.Tables.FirstOrDefault(), this.ControllerContext);
            }
            else
            {
                result = RenderPartialToString("PaymentInfo", db.Tables.FirstOrDefault(), this.ControllerContext);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,CellphoneNumber,Address,City,ProvinceCode,PostalCode,CreditCardType,CreditCardNumber,ExpiryDate,CardHolderName")] Table table, int? formId)
        {
            var tbl = db.Tables.FirstOrDefault();

            if (formId == 1)
            {
                tbl.FirstName = table.FirstName;
                tbl.LastName = table.LastName;
                tbl.CellphoneNumber = table.CellphoneNumber;
            }
            else if (formId == 2)
            {
                tbl.Address = table.Address;
                tbl.City = table.City;
                tbl.ProvinceCode = table.ProvinceCode;
                tbl.PostalCode = table.PostalCode;
            }
            else
            {
                tbl.CreditCardType = table.CreditCardType;
                tbl.CreditCardNumber = table.CreditCardNumber;
                tbl.ExpiryDate = table.ExpiryDate;
                tbl.CardHolderName = table.CardHolderName;
            }

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tbl).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error on update: " + ex.Message);
                TempData["message"] = "Ops! There is an error: " + ex.Message;
            }
            return View("Index");
        }

        // Copied from:
        // http://stackoverflow.com/questions/2537741/how-to-render-partial-view-into-a-string
        public static string RenderPartialToString(string viewName, object model, ControllerContext ControllerContext)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewDataDictionary ViewData = new ViewDataDictionary();
            TempDataDictionary TempData = new TempDataDictionary();
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }

        }
    }
}