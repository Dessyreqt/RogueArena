namespace RogueArena.Components
{
    public class StairsComponent : Component
    {
        public StairsComponent()
        {
            // here for deserialization purposes
        }

        public StairsComponent(int toFloor)
        {
            ToFloor = toFloor;
        }

        public int ToFloor { get; set; }
    }
}
