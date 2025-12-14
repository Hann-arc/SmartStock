using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Alert
{
    public class ReqResolveAlertDto
    {
        public Guid AlertId { get; set; }
        public Guid ResolvedByUserId { get; set; }
    }
}