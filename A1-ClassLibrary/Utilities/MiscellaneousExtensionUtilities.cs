using System.Data;
using Microsoft.Data.SqlClient;

namespace A1_ClassLibrary.Utilities;

public static class MiscellaneousExtensionUtilities
{
    public static bool IsInRange(this int value, int min, int max) => value >= min && value <= max;

    public static DataTable GetDataTable(this SqlCommand command)
    {
        using var adapter = new SqlDataAdapter(command);

        var table = new DataTable();
        adapter.Fill(table);

        return table;
    }

    public static object GetObjectOrDbNull(this object value) => value ?? DBNull.Value;

}
