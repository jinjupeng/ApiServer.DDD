﻿using System;
using System.Collections.Generic;

namespace ApiServer.Domain
{
    public partial class sys_user_role
    {
        public long role_id { get; set; }
        public long user_id { get; set; }
    }
}
