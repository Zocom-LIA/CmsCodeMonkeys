using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMonkeys.CMS.Public.Shared.Entities
{
    public interface IEntity
    {
        object GetIdentifier();
    }
}