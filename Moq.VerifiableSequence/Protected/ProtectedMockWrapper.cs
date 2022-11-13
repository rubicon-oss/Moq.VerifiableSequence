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

namespace Moq.Protected;

/// <inheritdoc cref="IProtectedMock{T}"/>
public class ProtectedMockWrapper<T>
    where T : class
{
  private readonly MockWrapper<T> _mockWrapper;

  internal ProtectedMockWrapper (MockWrapper<T> mockWrapper)
  {
    if (mockWrapper == null) throw new ArgumentNullException(nameof(mockWrapper));

    _mockWrapper = mockWrapper;
  }

  /// <inheritdoc cref="IProtectedMock{T}.As{TAnalog}"/>
  public ProtectedAsMockWrapper<T, TAnalog> As<TAnalog> ()
      where TAnalog : class
  {
    return new ProtectedAsMockWrapper<T, TAnalog>(_mockWrapper);
  }

  /// <inheritdoc cref="IProtectedMock{T}.Setup(string, object[])"/>
  public ISetup<T> Setup (string voidMethodName, params object[] args)
  {
    var setup = _mockWrapper.Mock.Protected().Setup(voidMethodName, args);
    return new SetupWrapper<T>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedMock{T}.Setup(string, bool, object[])"/>
  public ISetup<T> Setup (string voidMethodName, bool exactParameterMatch, params object[] args)
  {
    var setup = _mockWrapper.Mock.Protected().Setup(voidMethodName, exactParameterMatch, args);
    return new SetupWrapper<T>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedMock{T}.Setup(string, Type[], bool, object[])"/>
  public ISetup<T> Setup (string voidMethodName, Type[] genericTypeArguments, bool exactParameterMatch, params object[] args)
  {
    var setup = _mockWrapper.Mock.Protected().Setup(voidMethodName, genericTypeArguments, exactParameterMatch, args);
    return new SetupWrapper<T>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedMock{T}.Setup{TResult}(string, object[])"/>
  public ISetup<T, TResult> Setup<TResult> (string methodOrPropertyName, params object[] args)
  {
    var setup = _mockWrapper.Mock.Protected().Setup<TResult>(methodOrPropertyName, args);
    return new SetupWrapper<T, TResult>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedMock{T}.Setup{TResult}(string, bool, object[])"/>
  public ISetup<T, TResult> Setup<TResult> (string methodOrPropertyName, bool exactParameterMatch, params object[] args)
  {
    var setup = _mockWrapper.Mock.Protected().Setup<TResult>(methodOrPropertyName, exactParameterMatch, args);
    return new SetupWrapper<T, TResult>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedMock{T}.Setup{TResult}(string, Type[], bool, object[])"/>
  public ISetup<T, TResult> Setup<TResult> (string methodOrPropertyName, Type[] genericTypeArguments, bool exactParameterMatch, params object[] args)
  {
    var setup = _mockWrapper.Mock.Protected().Setup<TResult>(methodOrPropertyName, genericTypeArguments, exactParameterMatch, args);
    return new SetupWrapper<T, TResult>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedMock{T}.SetupGet{TProperty}(string)"/>
  public ISetupGetter<T, TProperty> SetupGet<TProperty> (string propertyName)
  {
    var setup = _mockWrapper.Mock.Protected().SetupGet<TProperty>(propertyName);
    return new SetupGetterWrapper<T, TProperty>(setup, _mockWrapper.VerifiableSequence);
  }

  /// <inheritdoc cref="IProtectedMock{T}.SetupSet{TProperty}(string, object"/>
  public ISetupSetter<T, TProperty> SetupSet<TProperty> (string propertyName, object value)
  {
    var setup = _mockWrapper.Mock.Protected().SetupSet<TProperty>(propertyName, value);
    return new SetupSetterWrapper<T, TProperty>(setup, _mockWrapper.VerifiableSequence);
  }
}