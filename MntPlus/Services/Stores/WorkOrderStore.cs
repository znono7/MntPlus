﻿using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class WorkOrderStore
    {
        public event Action<WorkOrderDto?>? WorkOrderCreated;

        public void CreateWorkOrder(WorkOrderDto? location)
        {
            WorkOrderCreated?.Invoke(location);
        }
    }
}
