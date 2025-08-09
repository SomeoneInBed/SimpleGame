namespace Engine
{
    public class HealingSpell : Item
    {
        public int AmountToHeal { get; set; }

        public HealingSpell(int id, string name, string namePlural, int amountToHeal) : base(id, name, namePlural)
        {
            AmountToHeal = amountToHeal; 
        }
    }
}
