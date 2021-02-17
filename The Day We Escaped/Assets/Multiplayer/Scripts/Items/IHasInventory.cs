using System.Collections.Generic;

public interface IHasInventory
{
    IEnumerable<PickableItem> Items { get; }
    void AddItem(PickableItem pItem);
}