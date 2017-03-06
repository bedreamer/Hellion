using Hellion.Core.Data.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems.Classes
{
    public class Mercenary : Vagrant
    {
        public Mercenary() 
            : base(DefineJob.JOB_MERCENARY)
        {
        }
    }
}
