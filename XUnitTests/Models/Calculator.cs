using System.Collections.Generic;

namespace XUnitTests.Models {
	public class Calculator {
		public int Ans { get; private set; }
		public Stack<int> EvaluationStack { get; } = new Stack<int>();

		public void EnterNumber(int x) {
			EvaluationStack.Push(x);
		}

		public void PressAdd() {
			Ans = EvaluationStack.Pop() + EvaluationStack.Pop();
		}
	}
}
