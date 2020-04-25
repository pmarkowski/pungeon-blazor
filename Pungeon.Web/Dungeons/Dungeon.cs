using System;
using System.Collections.Generic;
using System.Linq;

namespace Pungeon.Web.Dungeons
{
    public class Dungeon
    {
        public Size Size { get; set; }
        public List<DungeonRoom> Rooms { get; set; }

        public Dungeon()
        {
            Size = new Size(0, 0);
            Rooms = new List<DungeonRoom>();
        }

        public void RemoveRoom(Guid roomId)
        {
            int removalIndex = Rooms.FindIndex(room => room.Room.Id == roomId);
            Rooms.RemoveAt(removalIndex);
        }

        public void SetRoomPosition(Guid roomId, RelativePosition newPosition)
        {
            Rooms.Single(room => room.Room.Id == roomId).RelativePosition = newPosition;
        }
    }
}