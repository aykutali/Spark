﻿

using System.ComponentModel.DataAnnotations.Schema;

namespace SparkApp.Data.Models
{
    public class GamePlatform
    {

        public Guid GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = null!;

        public Guid PlatformId { get; set; }

        [ForeignKey(nameof(PlatformId))]
        public Platform Platform { get; set; } = null!;

        public string? LinkToPlatform { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
