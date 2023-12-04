﻿using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.DAO
{
    public interface ILabelsDao
    {
        public Label GetLabel(Label label);
        public bool AddLabel(Label label);
    }
}
