using System;

namespace Xbehave.Specs {
	public abstract class StepDefinitionBaseAttribute : Attribute {
		public string Regex { get; }

		protected StepDefinitionBaseAttribute(string regex) {
			Regex = "^" + System.Text.RegularExpressions.Regex.Replace(regex, @"\{[a-zA-Z_]+[a-zA-Z0-9_]*\}", @"(\"".*\""|-?[0-9]+|-?[0-9]*\.[0-9]+)") + "$";
		}
	}
}
