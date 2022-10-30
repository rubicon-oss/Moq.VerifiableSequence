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
using Moq.Language.Flow;

namespace Moq;

internal class SetupWrapper<T, TResult> : ISetup<T, TResult>
    where T : class
{
  private readonly ISetup<T, TResult> _implementation;
  private readonly VerifiableSequence _verifiableSequence;

  internal SetupWrapper (ISetup<T,TResult> setup, VerifiableSequence verifiableSequence)
  {
    _implementation = setup;
    _verifiableSequence = verifiableSequence;

    _verifiableSequence.AddStep(setup.ToString());
    _implementation.Callback(() => _verifiableSequence.RecordStep(setup.ToString()));
  }

  public IReturnsThrows<T, TResult> Callback (InvocationAction action)
  {
    var composite = (_ => _verifiableSequence.RecordStep(_implementation.ToString())) + action.GetAction();
    return _implementation.Callback(new InvocationAction(composite));
  }

  public IReturnsThrows<T, TResult> Callback (Delegate callback)
  {
    throw new NotSupportedException("The 'Callback (Delegate callback)' is not supported. Please use another overload.");
  }

  public IReturnsThrows<T, TResult> Callback (Action action)
  {
    return _implementation.Callback((() => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1> (Action<T1> action)
  {
    return _implementation.Callback((_ => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2> (Action<T1, T2> action)
  {
    return _implementation.Callback(((_, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3> (Action<T1, T2, T3> action)
  {
    return _implementation.Callback(((_, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4> (Action<T1, T2, T3, T4> action)
  {
    return _implementation.Callback(((_, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5> (Action<T1, T2, T3, T4, T5> action)
  {
    return _implementation.Callback(((_, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6> (Action<T1, T2, T3, T4, T5, T6> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7> (Action<T1, T2, T3, T4, T5, T6, T7> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8> (Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9> (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsThrows<T, TResult> Callback<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
  {
    return _implementation.Callback(((_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _) => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

  public IReturnsResult<T> Returns (TResult value)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(value), _verifiableSequence);
  }

  public IReturnsResult<T> Returns (InvocationFunc valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns (Delegate valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns (Func<TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1> (Func<T1, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> CallBase ()
  {
    return new ReturnsResultWrapper<T>(_implementation.CallBase(), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2> (Func<T1, T2, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3> (Func<T1, T2, T3, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4> (Func<T1, T2, T3, T4, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5> (Func<T1, T2, T3, T4, T5, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6> (Func<T1, T2, T3, T4, T5, T6, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7> (Func<T1, T2, T3, T4, T5, T6, T7, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8> (Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9> (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

  public IReturnsResult<T> Returns<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> (Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> valueFunction)
  {
    return new ReturnsResultWrapper<T>(_implementation.Returns(valueFunction), _verifiableSequence);
  }

#region Unchanged
  public IThrowsResult Throws (Exception exception)
  {
    return _implementation.Throws(exception);
  }

  public IThrowsResult Throws<TException> ()
      where TException : Exception, new()
  {
    return _implementation.Throws<TException>();
  }

  public void Verifiable ()
  {
    _implementation.Verifiable();
  }

  public void Verifiable (string failMessage)
  {
    _implementation.Verifiable(failMessage);
  }
#endregion
}