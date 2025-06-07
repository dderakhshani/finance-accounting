using FileTransfer.WebApi.Persistance.Entities;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileTransfer.WebApi.Persistance.Sql
{
    public interface IFileTransferUnitOfWork : IUnitOfWork
    {
        DbSet<Archive> Archives { get; set; }
        DbSet<ArchiveAttachments> ArchiveAttachments { get; set; }
        DbSet<Attachment> Attachments { get; set; }
        DbSet<BaseValue> BaseValues { get; set; }
        DbSet<BaseValueType> BaseValueTypes { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Language> Languages { get; set; }
       
        public FileTransferUnitOfWork Mock();

    }


}