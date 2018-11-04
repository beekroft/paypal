using PayPal.Api;
using PaypalMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaypalMVC.Paypal;

namespace PaypalMVC.Controllers
{
    public class PaypalController : Controller
    {
        // GET: Paypal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrderConfirmation()
        {
            ViewBag.result = PDTHolder.Success(Request.QueryString.Get("tx"));
            return View("orderConfirmation");
        }
        private Payment payment;

        private Payment CreatePayment (APIContext apiContext, string redirectURL)
        {
            var listItems = new ItemList() { items = new List<Item>() };
            listItems.items.Add(new Item() {
                name = "Product 1",
                currency = "USD",
                price = "100",
                quantity = "2",
                sku = "sku1"
            });
            listItems.items.Add(new Item()
            {
                name = "Product 2",
                currency = "USD",
                price = "150",
                quantity = "2",
                sku = "sku2"
            });


            var payer = new Payer() { payment_method = "paypal" };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = "",
                return_url = ""
            };

            // Create details object

            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = "30" // quantity * price
            };

            // Create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) +
                    Convert.ToDouble(details.shipping) +
                    Convert.ToDouble(details.subtotal)).ToString()// tax + shipping + subtotal
            };

            // Create transaction
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "testing transaction description",
                invoice_number = Convert.ToString((new Random()).Next(10000)).ToString(),
                amount = amount,
                item_list = listItems
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);

        }

        private Payment ExecutePayment(APIContext apiContex, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContex, paymentExecution);
        }

        // Create paymentWithPaypal method

        public ActionResult PaymentWithPaypal()
        {
            // Get apiContext from paypal using clientId and clientSecret
            APIContext aPIContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerId"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPaypal";
                }
                return View();

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}