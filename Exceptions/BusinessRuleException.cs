using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
        public BusinessRuleException(string entity, string rule)
            : base($"Business rule violation for {entity}: {rule}") { }

        public BusinessRuleException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}