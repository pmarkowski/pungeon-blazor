using System;
using System.Collections.Generic;

namespace Pungeon.Web.Dungeons
{
    public class Dungeon
    {
        public List<DungeonRoom> Rooms { get; set; }

        internal int GetHeight()
        {
            throw new NotImplementedException();
        }

        internal int GetWidth()
        {
            throw new NotImplementedException();
        }
    }
}