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

namespace Moq.Protected;

/// <inheritdoc cref="IProtectedAsMock{T, TAnalog}"/>
public class ProtectedAsMockWrapper<T, TAnalog>
    where T : class
    where TAnalog : class
{
  private readonly MockWrapper<T> _mockWrapper;

  public ProtectedAsMockWrapper (MockWrapper<T> mockWrapper)
  {
    if (mockWrapper == null) throw new ArgumentNullException(nameof(mockWrapper));

    _mockWrapper = mockWrapper;
  }

  /// <inheritdoc cref="IProtectedAsMock{T, TAnalog}.Setup(Expression{Action{TAnalog}})"/>
  public ISetup<T> Setup (Expression<Action<TAnalog>> expression)
  {
    var analogMock = _mockWrapper.Mock.Protected().As<TAnalog>();
    var setup = analogMock.Setup(expression);
    return new SetupWrapper<T>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedAsMock{T, TAnalog}.Setup{TResult}(Expression{Func{TAnalog, TResult}})"/>
  public ISetup<T, TResult> Setup<TResult> (Expression<Func<TAnalog, TResult>> expression)
  {
    var analogMock = _mockWrapper.Mock.Protected().As<TAnalog>();
    var setup = analogMock.Setup(expression);
    return new SetupWrapper<T, TResult>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedAsMock{T, TAnalog}.SetupGet{TResult}(Expression{Func{TAnalog, TResult}})"/>
  public ISetupGetter<T, TProperty> SetupGet<TProperty> (Expression<Func<TAnalog, TProperty>> expression)
  {
    var analogMock = _mockWrapper.Mock.Protected().As<TAnalog>();
    var setup = analogMock.SetupGet(expression);
    return new SetupGetterWrapper<T, TProperty>(setup, _mockWrapper.VerifiableSequence);
  }
}