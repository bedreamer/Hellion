using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems.Classes
{
    public abstract class AClass
    {
        public int Id { get; private set; }

        public AClass(int classId)
        {
            this.Id = classId;
        }
    }
}
