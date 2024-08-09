using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass()]
    public class VolumeIntegralTests {
        [TestMethod()]
        public void Test1() {
            (ddouble value, ddouble error) = VolumeIntegral.Integrate(
                (x, y, z) => 1,
                Volume3D.Ortho,
                (-1, 1), (-2, 1), (0, 4)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 24) < 1e-3);
        }

        [TestMethod()]
        public void Test2() {
            (ddouble value, ddouble error) = VolumeIntegral.Integrate(
                (x, y, z) => 1,
                Volume3D.Tetrahedron((0, 0, 0), (1, 0, 0), (0, 2, 0), (0, 0, 3)),
                (0, 1), (0, 1), (0, 1)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1) < 1e-3);
        }

        [TestMethod()]
        public void Test3() {
            (ddouble value, ddouble error) = VolumeIntegral.Integrate(
                (x, y, z) => 1,
                Volume3D.Sphere,
                (0, 1), (0, ddouble.PI), (0, ddouble.PI * 2)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI / 3) < 1e-2);
        }

        [TestMethod()]
        public void Test4() {
            (ddouble value, ddouble error, _) = VolumeIntegral.AdaptiveIntegrate(
                (x, y, z) => 1,
                Volume3D.Sphere,
                (0, 1), (0, ddouble.PI), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2, order: DoubleDoubleIntegrate.GaussKronrodOrder.G15K31
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI / 3) < 1e-25);
        }

        [TestMethod()]
        public void Test5() {
            (ddouble value, ddouble error, _) = VolumeIntegral.AdaptiveIntegrate(
                (x, y, z) => 1,
                Volume3D.Sphere + (1, 2, 3),
                (0, 1), (0, ddouble.PI), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2, order: DoubleDoubleIntegrate.GaussKronrodOrder.G15K31
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI / 3) < 1e-25);
        }

        [TestMethod()]
        public void Test6() {
            (ddouble value, ddouble error, _) = VolumeIntegral.AdaptiveIntegrate(
                (x, y, z) => 1,
                Volume3D.Sphere * (2, 3, 5),
                (0, 1), (0, ddouble.PI), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2, order: DoubleDoubleIntegrate.GaussKronrodOrder.G15K31
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI / 3 * 30) < 1e-25);
        }

        [TestMethod()]
        public void Test7() {
            (ddouble value, ddouble error, _) = VolumeIntegral.AdaptiveIntegrate(
                (x, y, z) => 1,
                Volume3D.Rotate(Volume3D.Sphere, (1, 2, 3), 0.5),
                (0, 1), (0, ddouble.PI), (0, ddouble.PI * 2),
                eps: 0, maxdepth: 2, order: DoubleDoubleIntegrate.GaussKronrodOrder.G15K31
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI / 3) < 1e-25);
        }

        [TestMethod()]
        public void Test8() {
            (ddouble value, ddouble error, _) = VolumeIntegral.AdaptiveIntegrate(
                (x, y, z) => ddouble.Exp(-(x * x + y * y + z * z)),
                Volume3D.InfinitySphere,
                Interval.Unit, Interval.OmniAltura, Interval.OmniAzimuth,
                eps: 0, maxdepth: 2
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - ddouble.Cube(ddouble.Sqrt(ddouble.PI))) < 1e-16);
        }
    }
}