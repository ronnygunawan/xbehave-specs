using System;

namespace Xbehave.Specs {
	public class MissingStepDefinitionException : Exception {
		public MissingStepDefinitionException(string step) : base($"'{step}' does not match any step definition.") { }
	}
}
