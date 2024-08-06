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
    }
}