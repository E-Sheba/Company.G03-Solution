using Company.G03.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Company.G03.PL.Helpers
{
    public static class EmailSettings
    {
        //Mail Server exists in any server allover the world 

        public static void SendEmail(Email email)
        {
            SmtpClient? client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("ebrahimsarhan09@gmail.com", "tykpwxmylieuprco");
            //Mail Messages ======= IIMMPPOORRTTAANNTT Important Important Important
            client.Send("ebrahimsarhan09@gmail.com", email.To, email.Subject, email.Body);
        }

    }
}
