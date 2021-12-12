using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatsWepApiWithDb.BL
{
    public enum MurcatResultStatus
    {
        Ok,
        NotFound,
        AlreadyExists,
        DataSaveFailed,
        WrongInput,
    }

    public class MurcatResult
    {
        public MurcatResultStatus Status { get; set; }

        public MurcatResult(MurcatResultStatus status)
        {
            Status = status;
        }
    }

    public class MurcatResult<T> : MurcatResult where T: class
    {
        public T Value { get; set; }

        public MurcatResult(MurcatResultStatus status) : base(status) { }

        public MurcatResult(MurcatResultStatus status, T value) : base(status)
        {
            Value = value;
        }

        public MurcatResult(T value) : this(MurcatResultStatus.Ok, value) { }
    }
}
