using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.Common.Entities;
using MISA.AMIS.DL;
using MISA.AMIS.DL.PositionDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.PositionBL
{
    public class PositionBL : BaseBL<Position>, IPositionBL
    {
        #region Field
        private IPositionDL _positionDL;
        #endregion
        #region Constructor
        public PositionBL(IPositionDL positionDL) : base(positionDL)
        {
            _positionDL = positionDL;
        }
        #endregion
    }
}
