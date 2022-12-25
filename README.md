# Moq.VerifiableSequence

## Availability
NuGet-packages for Moq.VerifiableSequence are available at https://nuget.re-motion.org/feeds/re-motion-infrastructure/Moq.VerifiableSequence

## Usage

```c#
using Moq;

public interface IMockable
{
  public void Method(string a0);
}

var seq = new VerifiableSequence();
var mock = new Mock<IMockable>();
mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

seq.Verify();
```