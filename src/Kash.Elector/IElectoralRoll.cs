using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector
{
    public interface IElectoralRoll
    {
        Elector GetElector(string credential);
    }
}
