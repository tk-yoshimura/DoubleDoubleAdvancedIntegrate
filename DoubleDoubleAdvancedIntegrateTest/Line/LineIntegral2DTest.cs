using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class LineIntegral2DTest {
        [TestMethod]
        public void Test1() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y) => x * x + y * y,
                Line2D.Circle,
                Interval.OmniAzimuth, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - ddouble.Pi * 2) < 1e-28);
        }

        [TestMethod]
        public void Test2() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y) => x + y,
                new Line2D(
                    t => (t, t * t),
                    t => (1, 2 * t)
                ),
                Interval.Unit, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - "1.454698971663725980684111673902") < 1e-28);
        }

        [TestMethod]
        public void Test3() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y) => 1,
                Line2D.Circle * (1.5, 2),
                Interval.OmniAzimuth, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 4 * 2 * ddouble.EllipticE(1 - 2.25 / 4)) < 1e-28);
        }

        [TestMethod]
        public void Test4() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y) => 1,
                Line2D.Line((0, 1), (2, 3)),
                Interval.Unit, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 2 * ddouble.Sqrt2) < 1e-28);
        }
    }
}