using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.Common.Entities;
using MISA.AMIS.DL;
using System;

namespace MISA.AMIS.BL
{
    public class DepartmentBL : BaseBL<Department>, IDepartmentBL
    {
        #region Field
        private IDepartmentDL _departmentDL;
        #endregion
        #region Constructor
        public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
        {
            _departmentDL = departmentDL;

        }
        #endregion
    }
}
