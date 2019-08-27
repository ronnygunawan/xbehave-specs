using FluentAssertions;
using Xbehave;
using Xbehave.Specs;
using Xunit;
using XUnitTests.Models;

namespace XUnitTests {
	public class CalculatorTests : IClassFixture<Calculator> {
		private readonly Calculator _calculator;

		public CalculatorTests(Calculator calculator) {
			_calculator = calculator;
		}

		/// <summary>
		/// Given the number 5
		/// And the number 3
		/// When I add the numbers together
		/// Then the answer is 8
		/// </summary>
		[Scenario]
		public void Addition() {
			Spec.IsInXmlComments(this);
		}

		[Given("the number {x}")]
		protected void GivenTheNumber(int x) {
			_calculator.EnterNumber(x);
		}

		[When("I add the numbers together")]
		protected void WhenIAddTheNumbersTogether() {
			_calculator.PressAdd();
		}

		[Then("the answer is {ans}")]
		protected void ThenTheAnswerIs(int ans) {
			_calculator.Ans.Should().Be(ans);
		}
	}
}
