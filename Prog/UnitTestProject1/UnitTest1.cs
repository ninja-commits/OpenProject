using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prog;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod4()
        {
            int[][] arrayList =
            {
                new [] { 5, 4, 4 },
                new [] { 4, 3,4 },
                new [] { 3,2,4 },
                new [] { 2,2,2 },
                new[] {3,3,4},
                new [] {1,4,4},
                new [] {4,1,1}
            };
            Assert.AreEqual(11, Program.solution(arrayList));
        }
    }
}
