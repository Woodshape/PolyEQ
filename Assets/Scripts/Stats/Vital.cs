namespace PolyEQ.Stats
{
    public class Vital : ModifiedStat
    {
        public float currentValue;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        public Vital()
        {
            currentValue = 0.0f;
            ExpToLevel = 50;
            LevelModifier = 1.1f;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public float CurrentValue
        {
            get
            {
                if (currentValue > AdjustedBaseValue)
                {
                    currentValue = AdjustedBaseValue;
                }

                return currentValue;
            }

            set
            {
                currentValue = value;
                //GameManager.Instance.UIManager.UpdateAllBars();
            }
        }
    }
}