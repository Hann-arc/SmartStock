using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string entity, Guid id)
            : base($"{entity} with ID {id} not found") { }

    }
}