namespace Fontinixxl.NaptimeGames.UI
{
    public interface ISelectable
    {
        void SetSelected(bool isSelected);
        int Value { get; }
    }
}