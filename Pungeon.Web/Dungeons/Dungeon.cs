using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Dungeon
    {
        public List<DungeonRoom> Rooms { get; set; }

        public Dungeon()
        {
            Rooms = new List<DungeonRoom>();
        }

        public int GetWidth()
        {
            return Rooms.Any() ? Rooms.Max(room => room.RelativePosition.X + room.Room.GetWidth()) : 0;
        }

        public int GetHeight()
        {
            return Rooms.Any() ? Rooms.Max(room => room.RelativePosition.Y + room.Room.GetHeight()) : 0;
        }
    }
}