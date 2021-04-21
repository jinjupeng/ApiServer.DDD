using System;

namespace ApiServer.EFCore.UOW
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class UnitOfWorkAttribute : Attribute
    {
        public bool IsDisabled { get; set; }
    }
}
