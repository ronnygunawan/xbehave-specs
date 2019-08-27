[![NuGet](https://img.shields.io/nuget/v/RG.Xbehave.Specs.svg)](https://www.nuget.org/packages/RG.Xbehave.Specs/)

# Installation and Setup

## 1. Install Package

Package Manager Console:
```
Install-Package RG.Xbehave.Specs
```

## 2. Turn On Generation of XML Documentation File

In your .csproj file add:
```xml
<PropertyGroup>
  <DocumentationFile>.\Documentation.xml</DocumentationFile>
</PropertyGroup>
```

## 3. Write Your Scenario in XML Comment

```csharp
using Xbehave;
using Xbehave.Specs;
using Xunit;

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
```

## 4. Implement Step Definitions

```csharp
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
```

[See full example](https://github.com/ronnygunawan/xbehave-specs/blob/master/XUnitTests/CalculatorTests.cs)
