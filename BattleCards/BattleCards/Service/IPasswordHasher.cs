using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Service
{
    public interface IPasswordHasher
    {
        string HashedPassword(string password);
    }
}
