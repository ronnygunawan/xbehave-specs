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

		[Scenario]
		public void Addition1() {
			Spec.IsInMarkdownDocument(this, "Calculator.md", "Addition 1");
		}

		[Scenario]
		public void Addition2() {
			Spec.IsInMarkdownDocument(this, "Calculator.md", "Addition 2");
		}

		[Scenario]
		public void Addition3() {
			Spec.IsInMarkdownDocument(this, "Calculator.md", "Addition 3");
		}

		[Scenario]
		public void Addition4() {
			Spec.IsInMarkdownDocument(this, "Calculator.md", "Addition 4");
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
