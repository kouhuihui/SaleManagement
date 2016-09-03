using Dickson.Core.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace SaleManagement.Core.Models
{
    public class FileInfo
    {
        string m_FileName;

        public FileInfo()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
            Expiration = Created.AddDays(1);
        }

        [StringLength(SaleManagentConstants.Validations.DefaultIdStringLength)]
        public string Id { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.GeneralShorterStringLength)]
        public string FileName
        {
            get { return m_FileName; }
            set
            {
                Requires.NotNullOrEmpty(value, nameof(value));
                m_FileName = value;
                var length = SaleManagentConstants.Validations.GeneralShorterStringLength;
                if (m_FileName.Length > length)
                {
                    var dotIndex = m_FileName.LastIndexOf('.');
                    if (dotIndex == -1)
                    {
                        m_FileName = m_FileName.Substring(0, 50);
                    }
                    else
                    {
                        var extentionName = m_FileName.Substring(dotIndex);
                        var fileName = m_FileName.Substring(0, length - extentionName.Length);
                        m_FileName = fileName + extentionName;
                    }
                }
            }
        }

        public int ContentLength { get; set; }

        [Required, StringLength(SaleManagentConstants.Validations.GeneralStringLength)]
        public string ContentType { get; set; }

        public DateTime Expiration { get; set; }

        [Required]
        public byte[] Data { get; set; }

        public DateTime Created { get; set; }

        [Required, StringLength(64)]
        public string Purpose { get; set; }
    }
}
