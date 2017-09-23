using System.Data;

namespace RoadFlow.Data.Interface {
    public interface IPaging {
        DataTable GetPagerData(out int allPages, out int count, string query, int pageIndex, int pageSize);

    }
}