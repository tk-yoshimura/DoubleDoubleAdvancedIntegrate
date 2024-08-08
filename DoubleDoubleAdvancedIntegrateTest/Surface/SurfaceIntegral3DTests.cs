using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass()]
    public class SurfaceIntegral3DTests {
        [TestMethod()]
        public void Test1() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => x + y + z,
                new Surface3D(
                    (u, v) => (u, v, u * u + v * v),
                    (u, v) => ((1, 0, 2 * u), (0, 1, 2 * v))
                ),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 3.4684) < 1e-3);
        }

        [TestMethod()]
        public void Test2() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => z,
                new Surface3D(
                    (u, v) => (2 * ddouble.Cos(u), 2 * ddouble.Sin(u), v),
                    (u, v) => ((-2 * ddouble.Sin(u), 2 * ddouble.Cos(u), 0), (0, 0, 1))
                ),
                (0, ddouble.PI * 2), (0, 4)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 16 * 2 * ddouble.PI) < 1e-3);
        }

        [TestMethod()]
        public void Test3() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => x + y + z,
                new Surface3D(
                    (u, v) => (u * ddouble.Cos(v), u * ddouble.Sin(v), u * u),
                    (u, v) => ((ddouble.Cos(v), ddouble.Sin(v), 2 * u), (-u * ddouble.Sin(v), u * ddouble.Cos(v), 0))
                ),
                (0, 1), (0, ddouble.PI * 2)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 2.97936601549346) < 1e-6);
        }

        [TestMethod()]
        public void Test4() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => ddouble.Sqrt(x * y * z),
                Surface3D.Triangle((1, 0, 0), (0, 1, 0), (0, 0, 1)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 0.10364567795634956) < 1e-6);
        }

        [TestMethod()]
        public void Test5() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y, z) => ddouble.Sqrt(2 * x + y) + z,
                Surface3D.Triangle((1, 0, 0), (0, 1, 0), (0, 0, 1)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1.1331894487786536) < 1e-6);
        }

        [TestMethod()]
        public void Test6() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y, z) => 1,
                Surface3D.Sphere,
                (0, ddouble.PI), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI) < 1e-25);
        }

        [TestMethod()]
        public void Test7() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y + z * z * z,
                Surface3D.Sphere,
                (0, ddouble.PI), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4.188790204786391) < 1e-6);
        }

        [TestMethod()]
        public void Test8() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y + z * z * z,
                Surface3D.Sphere * 2,
                (0, ddouble.PI), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 67.02064327658226) < 1e-6);
        }

        [TestMethod()]
        public void Test9() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y + z * z * z,
                Surface3D.Sphere * 2 + (2, 3, 5),
                (0, ddouble.PI), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 7707.373976806958) < 1e-6);
        }

        [TestMethod()]
        public void Test10() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y + z * z * z,
                Surface3D.Sphere,
                (0, ddouble.PI / 2), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - ddouble.PI * 7 / 6) < 1e-6);
        }

        [TestMethod()]
        public void Test11() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y + z * z * z,
                Surface3D.Sphere,
                (0, ddouble.PI), (-ddouble.PI / 2, ddouble.PI / 2),
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - ddouble.PI * 2 / 3) < 1e-6);
        }

        [TestMethod()]
        public void Test12() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y + z * z * z,
                Surface3D.Rotate(Surface3D.Sphere, (0, 1, 0), ddouble.PI / 2),
                (0, ddouble.PI / 2), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - ddouble.PI * 2 / 3) < 1e-6);
        }

        [TestMethod()]
        public void Test13() {
            (ddouble value, ddouble error, _) = SurfaceIntegral.AdaptiveIntegrate(
                (x, y, z) => x * x + y + z * z * z,
                Surface3D.Sphere,
                (0, ddouble.PI / 4), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1.4212911232567154) < 1e-6);
        }
    }
}