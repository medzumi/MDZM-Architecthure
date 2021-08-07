using System;
using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

namespace Architecture.ViewModel
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<T> : ICommand
    {
        T Param { get; set; }
    }

    public interface ICommand<T1, T2> : ICommand<T1>
    {
        T2 Param2 { get; set; }
    }
}