using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(T _data, string _message = null)
        {
            succeeded = true;
            message = _message;
            data = _data;
            transactionDateTime = DateTime.UtcNow;
        }

        public Response(string _message, bool _succeeded = true)
        {
            succeeded = _succeeded;
            message = _message;
            transactionDateTime = DateTime.UtcNow;
        }

        public bool succeeded { get; set; }
        public string message { get; set; }
        public DateTime transactionDateTime { get; set; }
        public List<ErrorModel> errors { get; set; }
        public T data { get; set; }
    }

    public class ErrorModel
    {
        public string propertyName { get; set; }
        public string errorMessage { get; set; }
    }
}
