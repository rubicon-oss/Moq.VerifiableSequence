// BSD 3-Clause License
//
// Copyright (c) RUBICON IT GmbH
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
//
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
//
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using System;
using NUnit.Framework;

namespace Moq.Tests;

[TestFixture]
public class VerifiableSequenceTests
{
  public interface IMockable
  {
    public void Method ();
    public string Method (string a0);
    public string Method (string a0, string a1);
    public string Getter { get; }
    public string Setter { set; }
    public event Action Event;
  }

  [Test]
  public void EmptySequence ()
  {
    var seq = new VerifiableSequence();

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_SingleSetupWithCall ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.Object.Method("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MultipleSetupsWithCallsInOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));
    mock.Object.Method("0");
    mock.Object.Method("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MultipleSetupsWithMissingCall ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));
    mock.Object.Method("0");

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Verification failed: Not all setups were matched:
- [OK]       '_ => _.Method(""0"")'
- [Expected] '_ => _.Method(""1"")'
"));
  }

  [Test]
  public void LooseMock_MultipleSetupsWithWrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    var action = () => mock.Object.Method("1");

    Assert.That(
        action,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""1"")' does not match setup '_ => _.Method(""0"")'."));
  }

  [Test]
  public void LooseMock_NonSequenceSetupCanBeExecutedWithin ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("2"));
    mock.Setup(_ => _.Method("1"));

    mock.Object.Method("0");
    mock.Object.Method("1");
    mock.Object.Method("2");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_Getter ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupGet(_ => _.Getter);
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");
    _ = mock.Object.Getter;
    mock.Object.Method("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_Getter_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupGet(_ => _.Getter);
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");

    var action = () => mock.Object.Method("1");

    Assert.That(
        action,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""1"")' does not match setup '_ => _.Getter'."));
  }

  [Test]
  public void LooseMock_Setter ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupSet(_ => _.Setter = "");
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");
    mock.Object.Setter = "";
    mock.Object.Method("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_Setter_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupSet(_ => _.Setter = "");
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");

    var actual = () => mock.Object.Method("1");

    Assert.That(
        actual,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""1"")' does not match setup '_ => _.Setter = """"'."));
  }

  [Test]
  public void LooseMock_SetterWithGenericArg ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupSet<string>(_ => _.Setter = "");
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");
    mock.Object.Setter = "";
    mock.Object.Method("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_SetterWithGenericArg_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupSet<string>(_ => _.Setter = "");
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");

    var actual = () => mock.Object.Method("1");

    Assert.That(
        actual,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""1"")' does not match setup '_ => _.Setter = """"'."));
  }

  [Test]
  public void LooseMock_EventAdd ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupAdd(_ => _.Event += It.IsAny<Action>());
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");
    mock.Object.Event += () => { };
    mock.Object.Method("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_EventAdd_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupAdd(_ => _.Event += It.IsAny<Action>());
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");

    var actual = () => mock.Object.Method("1");

    Assert.That(
        actual,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""1"")' does not match setup '_ => _.Event += It.IsAny<Action>()'."));
  }

  [Test]
  public void LooseMock_EventRemove ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupRemove(_ => _.Event -= It.IsAny<Action>());
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");
    mock.Object.Event -= () => { };
    mock.Object.Method("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_EventRemove_WrongOther ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupRemove(_ => _.Event -= It.IsAny<Action>());
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");

    var actual = () => mock.Object.Method("1");

    Assert.That(
        actual,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""1"")' does not match setup '_ => _.Event -= It.IsAny<Action>()'."));
  }

  [Test]
  public void LooseMock_MethodReturningVoid ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).Setup(_ => _.Method());
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("2"));

    mock.Object.Method("0");
    mock.Object.Method();
    mock.Object.Method("2");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MethodReturningVoid_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).Setup(_ => _.Method());
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("2"));

    mock.Object.Method("0");

    var action = () => mock.Object.Method("2");

    Assert.That(
        action,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""2"")' does not match setup '_ => _.Method()'."));
  }

  [Test]
  public void StrictMock_MultipleSetupsWithCallsInOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>(MockBehavior.Strict);
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0")).Returns("");
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1")).Returns("");
    mock.Object.Method("0");
    mock.Object.Method("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_CallbackAction ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0")).Callback(() => called = true).Returns("");
    mock.Object.Method("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_CallbackActionWithOneParameter ()
  {
    string argument = null;
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0")).Callback((string a0) => argument = a0).Returns("");
    mock.Object.Method("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(argument, Is.EqualTo("0"));
  }

  [Test]
  public void LooseMock_CallbackActionWithMultipleParameters ()
  {
    string argument0 = null;
    string argument1 = null;
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq)
        .Setup(_ => _.Method("0", "1"))
        .Callback(
            (string a0, string a1) =>
            {
              argument0 = a0;
              argument1 = a1;
            });
    mock.Object.Method("0", "1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(argument0, Is.EqualTo("0"));
    Assert.That(argument1, Is.EqualTo("1"));
  }

  [Test]
  public void LooseMock_CallbackActionWithInvocationAction ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0")).Callback(new InvocationAction(_ => called = true)).Returns("");
    mock.Object.Method("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_CallbackActionWithDelegate_NotSupported ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();

    var setup = () => mock.InVerifiableSequence(seq).Setup(_ => _.Method("0")).Callback((Delegate)new Action(() => { }));

    Assert.That(setup, Throws.InstanceOf<NotSupportedException>());
  }

  [Test]
  public void LooseMock_Getter_ThrowsSetupOnReturnsThrowsGetterWrapper ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<IMockable>();
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).SetupGet(_ => _.Getter).Callback(() => called = true).Throws(new Exception());
    mock.InVerifiableSequence(seq).Setup(_ => _.Method("1"));

    mock.Object.Method("0");
    Assert.That(() => _ = mock.Object.Getter, Throws.Exception);
    mock.Object.Method("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }
}