using System.Threading;
using System.Threading.Tasks;
using Library.Utility;
using Microsoft.AspNetCore.Http;

namespace Library.Interfaces
{
    public interface IUpLoader
    {
        Task<IoResult> UpLoadAsync(IFormFile file, string fileName, CustomPath customPath,
            CancellationToken cancellationToken = new CancellationToken());      
        
        
        Task<IoResult> UpLoadAsync(string sourceFileReletiveAddress, CustomPath customPath,
            CancellationToken cancellationToken = new CancellationToken());
    }
}