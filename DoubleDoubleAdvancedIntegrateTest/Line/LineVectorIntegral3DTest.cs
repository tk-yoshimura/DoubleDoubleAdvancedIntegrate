using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class LineVectorIntegral3DTest {
        [TestMethod]
        public void Test1() {
            (ddouble value, ddouble error, _) = LineVectorIntegral.AdaptiveIntegrate(
                (x, y, z) => (x * x * y * z, 3 * x * y, y * y),
                Line3D.Helix,
                (0, ddouble.PI / 2), eps: 0, maxdepth: 16
            );

            ddouble expected = 1 + ddouble.PI / 4 - ddouble.PI * ddouble.PI / 64;

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - expected) < 1e-28);
        }
    }
}