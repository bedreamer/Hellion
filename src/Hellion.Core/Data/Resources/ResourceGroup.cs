using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hellion.Data
{
    public class ResourceGroup
    {
        private string file;
        private Dictionary<string, int> defines;
        private Dictionary<string, string> texts;

        /// <summary>
        /// Gets the parsed groups from the file passed during the construction.
        /// </summary>
        public List<Group> Groups { get; private set; }
        
        
        /// <summary>
        /// Creates a new ResourceGroup instance.
        /// </summary>
        /// <param name="file"></param>
        public ResourceGroup(String file)
        {
            this.file = file;
            this.defines = new Dictionary<string, int>();
            this.texts = new Dictionary<string, string>();
            this.Groups = new List<Group>();
        }
        
        /// <summary>
        /// Parse the groups
        /// </summary>
        public void Parse()
        {
            var linesData = new List<string>();
            string[] lines = null;

            using (var fileStream = new FileStream(this.file, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
                lines = reader.ReadToEnd().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            #region Quote lines

            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = lines[i].Trim();
                if (lines[i].StartsWith("//") == true)
                {
                    lines[i] = null;
                    continue;
                }
                if (lines[i].StartsWith("/*") == true)
                {
                    int startLine = i;
                    int endLine = 0;
                    while (lines[i].Contains("*/") == false)
                        ++i;
                    endLine = i;
                    for (int j = startLine; j <= endLine; ++j) // remove commented block
                        lines[j] = null;
                    continue;
                }
                if (lines[i].Contains("//") == true)
                    lines[i] = lines[i].Remove(lines[i].IndexOf("/"));
            }

            for (int i = 0; i < lines.Length; ++i)
                if (!string.IsNullOrEmpty(lines[i]))
                    linesData.Add(lines[i]);

            #endregion

            try
            {
                for (int i = 0; i < linesData.Count; ++i)
                {
                    if (linesData[i + 1] != null && linesData[i + 1].StartsWith("{") == true)
                    {
                        int block = 0;
                        var group = new Group(linesData[i].Trim());
                        ++i;

                        while (true)
                        {
                            if (linesData[i].StartsWith("{") == true)
                                ++block;
                            else if (linesData[i].StartsWith("}") == true)
                            {
                                --block;
                                if (block == 0)
                                    break;
                            }
                            group.Content.Add(linesData[i]);
                            ++i;
                        }
                        group.Parse();
                        this.Replace(group);
                        this.Groups.Add(group);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Add defines to the resource group
        /// </summary>
        /// <param name="file"></param>
        public void AddDefine(Dictionary<string, int> defines)
        {
            this.defines = defines;
        }

        /// <summary>
        /// Add text to the resource group
        /// </summary>
        /// <param name="file"></param>
        public void AddText(String file)
        {
        }

        /// <summary>
        /// Replace all key words by defines or texts
        /// </summary>
        /// <param name="group"></param>
        private void Replace(Group group)
        {
            for (int i = 0; i < group.Content.Count; ++i)
            {
                if (this.defines.ContainsKey(group.Content[i]))
                    group.Content[i] = this.defines[group.Content[i]].ToString();
                else if (this.texts.ContainsKey(group.Content[i]))
                    group.Content[i] = this.texts[group.Content[i]].ToString();
            }

            if (this.defines.ContainsKey(group.Name))
                group.Name = this.defines[group.Name].ToString();
            else if (this.texts.ContainsKey(group.Name))
                group.Name = this.texts[group.Name].ToString();

            foreach (var kvalue in this.defines)
            {
                for (int j = 0; j < group.Content.Count; ++j)
                {
                    if (group.Content[j].Contains(kvalue.Key))
                        group.Content[j] = group.Content[j].Replace(kvalue.Key, kvalue.Value.ToString());
                }
            }
            foreach (Group groupChild in group.Groups)
                this.Replace(groupChild);
        }
    }

    public class Group
    {
        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the group collection inside this group.
        /// </summary>
        public IList<Group> Groups { get; set; }

        /// <summary>
        /// Gets or sets the group content collection.
        /// </summary>
        public IList<string> Content { get; set; }

        /// <summary>
        /// Creates a new Group instance.
        /// </summary>
        /// <param name="name"></param>
        public Group(string name)
        {
            this.Name = name;
            this.Groups = new List<Group>();
            this.Content = new List<string>();
        }

        /// <summary>
        /// Destroy the group.
        /// </summary>
        ~Group()
        {
            this.Groups.Clear();
            this.Content.Clear();
        }

        /// <summary>
        /// Add string to the group.
        /// </summary>
        /// <param name="obj"></param>
        public void Add(string obj)
        {
            this.Content.Add(obj);
        }

        /// <summary>
        /// Parse a group.
        /// </summary>
        internal void Parse()
        {
            for (int i = 1; i < this.Content.Count - 1; ++i)
            {
                if (this.Content[i + 1] != null && this.Content[i + 1].StartsWith("{") == true)
                {
                    int block = 0;
                    var group = new Group(this.Content[i]);
                    ++i;

                    while (true)
                    {
                        if (this.Content[i].StartsWith("{"))
                            ++block;
                        else if (this.Content[i].StartsWith("}"))
                        {
                            --block;
                            if (block == 0)
                                break; // we return to main block
                        }
                        group.Content.Add(this.Content[i]);
                        ++i;
                    }

                    group.Parse();
                    this.Groups.Add(group);
                }
                else if (this.Content[i + 1] != null && this.Content[i + 1].StartsWith("("))
                {
                    int block = 0;
                    var group = new Group(this.Content[i]);
                    ++i;

                    while (true)
                    {
                        if (this.Content[i].StartsWith("("))
                        {
                            ++block;
                            ++i;
                        }
                        else if (this.Content[i].StartsWith(")"))
                        {
                            --block;
                            if (block == 0)
                                break; // we return to main block
                        }
                        group.Content.Add(this.Content[i]);
                        ++i;
                    }

                    group.Parse();
                    this.Groups.Add(group);
                }
            }
        }

        /// <summary>
        /// Gets a group by his name.
        /// </summary>
        /// <param name="name">Group name</param>
        /// <returns></returns>
        public Group GetGroupByName(string name)
        {
            return this.Groups.Where(g => g.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Return a content line.
        /// </summary>
        /// <param name="directive"></param>
        /// <returns></returns>
        public string GetContentByBegin(string directive)
        {
            return this.Content.Where(c => c.StartsWith(directive)).FirstOrDefault();
        }

        /// <summary>
        /// Return all groups who start with the directive passed as parameter.
        /// </summary>
        /// <param name="directive"></param>
        /// <returns></returns>
        public IEnumerable<Group> GetGroupsStartWith(String directive)
        {
            return this.Groups.Where(g => g.Name.StartsWith(directive));
        }
        
        /// <summary>
        /// Display the group name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
