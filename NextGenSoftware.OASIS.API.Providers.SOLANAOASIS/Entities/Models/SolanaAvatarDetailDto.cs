﻿namespace NextGenSoftware.OASIS.API.Providers.SOLANAOASIS.Entities.Models
{
    public class SolanaAvatarDetailDto  : SolanaBaseDto
    {
        public string Address { get; set; }
        public string Mobile { get; set; }
        public int Karma { get; set; }
        public int Xp { get; set; }
    }
}