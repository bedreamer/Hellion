using Hellion.Core.Data.Headers;
using Hellion.World.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.World.Systems.Classes
{
    public abstract class AClass
    {
        /// <summary>
        /// Gets the id of the class.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the class job data.
        /// </summary>
        public JobData Data
        {
            get { return WorldServer.JobData[this.Id]; }
        }
        
        public AClass(int classId)
        {
            this.Id = classId;
        }

        public static AClass Create(int id)
        {
            switch (id)
            {
                case DefineJob.JOB_VAGRANT: return new Vagrant();
                case DefineJob.JOB_MERCENARY: return new Mercenary();
            }
            return null;
        }
    }
}
