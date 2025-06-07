using System.Threading.Tasks;

public interface IVoucherHeadCacheServices
{
    Task<int> GetNewVoucherNumber();
}