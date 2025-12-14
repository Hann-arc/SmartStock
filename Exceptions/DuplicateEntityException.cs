using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string entity, string value)
            : base($"A {entity} with name '{value}' already exists") { }
    }
}