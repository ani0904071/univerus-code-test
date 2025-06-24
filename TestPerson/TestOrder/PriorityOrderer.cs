using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestPerson.TestOrder
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<ITestCase> OrderTestCases(IEnumerable<ITestCase> testCases)
        {
            var sorted = testCases.Select(tc =>
            {
                var priorityAttr = tc.TestMethod.Method
                    .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName)
                    .FirstOrDefault();

                var priority = priorityAttr == null
                    ? 0
                    : priorityAttr.GetNamedArgument<int>("Priority");

                return new { testCase = tc, priority };
            });

            return sorted.OrderBy(x => x.priority).Select(x => x.testCase);
        }

        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            throw new NotImplementedException();
        }
    }
}
