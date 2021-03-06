﻿using System;
using System.Collections.Generic;

namespace Core.Entities
{
    /// <summary>
    /// Story point.
    /// </summary>
    public class StoryPoint: Entity
    {
        /// <summary>
        /// List of values.
        /// </summary>
        public static List<string> Estimates = new List<string>
        {
            "0",
            "1/2",
            "1",
            "2",
            "3",
            "5",
            "8",
            "13",
            "20",
            "40",
            "100",
            "?",
            "Invalid",
            "Won'tFix",
            "Duplicate",
        };

        /// <summary>
        /// User.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Task.
        /// </summary>
        public virtual Story Story { get; set; }

        /// <summary>
        /// Estimate value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Date of creation.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}