﻿using Microsoft.Data.SqlClient;
using OOADPRO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOADPRO.Utilities;

public class LoginFunc
{
    public static User VerifiedCredentials(SqlConnection con, Login login)
    {
        SqlCommand cmd = new SqlCommand("spVerifiedUserCredential", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@usr", login._username);
        cmd.Parameters.AddWithValue("@passwd", login._password);
        SqlDataReader? reader = null;
        try
        {
            reader = cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error in getting user with username, {login._username} > {ex.Message}");
        }
        finally
        {
            cmd.Dispose();
        }
        User? verify = null;
        if (reader != null && reader.HasRows == true)
        {
            if (reader.Read() == true)
            {
                verify = reader.ToUserAllDataToLogin();
            }
        }
        reader?.Close();
        return verify;
    }

}
