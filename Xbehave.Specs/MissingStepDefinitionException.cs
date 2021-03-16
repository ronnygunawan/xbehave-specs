using System;
using System.Diagnostics.CodeAnalysis;

namespace Xbehave.Specs {
	[SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Other constructors are unused")]
	public class MissingStepDefinitionException : Exception {
		public MissingStepDefinitionException(string step) : base($"'{step}' does not match any step definition.") { }
	}
}
