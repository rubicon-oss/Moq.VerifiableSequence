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
using Moq.Protected;
using NUnit.Framework;

namespace Moq.Tests.Protected;

[TestFixture]
public class ProtectedTests
{
  public class Mockable
  {
    protected virtual void Method ()
    {
    }

    protected virtual string Method (string a0)
    {
      return "<default>";
    }

    protected virtual string Method (string a0, string a1)
    {
      return "<default>";
    }

    protected virtual void GenericMethod<T> ()
    {
    }

    protected virtual string GenericMethod<T> (T a0)
    {
      return "<default>";
    }

    protected virtual string Getter { get; }

    protected virtual string Setter
    {
      set { }
    }

    public void CallMethod ()
    {
      Method();
    }

    public string CallMethod (string a0)
    {
      return Method(a0);
    }

    public string CallMethod (string a0, string a1)
    {
      return Method(a0, a1);
    }


    public void CallGenericMethod<T> ()
    {
      GenericMethod<T>();
    }

    public string CallGenericMethod<T> (T a0)
    {
      return GenericMethod<T>(a0);
    }

    public string CallGetter
    {
      get { return Getter; }
    }

    public string CallSetter
    {
      set { Setter = value; }
    }
  }

  public interface IMockable
  {
    public void Method ();

    public string Method (string a0);


    public string Getter { get; }
    public string Setter { get; set; }
  }

  [Test]
  public void LooseMock_SingleSetupWithNotVoidReturn ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();

    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Returns("result");

    var result = mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(result, Is.EqualTo("result"));
  }

  [Test]
  public void LooseMock_SingleSetupWithNonVoidReturn_OverloadWithoutParameterMatch ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();

    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", new object[] { "0" })
        .Returns("result");

    var result = mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(result, Is.EqualTo("result"));
  }

  [Test]
  public void LooseMock_SingleSetupWithNotVoidReturn_WithGenericTypeArguments ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();

    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>("GenericMethod", genericTypeArguments: new[] { typeof(string) }, exactParameterMatch: true, new object[] { "0" })
        .Returns("result");

    var result = mock.Object.CallGenericMethod<string>("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(result, Is.EqualTo("result"));
  }

  [Test]
  public void LooseMock_SingleSetupWithNotVoidReturn_AsDuckType ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();

    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup(_ => _.Method("0")).Returns("result");

    var result = mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(result, Is.EqualTo("result"));
  }

  [Test]
  public void LooseMock_MultipleSetupsWithCallsInOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "0" });
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "1" });
    mock.Object.CallMethod("0");
    mock.Object.CallMethod("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MultipleSetupsWithMissingCall ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "0" });
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "1" });
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(
                @"Verification failed: Not all setups were matched:
- [OK]       'mock => mock.Method(""0"")'
- [Expected] 'mock => mock.Method(""1"")'
"));
  }

  [Test]
  public void LooseMock_MultipleSetupsWithMissingCall_AsDuckType ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup<string>(_ => _.Method("0")).Returns("result");
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup<string>(_ => _.Method("1")).Returns("result");
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(
                @"Verification failed: Not all setups were matched:
- [OK]       '_ => _.Method(""0"")'
- [Expected] '_ => _.Method(""1"")'
"));
  }

  [Test]
  public void LooseMock_MultipleSetupsWithWrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "0" });
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "1" });

    var action = () => mock.Object.CallMethod("1");

    Assert.That(
        action,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action 'mock => mock.Method(""1"")' does not match setup 'mock => mock.Method(""0"")'."));
  }

  [Test]
  public void LooseMock_NonSequenceSetupCanBeExecutedWithin ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "0" });
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "2" });
    mock.Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "1" });

    mock.Object.CallMethod("0");
    mock.Object.CallMethod("1");
    mock.Object.CallMethod("2");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MethodReturningVoid ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "0" });
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[0]);
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "2" });

    mock.Object.CallMethod("0");
    mock.Object.CallMethod();
    mock.Object.CallMethod("2");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MethodReturningVoid_WithoutExactParameterMatch ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", new object[0]);

    mock.Object.CallMethod();

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MethodReturningVoid_WithGenericArgumentTypes ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock
        .InVerifiableSequence(seq)
        .Protected()
        .Setup(voidMethodName: "GenericMethod", genericTypeArguments: new[] { typeof(string) }, exactParameterMatch: true, new object[0]);

    mock.Object.CallGenericMethod<string>();

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MethodReturningVoid_AsDuckType ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup(_ => _.Method());

    mock.Object.CallMethod();

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_MethodReturningVoid_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "0" });
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[0]);
    mock.InVerifiableSequence(seq).Protected().Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "2" });


    mock.Object.CallMethod("0");

    var action = () => mock.Object.CallMethod("2");

    Assert.That(
        action,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action 'mock => mock.Method(""2"")' does not match setup 'mock => mock.Method()'."));
  }

  [Test]
  public void LooseMock_MethodReturningVoid_WrongOrder_AsDuckType ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup(_ => _.Method("0"));
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup(_ => _.Method());
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup(_ => _.Method("2"));

    mock.Object.CallMethod("0");

    var action = () => mock.Object.CallMethod("2");

    Assert.That(
        action,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""2"")' does not match setup '_ => _.Method()'."));
  }

  [Test]
  public void LooseMock_MethodReturningVoid_CallbackAction_MockMethodNotCalled_FailsWithVerifiableSequenceException ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup(voidMethodName: "Method", exactParameterMatch: true, new object[0])
        .Callback(() => called = true);

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.TypeOf<VerifiableSequenceException>()
            .With.Message.EqualTo("Verification failed: Not all setups were matched:\r\n- [Expected] 'mock => mock.Method()'\r\n"));
    Assert.That(called, Is.False);
  }

  [Test]
  public void LooseMock_MethodReturningVoid_CallbackAction_SubsequentCalls ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup(voidMethodName: "Method", exactParameterMatch: true, new object[0])
        .Callback(() => called = true);
    mock.InVerifiableSequence(seq).Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "a", "b" })
        .Returns("");
    mock.Object.CallMethod();
    mock.Object.CallMethod("a", "b");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_MethodReturningVoid_CallbackAction_TriggersSecondCall ()
  {
    var calledFirst = false;
    var calledSecond = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup(voidMethodName: "Method", exactParameterMatch: true, new object[0])
        .Callback(
            () =>
            {
              calledFirst = true;
              mock.Object.CallMethod("a", "b");
            });
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "a", "b" })
        .Callback(() => calledSecond = true)
        .Returns("");
    mock.Object.CallMethod();

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(calledFirst, Is.True);
    Assert.That(calledSecond, Is.True);
  }

  [Test]
  public void LooseMock_CallbackAction ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Callback(() => called = true)
        .Returns("");
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_CallbackAction_MockMethodNotCalled_FailsWithVerifiableSequenceException ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Callback(() => called = true)
        .Returns("");

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.TypeOf<VerifiableSequenceException>()
            .With.Message.EqualTo("Verification failed: Not all setups were matched:\r\n- [Expected] 'mock => mock.Method(\"0\")'\r\n"));
    Assert.That(called, Is.False);
  }

  [Test]
  public void LooseMock_CallbackAction_SubsequentCalls ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Callback(() => called = true)
        .Returns("");
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "a", "b" })
        .Returns("");
    mock.Object.CallMethod("0");
    mock.Object.CallMethod("a", "b");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_CallbackAction_TriggersSecondCall ()
  {
    var calledFirst = false;
    var calledSecond = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Callback(
            () =>
            {
              calledFirst = true;
              mock.Object.CallMethod("a", "b");
            })
        .Returns("");
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "a", "b" })
        .Callback(() => calledSecond = true)
        .Returns("");
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(calledFirst, Is.True);
    Assert.That(calledSecond, Is.True);
  }

  [Test]
  public void LooseMock_CallbackActionWithOneParameter ()
  {
    string argument = null;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Callback((string a0) => argument = a0)
        .Returns("");
    mock.Object.CallMethod("0");

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
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup(voidMethodName: "Method", exactParameterMatch: true, new object[] { "0", "1" })
        .Callback(
            (string a0, string a1) =>
            {
              argument0 = a0;
              argument1 = a1;
            });
    mock.Object.CallMethod("0", "1");

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
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Callback(new InvocationAction(_ => called = true))
        .Returns("");
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_CallbackActionWithDelegate_NotSupported ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();

    var setup = () => mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Callback((Delegate)new Action(() => { }))
        .Returns("");

    Assert.That(setup, Throws.InstanceOf<NotSupportedException>());
  }

  [Test]
  public void LooseMock_CallbackActionAfterReturn ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Returns("")
        .Callback(() => called = true);
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_CallbackActionAfterReturn_MockMethodNotCalled_FailsWithVerifiableSequenceException ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected().Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Returns("")
        .Callback(() => called = true);

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.TypeOf<VerifiableSequenceException>()
            .With.Message.EqualTo("Verification failed: Not all setups were matched:\r\n- [Expected] 'mock => mock.Method(\"0\")'\r\n"));
    Assert.That(called, Is.False);
  }

  [Test]
  public void LooseMock_CallbackActionAfterReturn_SubsequentCalls ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Returns("")
        .Callback(() => called = true);
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "a", "b" })
        .Returns("");
    mock.Object.CallMethod("0");
    mock.Object.CallMethod("a", "b");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_CallbackActionAfterReturn_TriggersSecondCall ()
  {
    var calledFirst = false;
    var calledSecond = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Returns("")
        .Callback(
            () =>
            {
              calledFirst = true;
              mock.Object.CallMethod("a", "b");
            });
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "a", "b" })
        .Callback(() => calledSecond = true)
        .Returns("");
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(calledFirst, Is.True);
    Assert.That(calledSecond, Is.True);
  }

  [Test]
  public void LooseMock_CallbackActionWithOneParameterAfterReturn ()
  {
    string argument = null;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Returns("")
        .Callback((string a0) => argument = a0);
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(argument, Is.EqualTo("0"));
  }

  [Test]
  public void LooseMock_CallbackActionWithMultipleParametersAfterReturn ()
  {
    string argument0 = null;
    string argument1 = null;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0", "1" })
        .Returns("")
        .Callback(
            (string a0, string a1) =>
            {
              argument0 = a0;
              argument1 = a1;
            });
    mock.Object.CallMethod("0", "1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(argument0, Is.EqualTo("0"));
    Assert.That(argument1, Is.EqualTo("1"));
  }

  [Test]
  public void LooseMock_CallbackActionWithInvocationActionAfterReturn ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", exactParameterMatch: true, new object[] { "0" })
        .Returns("")
        .Callback(new InvocationAction(_ => called = true));
    mock.Object.CallMethod("0");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_Getter ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "0" }).Returns("result");
    mock.InVerifiableSequence(seq).Protected().SetupGet<string>(propertyName: "Getter").Returns("x");
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "1" }).Returns("value");

    mock.Object.CallMethod("0");
    _ = mock.Object.CallGetter;
    mock.Object.CallMethod("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_Getter_AsDuckType ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup<string>(_ => _.Method("0")).Returns("result");
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().SetupGet<string>(_ => _.Getter).Returns("x");
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup<string>(_ => _.Method("1")).Returns("result");

    mock.Object.CallMethod("0");
    _ = mock.Object.CallGetter;
    mock.Object.CallMethod("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_Getter_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "0" }).Returns("result");
    mock.InVerifiableSequence(seq).Protected().SetupGet<string>(propertyName: "Getter").Returns("x");
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "1" }).Returns("value");

    mock.Object.CallMethod("0");

    var action = () => mock.Object.CallMethod("1");

    Assert.That(
        action,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action 'mock => mock.Method(""1"")' does not match setup 'mock => mock.Getter'."));
  }

  [Test]
  public void LooseMock_Getter_WrongOrder_AsDuckType ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup<string>(_ => _.Method("0")).Returns("result");
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().SetupGet<string>(_ => _.Getter).Returns("x");
    mock.InVerifiableSequence(seq).Protected().As<IMockable>().Setup<string>(_ => _.Method("1")).Returns("result");

    mock.Object.CallMethod("0");

    var action = () => mock.Object.CallMethod("1");

    Assert.That(
        action,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action '_ => _.Method(""1"")' does not match setup '_ => _.Getter'."));
  }

  [Test]
  public void LooseMock_Getter_CallbackAction_MockMethodNotCalled_FailsWithVerifiableSequenceException ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().SetupGet<string>(propertyName: "Getter").Callback(() => called = true).Returns("x");

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.TypeOf<VerifiableSequenceException>()
            .With.Message.EqualTo("Verification failed: Not all setups were matched:\r\n- [Expected] 'mock => mock.Getter'\r\n"));
    Assert.That(called, Is.False);
  }

  [Test]
  public void LooseMock_Getter_CallbackAction_SubsequentCalls ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().SetupGet<string>(propertyName: "Getter").Callback(() => called = true).Returns("x");
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "a", "b" }).Returns("");
    _ = mock.Object.CallGetter;
    mock.Object.CallMethod("a", "b");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_Getter_CallbackAction_TriggersSecondCall ()
  {
    var calledFirst = false;
    var calledSecond = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .SetupGet<string>(propertyName: "Getter")
        .Callback(
            () =>
            {
              calledFirst = true;
              mock.Object.CallMethod("a", "b");
            })
        .Returns("x");
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", new object[] { "a", "b" })
        .Callback(() => calledSecond = true)
        .Returns("");
    _ = mock.Object.CallGetter;

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(calledFirst, Is.True);
    Assert.That(calledSecond, Is.True);
  }

  [Test]
  public void LooseMock_Setter ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "0" }).Returns("result");
    mock.InVerifiableSequence(seq).Protected().SetupSet<string>(propertyName: "Setter", value: "x");
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "1" }).Returns("value");

    mock.Object.CallMethod("0");
    mock.Object.CallSetter = "x";
    mock.Object.CallMethod("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_Setter_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "0" }).Returns("result");
    mock.InVerifiableSequence(seq).Protected().SetupSet<string>(propertyName: "Setter", value: "x");
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "1" }).Returns("value");

    mock.Object.CallMethod("0");

    var actual = () => mock.Object.CallMethod("1");

    Assert.That(
        actual,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action 'mock => mock.Method(""1"")' does not match setup 'mock => mock.Setter = It.IsAny<string>()'."));
  }

  [Test]
  public void LooseMock_Setter_CallbackAction_MockMethodNotCalled_FailsWithVerifiableSequenceException ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().SetupSet<string>(propertyName: "Setter", value: "x").Callback(value => called = true);

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.TypeOf<VerifiableSequenceException>()
            .With.Message.EqualTo(
                "Verification failed: Not all setups were matched:\r\n- [Expected] 'mock => mock.Setter = It.IsAny<string>()'\r\n"));
    Assert.That(called, Is.False);
  }

  [Test]
  public void LooseMock_Setter_CallbackAction_SubsequentCalls ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().SetupSet<string>(propertyName: "Setter", value: "x").Callback(value => called = true);
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "a", "b" }).Returns("");
    mock.Object.CallSetter = "x";
    mock.Object.CallMethod("a", "b");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_Setter_CallbackAction_TriggersSecondCall ()
  {
    var calledFirst = false;
    var calledSecond = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .SetupSet<string>(propertyName: "Setter", value: "x")
        .Callback(
            value =>
            {
              calledFirst = true;
              mock.Object.CallMethod("a", "b");
            });
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", new object[] { "a", "b" })
        .Callback(() => calledSecond = true)
        .Returns("");
    mock.Object.CallSetter = "x";

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(calledFirst, Is.True);
    Assert.That(calledSecond, Is.True);
  }

  [Test]
  public void LooseMock_SetterWithGenericArg ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "0" }).Returns("result");
    mock.InVerifiableSequence(seq).Protected().SetupSet<string>(propertyName: "Setter", value: "x");
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "1" }).Returns("value");

    mock.Object.CallMethod("0");
    mock.Object.CallSetter = "x";
    mock.Object.CallMethod("1");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
  }

  [Test]
  public void LooseMock_SetterWithGenericArg_WrongOrder ()
  {
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "0" }).Returns("result");
    mock.InVerifiableSequence(seq).Protected().SetupSet<string>(propertyName: "Setter", value: "x");
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "1" }).Returns("value");

    mock.Object.CallMethod("0");

    var actual = () => mock.Object.CallMethod("1");

    Assert.That(
        actual,
        Throws.InstanceOf<VerifiableSequenceException>()
            .With.Message.EqualTo(@"Executed action 'mock => mock.Method(""1"")' does not match setup 'mock => mock.Setter = It.IsAny<string>()'."));
  }

  [Test]
  public void LooseMock_SetterWithGenericArg_CallbackAction_MockMethodNotCalled_FailsWithVerifiableSequenceException ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().SetupSet<string>(propertyName: "Setter", value: "x").Callback(_ => called = true);

    var verification = () => seq.Verify();

    Assert.That(
        verification,
        Throws.TypeOf<VerifiableSequenceException>()
            .With.Message.EqualTo(
                "Verification failed: Not all setups were matched:\r\n- [Expected] 'mock => mock.Setter = It.IsAny<string>()'\r\n"));
    Assert.That(called, Is.False);
  }

  [Test]
  public void LooseMock_SetterWithGenericArg_CallbackAction_SubsequentCalls ()
  {
    var called = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq).Protected().SetupSet<string>(propertyName: "Setter", value: "x").Callback(_ => called = true);
    mock.InVerifiableSequence(seq).Protected().Setup<string>(methodOrPropertyName: "Method", new object[] { "a", "b" }).Returns("value");
    mock.Object.CallSetter = "x";
    mock.Object.CallMethod("a", "b");

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(called, Is.True);
  }

  [Test]
  public void LooseMock_SetterWithGenericArg_CallbackAction_TriggersSecondCall ()
  {
    var calledFirst = false;
    var calledSecond = false;
    var seq = new VerifiableSequence();
    var mock = new Mock<Mockable>();
    mock.InVerifiableSequence(seq)
        .Protected()
        .SetupSet<string>(propertyName: "Setter", value: "x")
        .Callback(
            _ =>
            {
              calledFirst = true;
              mock.Object.CallMethod("a", "b");
            });
    mock.InVerifiableSequence(seq)
        .Protected()
        .Setup<string>(methodOrPropertyName: "Method", new object[] { "a", "b" })
        .Callback(() => calledSecond = true)
        .Returns("");
    mock.Object.CallSetter = "x";

    var verification = () => seq.Verify();

    Assert.That(verification, Throws.Nothing);
    Assert.That(calledFirst, Is.True);
    Assert.That(calledSecond, Is.True);
  }
}