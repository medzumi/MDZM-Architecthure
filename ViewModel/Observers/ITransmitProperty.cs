using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ITransmitProperty<T> : IObserver<T>
{
    IAsyncReactiveProperty<T> Property { get; set; }
}
