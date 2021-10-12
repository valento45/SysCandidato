using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExtensionMethods.ConnectionsGateway
{
    public static class ConnectionExtensionMethods
    {

        public static MySqlConnection ToMySql(this IDbConnection con)
        {
            if(con != null)
            {
                return con as MySqlConnection;
            }
            return null;
        }
    }
}
