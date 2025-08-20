using System.ComponentModel;

namespace Engine
{
    public class InventoryItem : INotifyPropertyChanged
    {
        private Item _details;
        public Item Details
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChange("Details");
            }
        }
        private int _quantity;
        public int Quantity 
        { get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChange("Quantity");
            }
        }
        public string Description
        {
            get { return Quantity > 1 ? Details.NamePlural : Details.Name; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public InventoryItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }


    }
}
