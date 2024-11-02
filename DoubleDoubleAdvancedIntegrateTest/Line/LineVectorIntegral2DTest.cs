using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class LineVectorIntegral2DTest {
        [TestMethod]
        public void Test1() {
            (ddouble value, ddouble error, _) = LineVectorIntegral.AdaptiveIntegrate(
                (x, y) => (x * y, x - y),
                Line2D.Line((0, 0), (1, 1)),
                Interval.Unit, eps: 0, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - ddouble.One / 3) < 1e-28);
        }

        [TestMethod]
        public void Test2() {
            (ddouble value, ddouble error, _) = LineVectorIntegral.AdaptiveIntegrate(
                (x, y) => (y, x),
                Line2D.Circle,
                Interval.OmniAzimuth, eps: 0, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 0) < 1e-28);
        }

        [TestMethod]
        public void Test3() {
            (ddouble value, ddouble error, _) = LineVectorIntegral.AdaptiveIntegrate(
                (x, y) => (y, x),
                Line2D.Circle,
                Interval.OmniAzimuth, eps: 0, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 0) < 1e-28);
        }

        [TestMethod]
        public void Test4() {
            (ddouble value, ddouble error, _) = LineVectorIntegral.AdaptiveIntegrate(
                (x, y) => (-y, x),
                Line2D.Circle,
                Interval.OmniAzimuth, eps: 0, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 2 * ddouble.Pi) < 1e-28);
        }

        [TestMethod]
        public void Test5() {
            (ddouble value, ddouble error, _) = LineVectorIntegral.AdaptiveIntegrate(
                (x, y) => (y, -x),
                Line2D.Circle,
                Interval.OmniAzimuth, eps: 0, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value + 2 * ddouble.Pi) < 1e-28);
        }

        [TestMethod]
        public void Test6() {
            (ddouble value, ddouble error, _) = LineVectorIntegral.AdaptiveIntegrate(
                (x, y) => (-y, x),
                Line2D.Circle,
                (2 * ddouble.Pi, 0), eps: 0, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value + 2 * ddouble.Pi) < 1e-28);
        }
    }
}