using System;
using System.Collections.Generic;

namespace MyTaskManagerAPI.Models
{
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
    public class Task
    {
        public required int TaskId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Low; // Default priority // 1=High,2=Medium,3=Low
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public required int CategoryId { get; set; }
        public string? Tags { get; set; } // like assignee, and add optional notes also here
        public Category? Category { get; set; }
    }
}

