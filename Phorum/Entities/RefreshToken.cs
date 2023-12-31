﻿namespace Phorum.Entities
{
    public class RefreshToken
    {
        public string TokenId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsBlackListed { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
