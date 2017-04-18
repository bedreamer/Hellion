using Hellion.Core.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellion.Core.Data.Resources
{
    public sealed class RgnFile
    {
        private byte[] fileData;

        public ICollection<RgnElement> Elements { get; private set; }


        public RgnFile(byte[] rgnData)
        {
            this.fileData = rgnData;
            this.Elements = new List<RgnElement>();
        }


        public void Read()
        {
            using (var memoryStream = new MemoryStream(this.fileData))
            using (var reader = new StreamReader(memoryStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line) || line.StartsWith(Global.SingleLineComment))
                        continue;

                    string[] data = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (line.StartsWith("respawn7"))
                    {
                        if (data.Length < 24)
                            continue;

                        this.Elements.Add(new RgnRespawn7(data));
                    }

                    if (line.StartsWith("region3"))
                    {
                        if (data.Length < 32)
                            continue;

                        this.Elements.Add(new RgnRegion3(data));
                    }

                    // add more...
                }
            }
        }
    }

    public class RgnElement
    {
        public RgnElement()
        {
        }
    }

    public sealed class RgnRespawn7 : RgnElement
    {
        public int Type { get; private set; }

        public int Model { get; private set; }

        public Vector3 Position { get; private set; }

        public int Count { get; private set; }

        public int Time { get; private set; }

        public int AgroNumber { get; private set; }

        public Vector3 StartPosition { get; private set; }

        public Vector3 EndPosition { get; private set; }

        public RgnRespawn7(string[] respawnData)
            : base()
        {
            this.Type = int.Parse(respawnData[1]);
            this.Model = int.Parse(respawnData[2]);
            this.Position = new Vector3(respawnData[3], respawnData[4], respawnData[5]);
            this.Count = int.Parse(respawnData[6]);
            this.Time = int.Parse(respawnData[7]);
            this.AgroNumber = int.Parse(respawnData[8]);
            this.StartPosition = new Vector3(respawnData[9], "", respawnData[10]);
            this.EndPosition = new Vector3(respawnData[11], "", respawnData[12]);
        }
    }

    public sealed class RgnRegion3 : RgnElement
    {
        public int Type { get; private set; }

        public int Index { get; private set; }

        public Vector3 Position { get; private set; }

        public int Attribute { get; private set; }

        public int MusicId { get; private set; }

        public bool DirectMusic { get; private set; }

        public string Script { get; private set; }

        public string Sound { get; private set; }

        public int TeleportWorldId { get; private set; }

        public Vector3 TeleportPosition { get; private set; }

        public Vector3 TopLeftPosition { get; set; }

        public Vector3 BottomRightPosition { get; set; }

        public string Key { get; private set; }

        public bool TargetKey { get; private set; }

        public int ItemId { get; private set; }

        public int ItemCount { get; private set; }

        public int MinLevel { get; private set; }

        public int MaxLevel { get; private set; }

        public int QuestId { get; private set; }

        public int QuestFlag { get; private set; }

        public int Job { get; private set; }

        public int Gender { get; private set; }

        public bool CheckParty { get; private set; }

        public bool CheckGuild { get; private set; }

        public bool ChaoKey { get; private set; }

        public RgnRegion3(string[] regionData)
        {
            this.Type = int.Parse(regionData[1]);
            this.Index = int.Parse(regionData[2]);
            this.Position = new Vector3(regionData[3], regionData[4], regionData[5]);
            this.Attribute = int.Parse(regionData[6].Replace("0x", string.Empty), System.Globalization.NumberStyles.AllowHexSpecifier);
            this.MusicId = int.Parse(regionData[7]);
            this.DirectMusic = int.Parse(regionData[8]) == 1;
            this.Script = regionData[9];
            this.Sound = regionData[10];
            this.TeleportWorldId = int.Parse(regionData[11]);
            this.TeleportPosition = new Vector3(regionData[12], regionData[13], regionData[14]);
            this.TopLeftPosition = new Vector3(regionData[15], "", regionData[16]);
            this.BottomRightPosition = new Vector3(regionData[17], "", regionData[18]);
            this.Key = regionData[19].Trim('"');
            this.TargetKey = int.Parse(regionData[20]) == 1;
            this.ItemId = int.Parse(regionData[21]);
            this.ItemCount = int.Parse(regionData[22]);
            this.MinLevel = int.Parse(regionData[23]);
            this.MaxLevel = int.Parse(regionData[24]);
            this.QuestId = int.Parse(regionData[25]);
            this.QuestFlag = int.Parse(regionData[26]);
            this.Job = int.Parse(regionData[27]);
            this.Gender = int.Parse(regionData[28]);
            this.CheckParty = int.Parse(regionData[29]) == 1;
            this.CheckGuild = int.Parse(regionData[30]) == 1;
            this.ChaoKey = int.Parse(regionData[31]) == 1;
        }
    }
}
