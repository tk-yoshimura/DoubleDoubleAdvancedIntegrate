using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public static class Interval {
        public static (ddouble, ddouble) Unit => (0d, 1d);

        public static (ddouble, ddouble) InfToUnit(ddouble min, ddouble max) 
            => (InfSCurve.Invert(min), InfSCurve.Invert(max));

        public static (ddouble, ddouble) OmniAzimuth => (0d, ddouble.PI * 2);

        public static (ddouble, ddouble) OmniAltura => (0d, ddouble.PI);
    }
}
