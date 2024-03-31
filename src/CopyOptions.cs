using System.ComponentModel.DataAnnotations;

namespace DragonsDogma2FileBackupWorker;

public class CopyOptions : IValidatableObject
{ 
    public string DestinationDirectory { get; set; } = @"C:\temp\DragonsDogma2Backups";
    public int WaitTimeInSeconds { get; set; } = 900;
    public IEnumerable<ValidationResult> Validate(ValidationContext? validationContext)
    { 
        if (!Path.IsPathRooted(DestinationDirectory))
        {
            yield return new ValidationResult("Destination directory must be provided.", new[] { nameof(DestinationDirectory) });
        }
        
        if (WaitTimeInSeconds < 0)
        {
            yield return new ValidationResult("Wait time must be greater than or equal to 0.", new[] { nameof(WaitTimeInSeconds) });
        }
    }
}