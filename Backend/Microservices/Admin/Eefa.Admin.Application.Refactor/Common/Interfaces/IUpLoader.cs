using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;

public interface IUpLoader
{
    Task<IoResult> UpLoadAsync(IFormFile file, string fileName, CustomPath customPath,
        CancellationToken cancellationToken = new CancellationToken());


    Task<IoResult> UpLoadAsync(string sourceFileReletiveAddress, CustomPath customPath,
        CancellationToken cancellationToken = new CancellationToken());
}