using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Pungeon.Web.Dungeons;
using Pungeon.Web.ViewUtilities;

namespace Pungeon.Web.Pages
{
    public class DungeonRendererBase : ComponentBase
    {
        protected string ErrorMessage = string.Empty;

        protected Dungeon Dungeon;
        protected Grid Grid;
        protected string DungeonJson;

        protected ToolType currentTool;

        protected Guid? SelectedElementId;

        protected int currentHoverX;
        protected int currentHoverY;
        protected int? dragStartX;
        protected int? dragStartY;

        protected override void OnInitialized()
        {
            Dungeon = new Dungeon()
            {
                Spaces = new List<Space>()
                {
                    new Space()
                    {
                        Position = new Position(1, 1),
                        Size = new Size(5, 5)
                    },
                    new Space()
                    {
                        Position = new Position(6, 4),
                        Size = new Size(7, 9)
                    },
                    new Space()
                    {
                        Position = new Position(15, 15),
                        Size = new Size(6, 7)
                    },
                    new Space()
                    {
                        Position = new Position(21, 19),
                        Size = new Size(4, 9)
                    }
                },
                Walls = new List<WallSegment>()
                {
                    new WallSegment()
                    {
                        Start = new Position(1, 1),
                        End = new Position(1, 6)
                    },
                    new WallSegment()
                    {
                        Start = new Position(6, 6),
                        End = new Position(1, 6)
                    },
                    new WallSegment()
                    {
                        Start = new Position(6, 13),
                        End = new Position(6, 6)
                    },
                    new WallSegment()
                    {
                        Start = new Position(13, 13),
                        End = new Position(6, 13)
                    },
                    new WallSegment()
                    {
                        Start = new Position(13, 4),
                        End = new Position(13, 13)
                    },
                    new WallSegment()
                    {
                        Start = new Position(6, 4),
                        End = new Position(13, 4)
                    },
                    new WallSegment()
                    {
                        Start = new Position(6, 1),
                        End = new Position(6, 4)
                    },
                    new WallSegment()
                    {
                        Start = new Position(1, 1),
                        End = new Position(6, 1)
                    },
                    new WallSegment()
                    {
                        Start = new Position(15, 15),
                        End = new Position(15, 22)
                    },
                    new WallSegment()
                    {
                        Start = new Position(21, 22),
                        End = new Position(15, 22)
                    },
                    new WallSegment()
                    {
                        Start = new Position(21, 15),
                        End = new Position(15, 15)
                    },
                    new WallSegment()
                    {
                        Start = new Position(21, 19),
                        End = new Position(21, 15)
                    },
                    new WallSegment()
                    {
                        Start = new Position(25, 19),
                        End = new Position(21, 19)
                    },
                    new WallSegment()
                    {
                        Start = new Position(21, 22),
                        End = new Position(21, 28)
                    },
                    new WallSegment()
                    {
                        Start = new Position(21, 28),
                        End = new Position(25, 28)
                    },
                    new WallSegment()
                    {
                        Start = new Position(25, 19),
                        End = new Position(25, 28)
                    }
                }
            };
            try
            {
                ErrorMessage = string.Empty;
                UpdateDungeon();
            }
            catch (Exception e)
            {
                ErrorMessage = e.ToString();
            }
        }

        protected void UpdateDungeon()
        {
            Grid = new Grid(Dungeon);
            DungeonJson = JsonSerializer.Serialize(
                Dungeon,
                new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
        }

        protected void RenderDungeon(ChangeEventArgs args)
        {
            try
            {
                string dungeonJson = args.Value.ToString();
                Dungeon = JsonSerializer.Deserialize<Dungeon>(dungeonJson);
                UpdateDungeon();
                ErrorMessage = string.Empty;
            }
            catch (Exception e)
            {
                ErrorMessage = e.ToString();
            }
        }

        protected void ChangeTool(ToolType newTool)
        {
            if (newTool != currentTool)
            {
                SelectedElementId = null;
                currentTool = newTool;
            }
        }

        protected void KeyDown(KeyboardEventArgs e)
        {
            if (currentTool == ToolType.Selector && SelectedElementId.HasValue)
            {
                Space selectedSpace = Dungeon.GetSpace(SelectedElementId.Value);
                Position newPosition;
                switch (e.Key)
                {
                    case "ArrowLeft":
                        newPosition = new Position(
                            selectedSpace.Position.X - 1,
                            selectedSpace.Position.Y);
                        Dungeon.SetSpacePosition(selectedSpace.Id, newPosition);
                        break;
                    case "ArrowRight":
                        newPosition = new Position(
                            selectedSpace.Position.X + 1,
                            selectedSpace.Position.Y);
                        Dungeon.SetSpacePosition(selectedSpace.Id, newPosition);
                        break;
                    case "ArrowUp":
                        newPosition = new Position(
                            selectedSpace.Position.X,
                            selectedSpace.Position.Y - 1);
                        Dungeon.SetSpacePosition(selectedSpace.Id, newPosition);
                        break;
                    case "ArrowDown":
                        newPosition = new Position(
                            selectedSpace.Position.X,
                            selectedSpace.Position.Y + 1);
                        Dungeon.SetSpacePosition(selectedSpace.Id, newPosition);
                        break;
                    case "Delete":
                        Dungeon.RemoveSpace(selectedSpace.Id);
                        SelectedElementId = null;
                        break;
                    default:
                        break;
                }
                UpdateDungeon();
            }
        }

        protected void MouseOver(MouseEventArgs e, int x, int y)
        {
            if (currentTool == ToolType.NewSpace || currentTool == ToolType.NewWall)
            {
                currentHoverX = x;
                currentHoverY = y;
            }
        }

        protected void MouseDown(MouseEventArgs e, int x, int y)
        {
            if (currentTool == ToolType.NewSpace || currentTool == ToolType.NewWall)
            {
                dragStartX = x;
                dragStartY = y;
            }
        }

        protected void MouseUp(MouseEventArgs e, int x, int y)
        {
            if (currentTool == ToolType.NewSpace)
            {
                if (!dragStartX.HasValue || !dragStartY.HasValue)
                {
                    return;
                }

                int endX = x;
                int endY = y;

                Position topLeft = new Position(
                    Math.Min(dragStartX.Value, endX),
                    Math.Min(dragStartY.Value, endY)
                );
                Space spaceToAdd = new Space()
                {
                    Position = topLeft,
                    Size = new Size(
                            Math.Abs(endX - dragStartX.Value) + 1,
                            Math.Abs(endY - dragStartY.Value) + 1)
                };
                Dungeon.Spaces.Add(spaceToAdd);

                // Down
                Dungeon.Walls.Add(new WallSegment()
                {
                    Start = spaceToAdd.Position,
                    End = new Position(spaceToAdd.Position.X, spaceToAdd.Position.Y + spaceToAdd.Size.Height)
                });
                // right
                Dungeon.Walls.Add(new WallSegment()
                {
                    Start = new Position(spaceToAdd.Position.X, spaceToAdd.Position.Y + spaceToAdd.Size.Height),
                    End = new Position(spaceToAdd.Position.X + spaceToAdd.Size.Width, spaceToAdd.Position.Y + spaceToAdd.Size.Height)
                });
                // up
                Dungeon.Walls.Add(new WallSegment()
                {
                    Start = new Position(spaceToAdd.Position.X + spaceToAdd.Size.Width, spaceToAdd.Position.Y + spaceToAdd.Size.Height),
                    End = new Position(spaceToAdd.Position.X + spaceToAdd.Size.Width, spaceToAdd.Position.Y)
                });
                // left
                Dungeon.Walls.Add(new WallSegment()
                {
                    Start = new Position(spaceToAdd.Position.X + spaceToAdd.Size.Width, spaceToAdd.Position.Y),
                    End = spaceToAdd.Position
                });

                UpdateDungeon();

                dragStartX = null;
                dragStartY = null;
            }
            else if (currentTool == ToolType.NewWall)
            {
                if (!dragStartX.HasValue || !dragStartY.HasValue)
                {
                    return;
                }

                int endX = x;
                int endY = y;

                Position start = new Position(
                    dragStartX.Value,
                    dragStartY.Value
                );
                Position end = new Position(
                    endX,
                    endY
                 );
                Dungeon.Walls.Add(new WallSegment
                {
                    Start = start,
                    End = end
                });
                UpdateDungeon();

                dragStartX = null;
                dragStartY = null;
            }
            else if (currentTool == ToolType.Selector)
            {
                SelectedElementId = Grid[x, y].ParentSpaceId;
            }
        }
    }
}
