using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Pungeon.Web.Dungeons;

namespace Pungeon.Web.Pages
{
    public class DungeonRendererBase : ComponentBase
    {
        protected string ErrorMessage = string.Empty;

        protected Dungeon Dungeon;
        protected Grid Grid;

        protected string currentTool = "new-room";

        protected DungeonRoom SelectedRoom;

        protected int currentHoverX;
        protected int currentHoverY;
        protected int? dragStartX;
        protected int? dragStartY;

        protected override void OnInitialized()
        {
            Dungeon = new Dungeon()
            {
                Rooms = new List<DungeonRoom>()
                {
                    new DungeonRoom()
                    {
                        RelativePosition = new RelativePosition(1, 1),
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = new RelativePosition(1, 1),
                                    Size = new Size(5, 5)
                                },
                                new Space()
                                {
                                    RelativePosition = new RelativePosition(6, 4),
                                    Size = new Size(7, 9)
                                }
                            }
                        }
                    },
                    new DungeonRoom()
                    {
                        RelativePosition = new RelativePosition(15, 16),
                        Room = new Room()
                        {
                            Spaces = new List<Space>()
                            {
                                new Space()
                                {
                                    RelativePosition = new RelativePosition(0, 0),
                                    Size = new Size(6, 7)
                                },
                                new Space()
                                {
                                    RelativePosition = new RelativePosition(6, 3),
                                    Size = new Size(4, 9)
                                }
                            }
                        }
                    }
                }
            };

            try
            {
                ErrorMessage = string.Empty;
                Grid = Dungeon.ToGrid();
            }
            catch (Exception e)
            {
                ErrorMessage = e.ToString();
            }
        }

        protected void RenderDungeon(ChangeEventArgs args)
        {
            try
            {
                string dungeonJson = args.Value.ToString();
                Dungeon = System.Text.Json.JsonSerializer.Deserialize<Dungeon>(dungeonJson);
                Grid = Dungeon.ToGrid();
                ErrorMessage = string.Empty;
            }
            catch (Exception e)
            {
                ErrorMessage = e.ToString();
            }
        }

        protected void ChangeTool(string newTool)
        {
            SelectedRoom = null;
            currentTool = newTool;
        }

        protected void KeyDown(KeyboardEventArgs e)
        {
            if (currentTool == "selector" && SelectedRoom != null)
            {
                RelativePosition newPosition;
                switch (e.Key)
                {
                    case "ArrowLeft":
                        newPosition = new RelativePosition(
                            SelectedRoom.RelativePosition.X - 1,
                            SelectedRoom.RelativePosition.Y);
                        Dungeon.SetRoomPosition(SelectedRoom.Room.Id, newPosition);
                        break;
                    case "ArrowRight":
                        newPosition = new RelativePosition(
                            SelectedRoom.RelativePosition.X + 1,
                            SelectedRoom.RelativePosition.Y);
                        Dungeon.SetRoomPosition(SelectedRoom.Room.Id, newPosition);
                        break;
                    case "ArrowUp":
                        newPosition = new RelativePosition(
                            SelectedRoom.RelativePosition.X,
                            SelectedRoom.RelativePosition.Y - 1);
                        Dungeon.SetRoomPosition(SelectedRoom.Room.Id, newPosition);
                        break;
                    case "ArrowDown":
                        newPosition = new RelativePosition(
                            SelectedRoom.RelativePosition.X,
                            SelectedRoom.RelativePosition.Y + 1);
                        Dungeon.SetRoomPosition(SelectedRoom.Room.Id, newPosition);
                        break;
                    case "Delete":
                        Dungeon.RemoveRoom(SelectedRoom.Room.Id);
                        break;
                    default:
                        break;
                }
                Grid = Dungeon.ToGrid();
            }
        }

        protected void MouseOver(MouseEventArgs e, int x, int y)
        {
            if (currentTool == "new-room")
            {
                currentHoverX = x;
                currentHoverY = y;
            }
        }

        protected void MouseDown(MouseEventArgs e, int x, int y)
        {
            if (currentTool == "new-room")
            {
                dragStartX = x;
                dragStartY = y;
            }
        }

        protected void MouseUp(MouseEventArgs e, int x, int y)
        {
            if (currentTool == "new-room")
            {
                if (!dragStartX.HasValue || !dragStartY.HasValue)
                {
                    return;
                }

                int endX = x;
                int endY = y;

                RelativePosition topLeft = new RelativePosition(
                    Math.Min(dragStartX.Value, endX),
                    Math.Min(dragStartY.Value, endY)
                );
                Dungeon.Rooms.Add(new DungeonRoom
                {
                    RelativePosition = topLeft,
                    Room = new Room
                    {
                        Spaces = new List<Space>()
                        {
                            new Space()
                            {
                                RelativePosition = new RelativePosition(0, 0),
                                Size = new Size(
                                    Math.Abs(endX - dragStartX.Value) + 1,
                                    Math.Abs(endY - dragStartY.Value) + 1)
                            }
                        }
                    }
                });
                Grid = Dungeon.ToGrid();

                dragStartX = null;
                dragStartY = null;
            }
            else if (currentTool == "selector")
            {
                SelectedRoom = Grid[x, y].ParentRoom;
            }
        }
    }
}