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

internal class SetupGetterWrapper<T, TProperty> : ISetupGetter<T, TProperty>
    where T : class
{
  private readonly ISetupGetter<T, TProperty> _implementation;
  private readonly VerifiableSequence _verifiableSequence;

  internal SetupGetterWrapper (ISetupGetter<T,TProperty> setupGet, VerifiableSequence verifiableSequence)
  {
    _implementation = setupGet;
    _verifiableSequence = verifiableSequence;

    _verifiableSequence.AddStep(setupGet.ToString());
    _implementation.Callback(() => _verifiableSequence.RecordStep(setupGet.ToString()));
  }

  public IReturnsThrowsGetter<T, TProperty> Callback (Action action)
  {
    return _implementation.Callback((() => _verifiableSequence.RecordStep(_implementation.ToString())) + action);
  }

#region Unchanged
  public IReturnsResult<T> Returns (TProperty value)
  {
    return _implementation.Returns(value);
  }

  public IReturnsResult<T> Returns (Func<TProperty> valueFunction)
  {
    return _implementation.Returns(valueFunction);
  }

  public IReturnsResult<T> CallBase ()
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