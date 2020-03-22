using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Dungeon
    {
        public List<DungeonRoom> Rooms { get; set; }

        public int GetWidth()
        {
            return Rooms.Max(room => room.RelativePosition.X + room.Room.GetWidth());
        }

        public int GetHeight()
        {
            return Rooms.Max(room => room.RelativePosition.Y + room.Room.GetHeight());
        }
    }
}