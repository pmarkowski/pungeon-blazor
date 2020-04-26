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

        protected string currentTool = "new-space";

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
                }
            };

            try
            {
                ErrorMessage = string.Empty;
                Grid = new Grid(Dungeon);
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
                Grid = new Grid(Dungeon);
                ErrorMessage = string.Empty;
            }
            catch (Exception e)
            {
                ErrorMessage = e.ToString();
            }
        }

        protected void ChangeTool(string newTool)
        {
            SelectedElementId = null;
            currentTool = newTool;
        }

        protected void KeyDown(KeyboardEventArgs e)
        {
            if (currentTool == "selector" && SelectedElementId.HasValue)
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
                        break;
                    default:
                        break;
                }
                Grid = new Grid(Dungeon);
            }
        }

        protected void MouseOver(MouseEventArgs e, int x, int y)
        {
            if (currentTool == "new-space")
            {
                currentHoverX = x;
                currentHoverY = y;
            }
        }

        protected void MouseDown(MouseEventArgs e, int x, int y)
        {
            if (currentTool == "new-space")
            {
                dragStartX = x;
                dragStartY = y;
            }
        }

        protected void MouseUp(MouseEventArgs e, int x, int y)
        {
            if (currentTool == "new-space")
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
                Grid = new Grid(Dungeon);

                dragStartX = null;
                dragStartY = null;
            }
            else if (currentTool == "selector")
            {
                SelectedElementId = Grid[x, y].ParentSpaceId;
            }
        }
    }
}