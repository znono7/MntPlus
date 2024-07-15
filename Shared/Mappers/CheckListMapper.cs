using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class CheckListMapper
    {
        public static CheckListDto? Map(CheckList? checkList)
        {
            if (checkList == null)
            {
                return null;
            }
            return new CheckListDto(checkList.Id,
                                    checkList.Description,
                                    checkList.Name);
        }
    }
}
