﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public interface ICommunicate
    {
        event EventHandler<EventArgs> DataRecieved;
        void Start();
        void Stop();


    }
}
