public interface IGiveItem
    {
        Item item { get; }
        int localCount { get; }
        void StackReturn(int amount);
    }