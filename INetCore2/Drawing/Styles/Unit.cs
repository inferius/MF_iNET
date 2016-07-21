namespace INetCore2.Drawing.Styles
{
    public class Unit
    {
        public static int DPI = 96;
        const float mm_px = 0.26455026455026455026455026455026f;//3.78f;
        const float cm_px = 0.026455026455026455026455026455026f; //37.8f;
        const float in_px = 0.01041666666666666666666666666667f;//96;
        const float pc_px = 0.0625f;//16;
        static float pt_px = DPI / 72;
        public UnitType Value { get; set; }

        public Unit(UnitType ut)
        {
            Value = ut;
        }
        public static Unit Parse(string input)
        {
            string i = input.ToLower().Trim();
            if (i == "px") return UnitType.Pixels;
            if (i == "pt") return UnitType.Poshort;
            if (i == "%") return UnitType.Percentage;
            if (i == "ex") return UnitType.Ex;
            if (i == "em") return UnitType.Em;
            if (i == "pc") return UnitType.Pica;
            if (i == "mm") return UnitType.Milimeter;
            if (i == "cm") return UnitType.Centimeter;
            if (i == "rem") return UnitType.RootEm;


            return UnitType.Pixels;
        }

        public static string GetUnitTypeString(UnitType unit)
        {
            switch (unit)
            {
                case UnitType.Centimeter: return "cm";
                case UnitType.Em: return "em";
                case UnitType.Ex: return "ex";
                case UnitType.Inch: return "in";
                case UnitType.Milimeter: return "mm";
                case UnitType.Percentage: return "%";
                case UnitType.Pica: return "pc";
                case UnitType.Pixels: return "px";
                case UnitType.Poshort: return "pt";
                case UnitType.RootEm: return "rem";
                default: return "px";
            }
        }

        public float ConvertUnit(float input, UnitType from, UnitType to = UnitType.Pixels)
        {
            float ret = 0;

            if (from == UnitType.Milimeter) ret = input / mm_px;
            else if (from == UnitType.Centimeter) ret = input / cm_px;
            else if (from == UnitType.Pica) ret = input / pc_px;
            else if (from == UnitType.Inch) ret = input / in_px;
            else if (from == UnitType.Poshort) ret = input * pt_px;
            else if (from == UnitType.Pixels) ret = input;

            if (to == UnitType.Pixels) return ret;
            if (from == UnitType.Milimeter) ret = input * mm_px;
            else if (from == UnitType.Centimeter) ret = input * cm_px;
            else if (from == UnitType.Pica) ret = input * pc_px;
            else if (from == UnitType.Inch) ret = input * in_px;
            else if (from == UnitType.Poshort) ret = input / pt_px;

            return ret;
        }

        public static implicit operator Unit(UnitType u)
        {
            return new Unit(u);
        }

        public override string ToString()
        {
            return GetUnitTypeString(Value);
        }
    }

    public enum UnitType
    {
        Percentage, // %
        Inch, // in
        Centimeter, // cm
        Milimeter, // mm
        Em,
        Ex,
        Poshort, // pt
        Pica, // pc
        Pixels, // px
        RootEm // rem
    }
}
