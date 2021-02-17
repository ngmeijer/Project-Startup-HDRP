using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer.Scripts.Items
{
    public class ItemsHolder : MonoBehaviour, IHasInventory
    {
        [SerializeField]
        private List<PickableItem> _items;
        
        public IEnumerable<PickableItem> Items => _items;

        public void AddItem(PickableItem pItem)
        {
            _items.Add(pItem);
        }

        public bool ContainsItem(PickableItem pItem)
        {
            return _items.Contains(pItem);
        }
    }
}