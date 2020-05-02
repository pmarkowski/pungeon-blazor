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
                        Start = new Position(3, 3),
                        End = new Position(3, 5)
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
            SelectedElementId = null;
            currentTool = newTool;
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
                Dungeon.Spaces.Add(
                    new Space()
                    {
                        Position = topLeft,
                        Size = new Size(
                            Math.Abs(endX - dragStartX.Value) + 1,
                            Math.Abs(endY - dragStartY.Value) + 1)
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