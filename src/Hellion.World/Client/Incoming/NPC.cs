using Hellion.Core.Data.Headers;
using Hellion.Core.Network;
using Hellion.Core.IO;
using Hellion.World.Systems;
using System.Linq;

namespace Hellion.World.Client
{
    public partial class WorldClient
    {
        [FFIncomingPacket(PacketType.SCRIPTDLG)]
        public void OnNPCInteraction(FFPacket packet)
        {
            var objectId = packet.Read<int>();
            var dialogKey = packet.Read<string>();
            var global1 = packet.Read<int>();
            var global2 = packet.Read<int>();
            var srcId = packet.Read<int>();
            var destId = packet.Read<int>();
            var npc = this.Player.GetSpawnedObjectById<Npc>(objectId);

            if (npc != null && npc.Dialog != null)
            {
                if (dialogKey == "BYE")
                {
                    var byeLink = npc.Dialog.Links.Where(x => x.Id == dialogKey).FirstOrDefault();

                    npc.SendNormalChatTo(byeLink?.Text, this.Player);
                    npc.SendCloseDialogTo(this.Player);
                }
                else
                    npc.SendDialogTo(this.Player, dialogKey);
            }
        }

        [FFIncomingPacket(PacketType.CHANGEFACE)]
        public void OnChangeFace(FFPacket packet)
        {
            var objectId = packet.Read<int>();
            var faceId = packet.Read<int>();
            var cost = packet.Read<int>();
            var coupon = packet.Read<bool>();

            if (objectId > 0)
            {
                if (cost >= 0 && this.Player.Gold >= cost)
                {
                    this.Player.FaceId = faceId;
                    this.Player.Gold -= cost;

                    this.Player.SendUpdateDestParam(DefineAttributes.GOLD, this.Player.Gold);
                    this.Player.SendChangeFace(faceId);
                }
                else
                {
                    Log.Error(this.Player.Name + " tried to change face without enough money.");
                }
            }
            else
            {
                Log.Error(this.Player.Name + " tried to change face with fake objectId.");
            }
        }

        [FFIncomingPacket(PacketType.SET_HAIR)]
        public void OnChangeHair(FFPacket packet)
        {
            var hairId = packet.Read<byte>();
            var red = packet.Read<byte>();
            var green = packet.Read<byte>();
            var blue = packet.Read<byte>();
            var coupon = packet.Read<bool>();
            var hairColor = (uint)((((0xff) & 0xff) << 24) | (((red) & 0xff) << 16) | (((green) & 0xff) << 8) | ((blue) & 0xff));
            var cost = 0;

            if (hairId != this.Player.HairId)
                cost += 2000000;
            if (this.Player.HairColor != hairColor)
                cost += 4000000;

            if (cost > 0 && this.Player.Gold >= cost)
            {
                this.Player.HairId = hairId;
                this.Player.Gold -= cost;
                
                this.Player.SendUpdateDestParam(DefineAttributes.GOLD, this.Player.Gold);
                this.Player.SendChangeHair(hairId, red, green, blue);
            }
            else
            {
                Log.Error(this.Player.Name + " tried to change hair without enough money.");
            }
        }
    }
}
