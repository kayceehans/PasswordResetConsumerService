using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateOtpAndSendMailSubscriber.Model
{
    public class OtpsModel
    {
        public int Id { get; set; }        
        public string Value { get; set; }
        public string Email { get; set; }
        public string Intent { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsValidated { get; set; }
    }
}
