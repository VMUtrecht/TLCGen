﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLCGen.Models;

namespace TLCGen.Generators.CCOL.CodeGeneration
{
    public abstract class CCOLCodePieceGeneratorBase : ICCOLCodePieceGenerator
    {
        public virtual void CollectCCOLElements(ControllerModel c)
        {
            
        }

        public virtual bool HasCCOLElements()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<CCOLElement> GetCCOLElements(CCOLElementTypeEnum type)
        {
            throw new NotImplementedException();
        }

        public virtual bool HasCCOLBitmapOutputs()
        {
            return false;
        }

        public virtual IEnumerable<CCOLIOElement> GetCCOLBitmapOutputs()
        {
            throw new NotImplementedException();
        }

        public virtual bool HasCCOLBitmapInputs()
        {
            return false;
        }

        public virtual IEnumerable<CCOLIOElement> GetCCOLBitmapInputs()
        {
            throw new NotImplementedException();
        }

        public virtual bool HasCode(CCOLRegCCodeTypeEnum type)
        {
            return false;
        }

        public virtual string GetCode(ControllerModel c, CCOLRegCCodeTypeEnum type, string tabspace)
        {
            throw new NotImplementedException();
        }
    }
}
