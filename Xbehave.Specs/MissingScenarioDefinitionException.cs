using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Xbehave.Specs {
	[SuppressMessage("Design", "RCS1194:Implement exception constructors.", Justification = "Other constructors are unused")]
	public class MissingScenarioDefinitionException : Exception {
		public MissingScenarioDefinitionException() : base("Scenario is not decorated with [SpecBehave.Scenario] attribute.") { }
		public MissingScenarioDefinitionException(MethodBase method) : base($"Scenario '{method.DeclaringType?.Name}.{method.Name}' doesn't have scenario definition.") { }
	}
}
