using System;
using System.ComponentModel.DataAnnotations;

namespace mtcg.Models
{
	public class Transaction
	{

        public Guid TransactionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PackageId { get; set; }

        [Required]
        public int Price { get; set; }

        public Transaction()
		{
		}
	}
}

