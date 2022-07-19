using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordResetConsumerService.Model
{
    public class EmailModel
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
