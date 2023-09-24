namespace UI
{
    public interface ISelectable
    {
        void SetSelected(bool isSelected);
        int Value { get; }
    }
}