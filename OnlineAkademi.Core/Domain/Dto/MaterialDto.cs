﻿using OnlineAkademi.Core.Domain.Abstractions;
using OnlineAkademi.Core.Domain.Common;
using OnlineAkademi.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineAkademi.Core.Domain.Dto
{
    public class MaterialDto : BaseEntity, ITrackable
    {
        public int CourseId { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public MaterialType MaterialType { get; set; }

        public string CreateUser { get; set; }

        public string LastupUser { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastupDate { get; set; }
    }
}
