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
using System.Linq.Expressions;
using Moq.Language.Flow;

namespace Moq;

public sealed class MockWrapper<T>
    where T : class
{
  private readonly Mock<T> _mock;
  private readonly VerifiableSequence _verifiableSequence;

  internal MockWrapper (Mock<T> mock, VerifiableSequence verifiableSequence)
  {
    _mock = mock;
    _verifiableSequence = verifiableSequence;
  }

  /// <inheritdoc cref="Mock{T}.Setup"/>
  public ISetup<T> Setup (Expression<Action<T>> expression) =>
      new SetupWrapper<T>(_mock.Setup(expression), _verifiableSequence);

  /// <inheritdoc cref="Mock{T}.Setup"/>
  public ISetup<T, TResult> Setup<TResult> (Expression<Func<T, TResult>> expression) =>
      new SetupWrapper<T, TResult>(_mock.Setup(expression), _verifiableSequence);

  /// <inheritdoc cref="Mock{T}.SetupGet"/>
  public ISetupGetter<T, TProperty> SetupGet<TProperty> (Expression<Func<T, TProperty>> expression) =>
      new SetupGetterWrapper<T, TProperty>(_mock.SetupGet(expression), _verifiableSequence);

  /// <inheritdoc cref="Mock{T}.SetupSet"/>
  public ISetupSetter<T, TProperty> SetupSet<TProperty> (Action<T> setterExpression) =>
      new SetupSetterWrapper<T, TProperty>(_mock.SetupSet<TProperty>(setterExpression), _verifiableSequence);

  /// <inheritdoc cref="Mock{T}.SetupSet"/>
  public ISetup<T> SetupSet (Action<T> setterExpression) =>
      new SetupWrapper<T>(_mock.SetupSet(setterExpression), _verifiableSequence);

  /// <inheritdoc cref="Mock{T}.SetupAdd"/>
  public ISetup<T> SetupAdd (Action<T> addExpression) =>
      new SetupWrapper<T>(_mock.SetupAdd(addExpression), _verifiableSequence);

  /// <inheritdoc cref="Mock{T}.SetupRemove"/>
  public ISetup<T> SetupRemove (Action<T> removeExpression) =>
      new SetupWrapper<T>(_mock.SetupRemove(removeExpression), _verifiableSequence);
}