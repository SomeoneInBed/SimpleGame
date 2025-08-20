using System.ComponentModel;

namespace Engine
{
    public class LivingBeing : INotifyPropertyChanged
    {
        private int currentHitPoints;
        public int CurrentHitPoints { 
            get { return currentHitPoints; }
            set 
            {
                currentHitPoints = value;
                OnPropertyChange("CurrentHitPoints");
            }
        }
        public int MaxHitPoints { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChange(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public LivingBeing(int currentHitPoints, int maxHitPoints)
        {
            CurrentHitPoints = currentHitPoints;
            MaxHitPoints = maxHitPoints;
        }

    }
}
