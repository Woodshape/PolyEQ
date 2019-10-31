using System.Collections.Generic;

namespace PolyEQ.Stats
{
    public class ModifiedStat : BaseStat
    {
        private List<ModifyingAttribute> mods;
        private List<ModifyingValue> values;
        private float modValue;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        public ModifiedStat()
        {
            mods = new List<ModifyingAttribute>();
            values = new List<ModifyingValue>();
            modValue = 0;
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public new float AdjustedBaseValue
        {
            get => (BaseValue + BuffValue + modValue);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void AddModifier(ModifyingAttribute mod)
        {
            mods.Add(mod);

            Update();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void AddModifierValue(ModifyingValue val)
        {
            values.Add(val);

            Update();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void Update()
        {
            CalculateModValues();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        public void CalculateModValues()
        {
            modValue = 0;

            if (mods.Count > 0)
            {
                foreach (ModifyingAttribute att in mods)
                {
                    modValue += (att.ModAttribute.AdjustedBaseValue * att.Ratio);
                }
            }

            if (values.Count > 0)
            {
                foreach (ModifyingValue val in values)
                {
                    modValue += (val.Value * val.Ratio);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////
    public struct ModifyingAttribute
    {
        public Attribute ModAttribute;
        public float Ratio;

        public ModifyingAttribute(Attribute att, float rat = 1.0f)
        {
            this.ModAttribute = att;
            this.Ratio = rat;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////
    public struct ModifyingValue
    {
        public float Value;
        public float Ratio;

        public ModifyingValue(int val, float rat = 1.0f)
        {
            this.Value = val;
            this.Ratio = rat;
        }
    }
}