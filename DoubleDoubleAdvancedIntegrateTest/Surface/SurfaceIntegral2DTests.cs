using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass()]
    public class SurfaceIntegral2DTests {
        [TestMethod()]
        public void Test1() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => x * x * y * y * y,
                Surface2D.Ortho,
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1d / 12) < 1e-14);
        }

        [TestMethod()]
        public void Test2() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => ddouble.Square(x - y),
                Surface2D.Ortho,
                (0, 2), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4d / 3) < 1e-14);
        }

        [TestMethod()]
        public void Test3() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => 2 * x + y,
                Surface2D.Triangle((0, 0), (1, 0), (0, 1)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 0.5) < 1e-14);
        }

        [TestMethod()]
        public void Test4() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => 1,
                Surface2D.Circle,
                (0, 1), (0, ddouble.Pi * 2)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - ddouble.Pi) < 1e-14);
        }

        [TestMethod()]
        public void Test5() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => y,
                Surface2D.Circle,
                (0, 1), (0, ddouble.Pi / 2)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1d / 3) < 1e-14);
        }

        [TestMethod()]
        public void Test6() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => x * x + y * y,
                Surface2D.Circle,
                (1, 2), (0, ddouble.Pi * 2)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 15 * ddouble.Pi / 2) < 1e-14);
        }

        [TestMethod()]
        public void Test7() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => x + y,
                Surface2D.Triangle((0, -1), (1, 0), (0, 1)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1d / 3) < 1e-14);
        }

        [TestMethod()]
        public void Test8() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y) => 1,
                Surface2D.InfinityOrtho,
                (InfSCurve.Invert(-4), InfSCurve.Invert(8)), (InfSCurve.Invert(-2), InfSCurve.Invert(6)),
                eps: 0, maxdepth: 4
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 96) < 1e-4);
        }

        [TestMethod()]
        public void Test9() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y) => ddouble.Exp(-(x * x + y * y)),
                Surface2D.InfinityCircle,
                Interval.Unit, Interval.OmniAzimuth,
                eps: 0, maxdepth: 4
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - ddouble.Pi) < 1e-4);
        }

        [TestMethod()]
        public void Test10() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => 1,
                Surface2D.Rhombus((0, 1), (1, 4), (2, 3)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4) < 1e-14);
        }
    }
}