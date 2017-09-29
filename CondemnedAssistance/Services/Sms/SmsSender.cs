using CondemnedAssistance.Models;
using CondemnedAssistance.Services.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CondemnedAssistance.Services.Sms {
    public class SmsSender : IMessageSender {

        private UserContext _db;

        public SmsSender(UserContext context) {
            _db = context;
        }
        
        public string Send(int userId, int senderId, string subject, string message) {

            string phone = _db.Users.Single(u => u.Id == userId).PhoneNumber;
            string login = "probaciya";
            string password = "Qwerty2017";

            Models.Sms sms = new Models.Sms {
                Subject = subject,
                Text = message
            };

            _db.Smss.Add(sms);
            _db.SaveChanges();

            string url = string.Format("http://smsc.kz/sys/send.php?login={0}" +
                "&psw={1}" +
                "&phones={2}" +
                "&mes={3}" +
                "&subj={4}" +
                "&id={5}", login, password, phone, message, subject, sms.Id);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(resp.GetResponseStream());
            string response = reader.ReadToEnd();
            reader.Close();
            resp.Close();

            SmsExchange smsExchange = new SmsExchange {
                ReceiverId = userId,
                IsSuccessfullySent = true,
                ReceiverPhone = phone,
                SentDate = DateTime.Now,
                SenderId = senderId,
                SmsId = sms.Id
            };

            _db.SmsExchanges.Add(smsExchange);
            _db.SaveChanges();

            return response;
        }
    }
}
