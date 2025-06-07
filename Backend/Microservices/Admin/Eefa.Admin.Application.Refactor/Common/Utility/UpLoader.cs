using Eefa.Admin.ApplicationRefector;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class UpLoader : IUpLoader
{
    private readonly IApplicationUser _applicationUser;

    public UpLoader(IApplicationUser applicationUser)
    {
        _applicationUser = applicationUser;
    }

    public async Task<IoResult> UpLoadAsync(IFormFile file, string fileName,
        CustomPath customPath, CancellationToken cancellationToken = new CancellationToken())
    {
        if (file.Length <= 0)
        {
            return new IoResult();
        }
        if (string.IsNullOrEmpty(fileName)) fileName = Guid.NewGuid().ToString();

        var extention = Path.GetExtension(file.FileName);
        var path = GetCustomePath(customPath);
        var reletivePath = Path.Combine(path, $"{fileName}{extention}");
        var fullPath = Path.Combine(_applicationUser.GetIoPaths().Root, reletivePath);

        try
        {
            var dir = new System.IO.FileInfo(fullPath);
            dir?.Directory?.Create();

            await using var myFile = File.Create(fullPath);
            await file.CopyToAsync(myFile, cancellationToken);


        }
        catch (Exception e)
        {
            return new IoResult();
        }

        return new IoResult() { FullPath = "assets/" + fullPath.Replace('\\', '/'), ReletivePath = "assets/" + reletivePath.Replace('\\', '/'), FileName = fileName };
    }



    public async Task<IoResult> UpLoadAsync(string sourceFileReletiveAddress, CustomPath customPath,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var path = GetCustomePath(customPath);
        var fileName = Path.GetFileName(sourceFileReletiveAddress);
        var sourcePath = Path.Combine(_applicationUser.GetIoPaths().Root, sourceFileReletiveAddress.Remove(0, 7));
        var reletiveDestinationPath = Path.Combine(path, fileName);
        var destinationFullPath = Path.Combine(_applicationUser.GetIoPaths().Root, reletiveDestinationPath);

        sourcePath = sourcePath.Replace('/', '\\');
        destinationFullPath = destinationFullPath.Replace('/', '\\');

        await using (var source = File.Open(sourcePath, FileMode.OpenOrCreate))
        {
            var dir = new System.IO.FileInfo(destinationFullPath);
            dir?.Directory?.Create();

            await using (var destination = File.Create(destinationFullPath))
            {
                await source.CopyToAsync(destination, cancellationToken);
            }
        }

        return new IoResult() { FullPath = "assets/" + destinationFullPath.Replace('\\', '/'), ReletivePath = "assets/" + reletiveDestinationPath.Replace('\\', '/'), FileName = fileName };
    }


    private string GetCustomePath(CustomPath customPath)
    {
        string path;
        switch (customPath)
        {
            case CustomPath.PersonProfile:
                path = _applicationUser.GetIoPaths().PersonProfile;
                break;
            case CustomPath.Signature:
                path = _applicationUser.GetIoPaths().PersonSignature;
                break;
            case CustomPath.Temp:
                path = _applicationUser.GetIoPaths().Temp;
                break;
            case CustomPath.FlagImage:
                path = _applicationUser.GetIoPaths().FlagImage;
                break;
            case CustomPath.Attachment:
                path = _applicationUser.GetIoPaths().Attachment;
                break;
            default:
                path = _applicationUser.GetIoPaths().Root;
                break;
        }
        return path;
    }
}