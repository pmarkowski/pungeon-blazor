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

        public void RenderDungeon(ChangeEventArgs args)
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
            currentHoverX = x;
            currentHoverY = y;
        }

        protected void MouseDown(MouseEventArgs e, int x, int y)
        {
            dragStartX = x;
            dragStartY = y;
        }

        protected void MouseUp(MouseEventArgs e, int x, int y)
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
    }
}