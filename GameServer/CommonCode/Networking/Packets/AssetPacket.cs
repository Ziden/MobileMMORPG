using Common.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonCode.Networking.Packets
{
    [Serializable]
    public class AssetPacket : BasePacket
    {
        public string ResquestedImageName;
        public bool HaveIt;
        public byte[] Asset;
        public AssetType AssetType;
    }

    public enum AssetType
    {
        TILESET = 1,
        SPRITE = 2,
        ANIMATION = 3,
        ITEMS = 4,
    }
}
