using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Hub.Application.Common.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message) : base(message) { }
    }
}
