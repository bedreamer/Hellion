using Hellion.Core.Data.Resources;
using Hellion.Core.Helpers;
using Hellion.Core.IO;
using Hellion.Core.Structures.Dialogs;
using Hellion.World.Structures;
using System.Collections.Generic;
using System.Linq;
using Hellion.Core.Structures;

namespace Hellion.World.Systems
{
    /// <summary>
    /// FlyFF NPC structure.
    /// </summary>
    public sealed partial class Npc : Mover
    {
        private long lastSpeakTime;

        /// <summary>
        /// Gets or sets the NPC data.
        /// </summary>
        public NPCData Data { get; private set; }

        /// <summary>
        /// Gets or sets the npc dialog data.
        /// </summary>
        public DialogData Dialog { get; private set; }

        /// <summary>
        /// Gets the NPC shop items.
        /// </summary>
        /// <remarks>
        /// We know that a shop is composed of a maximum of 4 tabs and can have 100 items max in each tab.
        /// So we this property is an array of a collection with a variable size.
        /// </remarks>
        public ICollection<Item>[] Shop { get; set; }

        /// <summary>
        /// Creates a new NPC instance.
        /// </summary>
        public Npc()
            : base(-1)
        {
        }

        /// <summary>
        /// Creates a new NPC instance with a model id.
        /// </summary>
        /// <param name="modelId"></param>
        public Npc(int modelId)
            : base(modelId)
        {
        }

        /// <summary>
        /// Update the NPC.
        /// </summary>
        public override void Update()
        {
            this.SpeakOralText();

            base.Update();
        }

        /// <summary>
        /// Get the weapon attack damages.
        /// </summary>
        /// <param name="weaponType"></param>
        /// <returns></returns>
        public override int GetWeaponAttackDamages(int weaponType)
        {
            return 0;
        }

        /// <summary>
        /// Process the npc oral dialog.
        /// </summary>
        private void SpeakOralText()
        {
            if (this.lastSpeakTime <= Time.TimeInSeconds())
            {
                if (this.Dialog != null && !string.IsNullOrEmpty(this.Dialog.OralText))
                {
                    var playersAround = this.GetSpawnedObjectsAround(60).Where(x => x is Player).Cast<Player>();

                    foreach (var player in playersAround)
                    {
                        string oralText = this.Dialog.OralText.Replace("%PLAYERNAME%", player.Name);
                        this.SendNormalChatTo(oralText, player);
                    }
                }

                this.lastSpeakTime = Time.TimeInSeconds() + RandomHelper.LongRandom(10, 15);
            }
        }

        /// <summary>
        /// Creates a new NPC from a dyo element.
        /// </summary>
        /// <param name="dyoElement">Map Dyo element</param>
        /// <param name="mapId">Npc map Id</param>
        /// <returns></returns>
        public static Npc CreateFromDyo(NpcDyoElement dyoElement, int mapId)
        {
            var npc = new Npc(dyoElement.Index);

            npc.MapId = mapId;
            npc.Name = dyoElement.Name;
            npc.Angle = dyoElement.Angle;
            npc.Position = dyoElement.Position.Clone();
            npc.DestinationPosition = dyoElement.Position.Clone();
            npc.Size = (short)(npc.Size * dyoElement.Scale.X);

            if (WorldServer.NPCData.ContainsKey(npc.Name))
                npc.Data = WorldServer.NPCData[npc.Name];

            if (WorldServer.DialogsData.ContainsKey(npc.Name))
                npc.Dialog = WorldServer.DialogsData[npc.Name];

            return npc;
        }
    }
}
