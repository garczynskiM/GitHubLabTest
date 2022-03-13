using Microsoft.VisualStudio.TestTools.UnitTesting;
using KataStringCalculator;
using System;

namespace KataStringCalculatorTest
{
    [TestClass]
    public class CalculatorUnitTest
    {
        [TestMethod]
        [DataRow("", 0)]
        public void EmptyStringTest(string input, int expected)
        {
            AssertTest(input, expected);
        }

        [TestMethod]
        [DataRow("1", 1)]
        public void SingleNumberTest(string input, int expected)
        {
            AssertTest(input, expected);
        }

        [TestMethod]
        [DataRow("8, 10", 18)]
        public void TwoNumbersTest(string input, int expected)
        {
            AssertTest(input, expected);
        }

        [TestMethod]
        [DataRow("8\n9\n10", 27)]
        public void ThreeNumbersNewlineTest(string input, int expected)
        {
            AssertTest(input, expected);
        }

        [TestMethod]
        [DataRow("8\n9, 10", 27)]
        public void ThreeNumbersTest(string input, int expected)
        {
            AssertTest(input, expected);
        }

        [TestMethod]
        [DataRow("-1")]
        [DataRow("0, 10\n -7")]
        public void NegativeNumbersTest(string input)
        {
            // Arrange
            Calculator calculator = new Calculator();
            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() => calculator.Add(input));
        }

        [TestMethod]
        [DataRow("1001", 0)]
        [DataRow("1, 2\n1001", 3)]
        public void GreaterThan1000Test(string input, int expected)
        {
            AssertTest(input, expected);
        }

        [TestMethod]
        [DataRow("//#10#1,1001\n9", 20)]
        [DataRow("//[10[1,1001\n9", 20)]
        [DataRow("//]10]1,1001\n9", 20)]
        [DataRow("//-10-1,1001\n9", 20)]
        [DataRow("//-10,-1,1001\n9", 20)]
        public void CustomSeparatorTest(string input, int expected)
        {
            AssertTest(input, expected);
        }

        [TestMethod]
        [DataRow("//[###]10###1,1001\n9", 20)]
        [DataRow("//[[]10[1,1001\n9", 20)]
        [DataRow("//[[]]10[]1,1001\n9", 20)]
        [DataRow("//[]]]10]]1,1001\n9", 20)]
        public void CustomLongSeparatorTest(string input, int expected)
        {
            AssertTest(input, expected);
        }

        [TestMethod]
        [DataRow("//[#][##][###]1#2##3###4000, 5000 \n6000", 6)]
        [DataRow("//[]][]]][]]]]1]2]]3]]]4000, 5000 \n6000", 6)]
        [DataRow("//[[][[[][[[[]1[2[[3[[[4000, 5000 \n6000", 6)]
        [DataRow("//[[]][[][]][][]1[]2[3]4000, 5000 \n6000", 6)]
        public void ManyCustomSeparatorsTest(string input, int expected)
        {
            AssertTest(input, expected);
        }

        public void AssertTest(string input, int expected)
        {
            // Arrange
            Calculator calculator = new Calculator();
            // Act
            int result = calculator.Add(input);
            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}