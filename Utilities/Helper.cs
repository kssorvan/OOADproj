using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace OOADPRO.Utilities;

public static class Helper
{
    public static string ConnectionStringKey { get; set; } = "DBConnectionString";
    public static IConfiguration? Configuration { get; set; } = null;

    public static SqlConnection? Connection { get; private set; } = null;
    public static void LoadConfiguration(string jsonFile)
    {
        var builder = new ConfigurationBuilder()
          .AddJsonFile(jsonFile, optional: false, reloadOnChange: true);
        Configuration = builder.Build();
    }
    public static SqlConnection OpenConnection()
    {
        try
        {
            string? connStr = Configuration?.GetRequiredSection(ConnectionStringKey).Value;
            var conn = new SqlConnection(connStr);

            conn.Open();
            Connection = conn;
            if (Connection != null)
            {
                MessageBox.Show($"Connected to server successfully", "Connection To Server");
            }
            return conn;
        }
        catch (Exception ex)
        {
            Connection = null;
            throw new Exception($"Failed to connect to the server > {ex.Message}");
        }
    }
    public static string GetDBConnectionSetting(string connectionType)
    {
        var procSettingSection = Configuration?.GetRequiredSection($"{connectionType}");
        string? procSetting = procSettingSection?.Value;
        return procSetting;
    }


}
