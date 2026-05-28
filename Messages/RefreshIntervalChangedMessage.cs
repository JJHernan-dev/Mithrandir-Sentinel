namespace Mithrandir_Sentinel.Messages
{
    public class RefreshIntervalChangedMessage
    {
        public int Value { get; }

        public RefreshIntervalChangedMessage(int value)
        {
            Value = value;
        }
    }
}
