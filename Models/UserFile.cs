using System;
using System.ComponentModel.DataAnnotations;

namespace EncryptedFileApp.Models
{
    public class UserFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public DateTimeOffset UploadTime { get; set; }

        [Required]
        public string SavedFileName { get; set; } = string.Empty;

        public int BrugerId { get; set; }

        // Her gemmer den, den krypterede fil i databasen
        [Required]
        public byte[] EncryptedData { get; set; } = Array.Empty<byte>();
    }
}
