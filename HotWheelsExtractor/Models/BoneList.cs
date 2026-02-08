using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace HotWheels.Models
{
    [DisplayName("BONE_LIST")]
    public class BoneList : ISelfReader, IInlineCount
    {
        [DisplayName("BONENUM")]
        [LinkedCount("BONE_LIST")]
        public int Count { get; set; }

        public List<Bone> Bones { get; set; }

        public void Read(BinaryReader br, int count)
        {
            Bones = [];

            for (int i = 0; i < count; i++)
            {
                string[] s = br.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                Bone bone = new()
                {
                    Name = string.Join(' ', s, 0, Array.IndexOf(s, "*ID")),
                    ID = s.Last()
                };

                Bones.Add(bone);
            }
        }
    }
}
