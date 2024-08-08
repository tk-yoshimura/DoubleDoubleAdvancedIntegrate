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
                0, ddouble.PI * 2, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - ddouble.PI * 2) < 1e-28);
        }

        [TestMethod]
        public void Test2() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y) => x + y,
                new Line2D(
                    t => (t, t * t),
                    t => (1, 2 * t)
                ),
                0, 1, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - "1.454698971663725980684111673902") < 1e-28);
        }
    }
}