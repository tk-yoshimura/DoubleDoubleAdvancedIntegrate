﻿using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass()]
    public class SurfaceIntegralTests {
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
                (x, y) => x * x * y * y * y,
                new Surface2D(
                    (u, v) => (u, v),
                    (u, v) => ((1, 0), (0, 1))
                ),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1d / 12) < 1e-14);
        }

        [TestMethod()]
        public void Test3() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => ddouble.Square(x - y),
                new Surface2D(
                    (u, v) => (u, v),
                    (u, v) => ((1, 0), (0, 1))
                ),
                (0, 2), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4d / 3) < 1e-14);
        }

        [TestMethod()]
        public void Test4() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => 2 * x + y,
                new Surface2D(
                    (u, v) => (u, v * (1 - u)),
                    (u, v) => ((1, -v), (0, 1 - u))
                ),
                (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 0.5) < 1e-14);
        }

        [TestMethod()]
        public void Test5() {
            (ddouble value, ddouble error) = SurfaceIntegral.Integrate(
                (x, y) => 1,
                new Surface2D(
                    (theta, r) => (ddouble.Cos(theta) * r, ddouble.Sin(theta) * r),
                    (theta, r) => (
                        (-ddouble.Sin(theta) * r, ddouble.Cos(theta) * r),
                        (ddouble.Cos(theta), ddouble.Sin(theta))
                    )
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
                (x, y) => y,
                new Surface2D(
                    (theta, r) => (ddouble.Cos(theta) * r, ddouble.Sin(theta) * r),
                    (theta, r) => (
                        (-ddouble.Sin(theta) * r, ddouble.Cos(theta) * r),
                        (ddouble.Cos(theta), ddouble.Sin(theta))
                    )
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
                (x, y) => x * x + y * y,
                new Surface2D(
                    (theta, r) => (ddouble.Cos(theta) * r, ddouble.Sin(theta) * r),
                    (theta, r) => (
                        (-ddouble.Sin(theta) * r, ddouble.Cos(theta) * r),
                        (ddouble.Cos(theta), ddouble.Sin(theta))
                    )
                ),
                (0, ddouble.PI * 2), (1, 2)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 15 * ddouble.PI / 2) < 1e-14);
        }
    }
}