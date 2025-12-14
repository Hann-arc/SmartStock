using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStockAI.Dtos.Account
{
    public class ResLoginDto
    {
        public string Token { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}