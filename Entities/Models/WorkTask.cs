﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class IndividualTask
    {
        [Key]
        public Guid Id { get; set; }

        public ICollection<CheckListItem>? CheckListItems { get; set; }

    }
}
