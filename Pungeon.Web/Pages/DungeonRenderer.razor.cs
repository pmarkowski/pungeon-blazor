using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Pungeon.Web.Dungeons;
using Pungeon.Web.Dungeons.AStar;

namespace Pungeon.Web.Pages
{
    public class DungeonRendererBase : ComponentBase
    {
        protected string ErrorMessage = string.Empty;

        protected Dungeon Dungeon;

        protected string currentTool = "new-room";

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
                                    Size = new Size(7, 9),
                                    Connectors = new List<Connector>()
                                    {
                                        new Connector()
                                        {
                                            Id = "Connector1",
                                            RelativePosition = new RelativePosition(7, 3)
                                        }
                                    }
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
                                    Size = new Size(4, 9),
                                    Connectors = new List<Connector>()
                                    {
                                        new Connector()
                                        {
                                            Id = "Connector2",
                                            RelativePosition = new RelativePosition(2, 9)
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Connections = new List<Connection>()
                {
                    new Connection()
                    {
                        ConnectorId1 = "Connector1",
                        ConnectorId2 = "Connector2"
                    }
                }
            };

            try
            {
                ErrorMessage = string.Empty;
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
                ErrorMessage = string.Empty;
            }
            catch (Exception e)
            {
                ErrorMessage = e.ToString();
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
                    System.Math.Min(dragStartX.Value, endX),
                    System.Math.Min(dragStartY.Value, endY)
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
                            System.Math.Abs(endX - dragStartX.Value) + 1,
                            System.Math.Abs(endY - dragStartY.Value) + 1)
                    }
                }
                    }
                });

                dragStartX = null;
                dragStartY = null;
            }
            else if (currentTool == "new-connector")
            {
                // Get room/space for this location
                // We have dungeon space coordinates
                int dungeonSpaceX = x;
                int dungeonSpaceY = y;

                // Need to find the closest Space to those coordinates
                // Get manhattan distance from all corners of all spaces and take the min
                Space space = GetClosestSpaceToPoint(dungeonSpaceX, dungeonSpaceY);

                // how do you get the relative room position with this way?
                // and convert the connector to that Space space
                // If you get a reference to a space... you can actually use equality here
                // to find the dungeon space offset by reference
                RelativePosition offset = GetDungeonSpaceOffsetForSpace(space);

                // add a new connector to it with a relative position
                space.Connectors.Add(new Connector()
                {
                    Id = Guid.NewGuid().ToString(),
                    RelativePosition = new RelativePosition(
                        dungeonSpaceX - offset.X,
                        dungeonSpaceY - offset.Y
                    )
                });
            }
        }

        private Space GetClosestSpaceToPoint(int dungeonSpaceX, int dungeonSpaceY)
        {
            RelativePosition pointPosition = new RelativePosition(dungeonSpaceX, dungeonSpaceY);
            var spaceList = Dungeon.Rooms.SelectMany(room =>
                room.Room.Spaces.Select(space =>
                    new
                    {
                        Space = space,
                        SpaceOffset = new RelativePosition(
                            room.RelativePosition.X + space.RelativePosition.X,
                            room.RelativePosition.Y + space.RelativePosition.Y
                        )
                    }));
            Space minSpace = null;
            int minDistance = int.MaxValue;
            foreach (var space in spaceList)
            {
                int minX, minY, maxX, maxY;
                minX = space.SpaceOffset.X;
                minY = space.SpaceOffset.Y;
                maxX = minX + space.Space.Size.Width;
                maxY = minY + space.Space.Size.Height;
                int localMinDistance = int.MaxValue;

                localMinDistance = Math.Min(localMinDistance, DistanceCalculator.GetManhattanDistance(pointPosition, new RelativePosition(minX, minY)));
                localMinDistance = Math.Min(localMinDistance, DistanceCalculator.GetManhattanDistance(pointPosition, new RelativePosition(minX, maxY)));
                localMinDistance = Math.Min(localMinDistance, DistanceCalculator.GetManhattanDistance(pointPosition, new RelativePosition(maxX, minY)));
                localMinDistance = Math.Min(localMinDistance, DistanceCalculator.GetManhattanDistance(pointPosition, new RelativePosition(maxX, maxY)));

                if (localMinDistance < minDistance)
                {
                    minDistance = localMinDistance;
                    minSpace = space.Space;
                }
            }

            return minSpace;
        }

        private RelativePosition GetDungeonSpaceOffsetForSpace(Space space)
        {
            var spaceWithOffset = Dungeon.Rooms.SelectMany(room =>
                room.Room.Spaces.Select(space =>
                    new
                    {
                        Space = space,
                        SpaceOffset = new RelativePosition(
                            room.RelativePosition.X + space.RelativePosition.X,
                            room.RelativePosition.Y + space.RelativePosition.Y
                        )
                    }))
            .Single(spaceWithOffset => spaceWithOffset.Space == space);

            return spaceWithOffset.SpaceOffset;
        }
    }
}