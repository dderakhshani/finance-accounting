public class IoResult
{
    private string _fullPath;
    private string _reletivePath;
    public string FullPath
    {
        get => _fullPath;
        set
        {
            if (!string.IsNullOrEmpty(value)) Succeed = true;
            _fullPath = value;
        }
    }

    public string ReletivePath
    {
        get => _reletivePath;
        set
        {
            if (!string.IsNullOrEmpty(value)) Succeed = true;
            _reletivePath = value;
        }
    }
    public string FileName { get; set; }
    public bool Succeed { get; private set; }
}