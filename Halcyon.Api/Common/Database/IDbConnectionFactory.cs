using System.Data;

namespace Halcyon.Api.Common.Database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
