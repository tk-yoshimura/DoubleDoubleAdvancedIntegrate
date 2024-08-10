using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass()]
    public class LineIntegral4DTests {
        [TestMethod()]
        public void Test1() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y, z, w) => x + y + z + w,
                Line4D.Line((0, 0, 0, 0), (1, 1, 1, 1)),
                Interval.Unit, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 4) < 1e-28);
        }

        [TestMethod()]
        public void Test2() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y, z, w) => x * x + y * y + z * z + w * w,
                new Line4D(
                    t => (ddouble.Cos(t), ddouble.Sin(t), ddouble.Cos(t), ddouble.Sin(t)),
                    t => (-ddouble.Sin(t), ddouble.Cos(t), -ddouble.Sin(t), ddouble.Cos(t))
                ),
                (0, ddouble.PI / 2), 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - ddouble.Sqrt2 * ddouble.PI) < 1e-28);
        }

        [TestMethod()]
        public void Test3() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y, z, w) => w * w,
                new Line4D(
                    t => (ddouble.Cos(t), ddouble.Sin(t), ddouble.Cos(2 * t), t),
                    t => (-ddouble.Sin(t), ddouble.Cos(t), -2 * ddouble.Sin(2 * t), 1d)
                ),
                (0, ddouble.PI * 2), 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 162.20224296712172) < 1e-10);
        }

        [TestMethod()]
        public void Test4() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y, z, w) => x * x + y * y,
                new Line4D(
                    t => (ddouble.Cos(t), ddouble.Sin(t), t, t),
                    t => (-ddouble.Sin(t), ddouble.Cos(t), 1d, 1d),
                    t => ddouble.Sqrt(3)
                ),
                (0, ddouble.PI * 2), 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 2 * ddouble.Sqrt(3) * ddouble.PI) < 1e-10);
        }
    }
}
