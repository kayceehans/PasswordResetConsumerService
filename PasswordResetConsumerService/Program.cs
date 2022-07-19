using Newtonsoft.Json;
using PasswordResetConsumerService.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace PasswordResetConsumerService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            string queueName = "PasswordResetQueue";
            var rabbitMqConnection = factory.CreateConnection();
            var rabbitMqChannel = rabbitMqConnection.CreateModel();

            rabbitMqChannel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            rabbitMqChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            int messageCount = Convert.ToInt16(rabbitMqChannel.MessageCount(queueName));
            Console.WriteLine(" Listening to the queue. This channels has {0} messages on the queue", messageCount);

            var consumer = new EventingBasicConsumer(rabbitMqChannel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine(" Message received: " + message);
                var messageJson = JsonConvert.DeserializeObject<EmailModel>(message);
                SendMail(messageJson);
                Console.WriteLine(" Mail sent:");
                rabbitMqChannel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                Thread.Sleep(1000);
            };
            rabbitMqChannel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            Thread.Sleep(1000 * messageCount);
            Console.WriteLine(" Connection closed, no more messages.");

            Console.ReadLine();
            Console.WriteLine("About to send password reset Email World!");


            Console.WriteLine("password reset Email sent!!");
        }
        public static void SendMail(EmailModel request)
        {
            try
            {
                MailMessage mm = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                mm.From = new MailAddress("hassancodesbetter@gmail.com", "Numero Services", System.Text.Encoding.UTF8);
                mm.To.Add(new MailAddress(request.EmailTo));
                mm.Subject = request.Subject;
                mm.Body = request.Body;

                mm.IsBodyHtml = true;
                smtp.Host = "smtp.gmail.com";
                //if (ccAdd != "")
                //{
                //    mm.CC.Add(ccAdd);
                //}
                smtp.EnableSsl = true;
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = "kayceehans@gmail.com";//gmail user name
                NetworkCred.Password = "fkinjnzgkrqluykp";// password
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587; //Gmail port for e-mail 465 or 587
                smtp.Send(mm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.ToString()}");
            }

        }
    }
}
