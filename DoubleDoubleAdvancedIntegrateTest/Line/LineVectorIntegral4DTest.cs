using DoubleDouble;
using DoubleDoubleAdvancedIntegrate;

namespace DoubleDoubleAdvancedIntegrateTest {
    [TestClass]
    public class LineVectorIntegral4DTest {
        [TestMethod]
        public void Test1() {
            (ddouble value, ddouble error, _) = LineVectorIntegral.AdaptiveIntegrate(
                (x, y, z, w) => (-y, x, -w, z),
                new Line4D(
                    t => (ddouble.Cos(t), ddouble.Sin(t), ddouble.Cos(t), ddouble.Sin(t)),
                    t => (-ddouble.Sin(t), ddouble.Cos(t), -ddouble.Sin(t), ddouble.Cos(t))
                ),
                Interval.OmniAzimuth, eps: 0, maxdepth: 16
            );

            Console.WriteLine(value);
            Assert.IsTrue(ddouble.Abs(value - 4 * ddouble.PI) < 1e-28);
        }
    }
}