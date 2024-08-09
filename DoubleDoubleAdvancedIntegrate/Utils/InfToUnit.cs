using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class InfSCurve {
        public static ddouble Value(ddouble x) {
            ddouble abs_x = ddouble.Abs(x);

            if (abs_x <= 1d) {
                return x / (1d - ddouble.Abs(x));
            }

            return ddouble.NaN;
        }

        public static ddouble Diff(ddouble x) {
            ddouble abs_x = ddouble.Abs(x);

            if (abs_x <= 1d) {
                return 1d / ddouble.Square(1d - ddouble.Abs(x));
            }

            return ddouble.NaN;
        }

        public static ddouble Invert(ddouble x) {
            return x / (ddouble.Abs(x) + 1d);
        }
    }
}
