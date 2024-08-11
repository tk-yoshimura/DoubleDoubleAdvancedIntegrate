using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass()]
    public class SurfaceVectorIntegral3DTests {
        [TestMethod()]
        public void Test1() {
            (ddouble value, ddouble error) = SurfaceVectorIntegral.Integrate(
                (x, y, z) => (x * x, x - y, 1),
                Surface3D.Sphere,
                Interval.OmniAltura, Interval.OmniAzimuth
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - -4 * ddouble.PI / 3) < 1e-3);
        }

        [TestMethod()]
        public void Test2() {
            (ddouble value, ddouble error) = SurfaceVectorIntegral.Integrate(
                (x, y, z) => (-x, -y, -z),
                Surface3D.Cylinder,
                (0, ddouble.PI / 2), (0d, 1d)
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - -ddouble.PI / 2) < 1e-3);
        }

        [TestMethod()]
        public void Test3() {
            (ddouble value, ddouble error) = SurfaceVectorIntegral.Integrate(
                (x, y, z) => (2 * x, -y, z),
                Surface3D.Triangle((1, 0, 0), (0, 1, 0), (0, 0, 1)),
                Interval.Unit, Interval.Unit
            );

            Console.WriteLine(value);
            Console.WriteLine($"{error:e4}");

            Assert.IsTrue(ddouble.Abs(value - 1d / 3) < 1e-3);
        }
    }
}