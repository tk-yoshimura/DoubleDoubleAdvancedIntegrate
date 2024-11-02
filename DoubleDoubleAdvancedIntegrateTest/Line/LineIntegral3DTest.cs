using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class LineIntegral3DTest {
        [TestMethod]
        public void Test1() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y, z) => x + y + z,
                Line3D.Line((0, 0, 0), (1, 1, 1)),
                Interval.Unit, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 3 * ddouble.Sqrt(3) / 2) < 1e-28);
        }

        [TestMethod]
        public void Test2() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y * y,
                Line3D.Circle,
                Interval.OmniAzimuth, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - ddouble.Pi * 2) < 1e-28);
        }

        [TestMethod]
        public void Test3() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y, z) => z * z,
                Line3D.Helix,
                Interval.OmniAzimuth, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 8 * ddouble.Cube(ddouble.Pi) * ddouble.Sqrt2 / 3) < 1e-28);
        }

        [TestMethod]
        public void Test4() {
            (ddouble value, ddouble error, _) = LineIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y * y + z,
                Line3D.Circle + (0, 0, 1),
                Interval.OmniAzimuth, 1e-28, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.Pi) < 1e-28);
        }
    }
}