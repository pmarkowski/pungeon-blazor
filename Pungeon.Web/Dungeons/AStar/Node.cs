namespace Pungeon.Web.Dungeons.AStar
{
    public class Node
    {
        public RelativePosition Position { get; set; }
        public int RunningCost { get; set; }
        public int EstimatedRemainingCost { get; set; }
        public int TotalCost => RunningCost + EstimatedRemainingCost;
        public Node Parent { get; set; }
    }
}