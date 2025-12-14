using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartStockAI.models;

namespace SmartStockAI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}