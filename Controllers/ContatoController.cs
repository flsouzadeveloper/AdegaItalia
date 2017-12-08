using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AdegaItalia.Models;

namespace AdegaItalia.Controllers
{
    public class ContatoController : Controller
    {

        public ActionResult Contato()
        {
            return View();
        }

        public ActionResult _ContatoPartial()
        {
            return PartialView("_ContatoPartial");
        }   

        //public ActionResult SalvarContato(ContatoModel model)
        [HttpPost]
        public async Task<ActionResult> SalvarContato(ContatoModel model)
        {
            string retorno = "";
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p><b>Mensagem:</b></p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("contato@vinhosadegaditalia.com.br"));
                message.From = new MailAddress(model.Email);
                message.Subject = "Contato - Site Adega Itália";
                message.Body = string.Format(body, model.Name, model.Email, model.Mensagem);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "nandoonce97@hotmail.com",  // replace with valid value
                        Password = "c6u4u6"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            }
            ContatoModel contato = new ContatoModel();
            contato = model;
            return PartialView("_ContatoPartial", contato);

        }
    }
}