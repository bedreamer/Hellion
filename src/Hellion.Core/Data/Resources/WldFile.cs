using System;
using System.IO;

namespace Hellion.Core.Data.Resources
{
    /// <summary>
    /// FlyFF World File structure.
    /// </summary>
    public class WldFile
    {
        private byte[] data;

        /// <summary>
        /// Gets the world file width value.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the world file length value.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the MPU (meters per unit) value.
        /// </summary>
        public int MPU { get; private set; }

        /// <summary>
        /// Gets a value that indicates if the world is indoor or not.
        /// </summary>
        public bool Indoor { get; private set; }

        /// <summary>
        /// Gets a value that indicates if we can fly in the world.
        /// </summary>
        public bool Fly { get; private set; }

        /// <summary>
        /// Gets the revival map id.
        /// </summary>
        public int RevivalMapId { get; private set; }

        /// <summary>
        /// Gets the revival key.
        /// </summary>
        public string RevivalKey { get; private set; }
        
        /// <summary>
        /// Creates a new WldFile instance.
        /// </summary>
        /// <param name="fileData">Wld file data</param>
        public WldFile(byte[] fileData)
        {
            this.data = fileData;
        }

        /// <summary>
        /// Reads the content of the Wld file.
        /// </summary>
        public void Read()
        {
            using (var stream = new MemoryStream(this.data))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim().ToLower();

                    if (line.StartsWith("//") || string.IsNullOrEmpty(line))
                        continue;

                    string[] lineArray = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    switch (lineArray[0])
                    {
                        case "size": this.ReadSize(lineArray); break;
                        case "indoor":
                            this.Indoor = lineArray[1] == "1" ? true : false;
                            break;
                        case "fly":
                            this.Fly = lineArray[1] == "1" ? true : false;
                            break;
                        case "mpu":
                            this.MPU = int.Parse(lineArray[1]);
                            break;
                        case "revival":
                            this.RevivalMapId = int.Parse(lineArray[1]);
                            this.RevivalKey = lineArray[2].Trim('"');
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Read the "size" field of the wld file.
        /// </summary>
        /// <param name="lineArray">Current line array</param>
        private void ReadSize(string[] lineArray)
        {
            string width = lineArray[1].Replace(",", string.Empty);
            string length = lineArray[2];

            this.Width = int.Parse(width);
            this.Length = int.Parse(length);
        }
    }
}
