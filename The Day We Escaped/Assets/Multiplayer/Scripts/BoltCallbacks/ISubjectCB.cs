public interface ISubjectCB
{
    int State { get; set; }

    void AttachPlayerNetwork(IObserverCB pObserver);
    void DetachPlayerNetwork(IObserverCB pObserver);
    void UpdateNPCObservers();
}