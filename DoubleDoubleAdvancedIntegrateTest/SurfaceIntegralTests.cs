using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass()]
    public class SurfaceIntegralTests {
        [TestMethod()]
        public void Test1() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => x + y + z,
                (u, v) => new SurfaceIntegral.Surface(u, v, u * u + v * v),
                (u, v) => new SurfaceIntegral.Partials((1, 0, 2 * u), (0, 1, 2 * v)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 3.4684) < 1e-3);
        }

        [TestMethod()]
        public void Test2() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => x * x * y * y * y,
                (u, v) => new SurfaceIntegral.Surface(u, v, 0),
                (u, v) => new SurfaceIntegral.Partials((1, 0, 0), (0, 1, 0)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1d / 12) < 1e-14);
        }

        [TestMethod()]
        public void Test3() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => ddouble.Square(x - y),
                (u, v) => new SurfaceIntegral.Surface(u, v, 0),
                (u, v) => new SurfaceIntegral.Partials((1, 0, 0), (0, 1, 0)),
                (0, 2), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4d / 3) < 1e-14);
        }

        [TestMethod()]
        public void Test4() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => 2 * x + y,
                (u, v) => new SurfaceIntegral.Surface(u, v * (1 - u), 0),
                (u, v) => new SurfaceIntegral.Partials((1, -v, 0), (0, 1 - u, 0)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 0.5) < 1e-14);
        }

        [TestMethod()]
        public void Test5() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => 1,
                (theta, r) => new SurfaceIntegral.Surface(ddouble.Cos(theta) * r, ddouble.Sin(theta) * r, 0),
                (theta, r) => new SurfaceIntegral.Partials(
                    (-ddouble.Sin(theta) * r, ddouble.Cos(theta) * r, 0),
                    (ddouble.Cos(theta), ddouble.Sin(theta), 0)
                ),
                (0, ddouble.PI * 2), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - ddouble.PI) < 1e-14);
        }

        [TestMethod()]
        public void Test6() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => y,
                (theta, r) => new SurfaceIntegral.Surface(ddouble.Cos(theta) * r, ddouble.Sin(theta) * r, 0),
                (theta, r) => new SurfaceIntegral.Partials(
                    (-ddouble.Sin(theta) * r, ddouble.Cos(theta) * r, 0),
                    (ddouble.Cos(theta), ddouble.Sin(theta), 0)
                ),
                (0, ddouble.PI / 2), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1d / 3) < 1e-14);
        }

        [TestMethod()]
        public void Test7() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => x * x + y * y,
                (theta, r) => new SurfaceIntegral.Surface(ddouble.Cos(theta) * r, ddouble.Sin(theta) * r, 0),
                (theta, r) => new SurfaceIntegral.Partials(
                    (-ddouble.Sin(theta) * r, ddouble.Cos(theta) * r, 0),
                    (ddouble.Cos(theta), ddouble.Sin(theta), 0)
                ),
                (0, ddouble.PI * 2), (1, 2)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 15 * ddouble.PI / 2) < 1e-14);
        }
    }
}