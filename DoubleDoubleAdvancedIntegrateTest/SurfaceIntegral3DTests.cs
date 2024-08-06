﻿using DoubleDouble;
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
                Surface3D.Triangular((1, 0, 0), (0, 1, 0), (0, 0, 1)),
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
                Surface3D.Triangular((1, 0, 0), (0, 1, 0), (0, 0, 1)),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1.1331894487786536) < 1e-6);
        }
    }
}