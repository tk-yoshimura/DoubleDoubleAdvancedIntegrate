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
                Volume3D.Sphere(),
                (0, 1), (0, ddouble.PI * 2), (-ddouble.PI / 2, ddouble.PI / 2)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI / 3) < 1e-2);
        }

        [TestMethod()]
        public void Test4() {
            (ddouble value, ddouble error, _) = VolumeIntegral.AdaptiveIntegrate(
                (x, y, z) => 1,
                Volume3D.Sphere(),
                (0, 1), (0, ddouble.PI * 2), (-ddouble.PI / 2, ddouble.PI / 2),
                eps: 0, maxdepth: 2, order: DoubleDoubleIntegrate.GaussKronrodOrder.G15K31
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI / 3) < 1e-25);
        }
    }
}