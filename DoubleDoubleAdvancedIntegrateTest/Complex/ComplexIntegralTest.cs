using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;
using DoubleDoubleComplex;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class ComplexIntegralTest {
        [TestMethod]
        public void Test1() {
            (Complex value, ddouble error, _) = ComplexIntegral.AdaptiveIntegrate(
                z => z * z,
                Line2D.Circle,
                Interval.OmniAzimuth, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue((value - 0d).Norm < 1e-28);
        }

        [TestMethod]
        public void Test2() {
            (Complex value, ddouble error, _) = ComplexIntegral.AdaptiveIntegrate(
                z => 1 / z,
                Line2D.Circle,
                Interval.OmniAzimuth, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue((value - (0d, 2 * ddouble.Pi)).Norm < 1e-28);
        }

        [TestMethod]
        public void Test3() {
            (Complex value, ddouble error, _) = ComplexIntegral.AdaptiveIntegrate(
                z => Complex.Exp(z),
                new Line2D(t => (t, 1d), t => (1d, 0d)),
                (0, 1), 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue((value - (Complex.Exp((1d, 1d)) - Complex.Exp((0, 1d)))).Norm < 1e-28);
        }

        [TestMethod]
        public void Test4() {
            (Complex value, ddouble error, _) = ComplexIntegral.AdaptiveIntegrate(
                z => Complex.Cos(z),
                Line2D.Circle,
                (0, ddouble.Pi / 2), 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue((value - (-0.8414709848078964, 1.1752011936438012)).Norm < 1e-12);
        }

        [TestMethod]
        public void Test5() {
            (Complex value, ddouble error, _) = ComplexIntegral.AdaptiveIntegrate(
                z => 1 / (z * z + 1),
                Line2D.Circle,
                (-ddouble.Pi / 4, ddouble.Pi / 4), 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue((value - (0, 0.881373587019543)).Norm < 1e-12);
        }
    }
}