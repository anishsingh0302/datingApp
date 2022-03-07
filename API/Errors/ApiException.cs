using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string Messages = null,string Details = null)
        {
            this.statusCode = statusCode;
            this.Messages = Messages;
            this.Details = Details;

        }
        public int statusCode { get; set; }
        public string Messages { get; set; }
        public string Details { get; set; }
    }
}