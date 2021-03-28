﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.Blazor.Services
{
    public interface IMessenger
    {
        void DisplaySuccessAlert(string message); 
        void DisplayErrorAlert(string message);
    }
}
