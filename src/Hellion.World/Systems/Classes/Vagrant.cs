using Hellion.Core.Data.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems.Classes
{
    public class Vagrant : AClass
    {
        public Vagrant() 
            : base(DefineJob.JOB_VAGRANT)
        {
        }

        public Vagrant(int parentClassId)
            : base(parentClassId)
        {
        }
    }
}
