﻿namespace OOADPRO.Models;

public class User
{
    public int UserID { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public Staff? Staff { get; set; }
}
