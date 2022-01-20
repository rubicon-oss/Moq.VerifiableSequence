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
using Moq.Language;
using Moq.Language.Flow;

namespace Moq;

internal class SetupSetterWrapper<T, TProperty> : ISetupSetter<T, TProperty>
    where T : class
{
  private readonly ISetupSetter<T, TProperty> _implementation;
  private readonly VerifiableSequence _verifiableSequence;

  internal SetupSetterWrapper (ISetupSetter<T, TProperty> setupSet, VerifiableSequence verifiableSequence)
  {
    _implementation = setupSet;
    _verifiableSequence = verifiableSequence;

    _verifiableSequence.AddStep(setupSet.ToString());
    _implementation.Callback(_ => _verifiableSequence.RecordStep(setupSet.ToString()));
  }

  public ICallbackResult Callback (Action<TProperty> action)
  {
    return _implementation.Callback(action + (_ => _verifiableSequence.RecordStep(_implementation.ToString())));
  }

  #region Unchanged
  public ICallBaseResult CallBase ()
  {
    return _implementation.CallBase();
  }

  public IThrowsResult Throws (Exception exception)
  {
    return _implementation.Throws(exception);
  }

  public IThrowsResult Throws<TException> ()
      where TException : Exception, new()
  {
    return _implementation.Throws<TException>();
  }

  [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMostOnce().")]
  public IVerifies AtMostOnce ()
  {
    return _implementation.AtMostOnce();
  }

  [Obsolete("To verify this condition, use the overload to Verify that receives Times.AtMost(callCount).")]
  public IVerifies AtMost (int callCount)
  {
    return _implementation.AtMost(callCount);
  }

  public void Verifiable ()
  {
    _implementation.Verifiable();
  }

  public void Verifiable (string failMessage)
  {
    _implementation.Verifiable(failMessage);
  }

  public IVerifies Raises (Action<T> eventExpression, EventArgs args)
  {
    return _implementation.Raises(eventExpression, args);
  }

  public IVerifies Raises (Action<T> eventExpression, Func<EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises (Action<T> eventExpression, params object[] args)
  {
    return _implementation.Raises(eventExpression, args);
  }

  public IVerifies Raises<T1> (Action<T> eventExpression, Func<T1, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2> (Action<T> eventExpression, Func<T1, T2, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3> (Action<T> eventExpression, Func<T1, T2, T3, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4> (Action<T> eventExpression, Func<T1, T2, T3, T4, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }

  public IVerifies Raises<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> (Action<T> eventExpression, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, EventArgs> func)
  {
    return _implementation.Raises(eventExpression, func);
  }
  #endregion
}