using DoubleDouble;

namespace DoubleDoubleAdvancedIntegrate {
    public class Curve4D {
        public Func<ddouble, (ddouble x, ddouble y, ddouble z, ddouble w)> Value { get; }
        public Func<ddouble, (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt)> Diff { get; }

        public Curve4D(
            Func<ddouble, (ddouble x, ddouble y, ddouble z, ddouble w)> value,
            Func<ddouble, (ddouble dxdt, ddouble dydt, ddouble dzdt, ddouble dwdt)> diff) {

            Value = value;
            Diff = diff;
        }

        public Curve4D(
            Func<ddouble, ddouble> x, Func<ddouble, ddouble> y, Func<ddouble, ddouble> z, Func<ddouble, ddouble> w,
            Func<ddouble, ddouble> dxdt, Func<ddouble, ddouble> dydt, Func<ddouble, ddouble> dzdt, Func<ddouble, ddouble> dwdt) {

            Value = t => (x(t), y(t), z(t), w(t));
            Diff = t => (dxdt(t), dydt(t), dzdt(t), dwdt(t));
        }
    }
}
