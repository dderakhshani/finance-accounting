using FileTransfer.WebApi.Persistance.Entities;
using FileTransfer.WebApi.Persistance.Sql;
using FileTransfer.WebApi.Services.Models;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace FileTransfer.WebApi.Services
{
    public class ArchiveServices : IArchiveServices
    {
        private readonly IFileTransferUnitOfWork _unitOfWork;
        private readonly ICurrentUserAccessor _currentUser;

        public ArchiveServices(IFileTransferUnitOfWork unitOfWork, ICurrentUserAccessor currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<ArchiveModel> CreateArchive(CreateArchiveModel model)
        {
            var archive = new Archive
            {
                TypeBaseId = model.TypeBaseId,
                BaseValueTypeId = model.BaseValueTypeId,
                FileNumber = model.FileNumber,
                Description = model.Description,
                Title = model.Title,
                KeyWords = model.KeyWords,
                CreatedAt = DateTime.UtcNow,
                CreatedById = _currentUser.GetId(),
                OwnerRoleId = _currentUser.GetRoleId()
            };

            archive.ArchiveAttachments = model.AttachmentIds.Select(attachmentId => new ArchiveAttachments
            {
                Archive = archive,
                AttachmentId = attachmentId,
                CreatedAt = DateTime.UtcNow,
                CreatedById = _currentUser.GetId(),
                OwnerRoleId = _currentUser.GetRoleId()
            }).ToList();

            _unitOfWork.Archives.Add(archive);
            archive.ArchiveAttachments.ForEach(archiveAttachment =>
            {
                _unitOfWork.ArchiveAttachments.Add(archiveAttachment);
            });

            await _unitOfWork.SaveAsync(CancellationToken.None);

            return await this.GetArchive(archive.Id);
        }

        public async Task<ArchiveModel> UpdateArchive(UpdateArchiveModel model)
        {
            var archive = await _unitOfWork.Archives.Include(x => x.ArchiveAttachments).FirstOrDefaultAsync(x => x.Id == model.Id);

            archive.Description = model.Description;
            archive.KeyWords = model.KeyWords;
            archive.FileNumber = model.FileNumber;
            archive.Title = model.Title;
            archive.TypeBaseId = model.TypeBaseId;
            archive.ModifiedById = _currentUser.GetId();
            archive.ModifiedAt = DateTime.UtcNow;

            archive.ArchiveAttachments.ForEach(_archiveAttachment =>
            {
                if (!model.AttachmentIds.Contains(_archiveAttachment.AttachmentId))
                {
                    _archiveAttachment.IsDeleted = true;
                    _archiveAttachment.ModifiedById = _currentUser.GetId();
                    _archiveAttachment.ModifiedAt = DateTime.UtcNow;
                    _unitOfWork.ArchiveAttachments.Update(_archiveAttachment);
                }
            });

            model.AttachmentIds.ForEach(attachmendId =>
            {
                if (!archive.ArchiveAttachments.Any(x => x.AttachmentId == attachmendId))
                {
                    var attachment = new ArchiveAttachments
                    {
                        Archive = archive,
                        AttachmentId = attachmendId,
                        CreatedAt = DateTime.UtcNow,
                        CreatedById = _currentUser.GetId(),
                        OwnerRoleId = _currentUser.GetRoleId()
                    };
                }
            });
            _unitOfWork.Archives.Update(archive);

            await _unitOfWork.SaveAsync(CancellationToken.None);

            // remove  old/ add new attachments
            return await this.GetArchive(archive.Id);

        }
        public async Task<ArchiveModel> GetArchive(int id)
        {
            return await _unitOfWork.Archives.Where(x => !x.IsDeleted)
                .Include(x => x.ArchiveAttachments.Where(x => !x.IsDeleted))
                .ThenInclude(x => x.Attachment)
                .Include(x => x.TypeBase)
                .Include(x => x.BaseValueType)
                .Select(archive => new ArchiveModel
                {
                    Id = archive.Id,
                    Description = archive.Description,
                    KeyWords = archive.KeyWords,
                    FileNumber = archive.FileNumber,
                    Title = archive.Title,
                    TypeBaseId = archive.TypeBaseId,
                    BaseValueTypeTitle = archive.BaseValueType.Title,
                    TypeBaseTitle = archive.TypeBase.Title,
                    Attachments = archive.ArchiveAttachments.Select(x => new AttachmentModel
                    {
                        Id = x.Attachment.Id,
                        Description = x.Attachment.Description,
                        KeyWords = x.Attachment.KeyWords,
                        Extention = x.Attachment.Extention,
                        IsUsed = x.Attachment.IsUsed,
                        LanguageId = x.Attachment.LanguageId,
                        Title = x.Attachment.Title,
                        TypeBaseId = x.Attachment.TypeBaseId,
                        Url = x.Attachment.Url
                    }).ToList(),
                }).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<ArchiveModel>> GetArchives(int typeId)
        {
            return await _unitOfWork.Archives.Where(x => !x.IsDeleted)
                .Include(x => x.ArchiveAttachments.Where(x => !x.IsDeleted))
                .ThenInclude(x => x.Attachment)
                .Include(x => x.TypeBase)
                .Include(x => x.BaseValueType)
                .Select(archive => new ArchiveModel
                {
                    Id = archive.Id,
                    Description = archive.Description,
                    KeyWords = archive.KeyWords,
                    FileNumber = archive.FileNumber,
                    Title = archive.Title,
                    TypeBaseId = archive.TypeBaseId,
                    BaseValueTypeId = archive.BaseValueTypeId,
                    BaseValueTypeTitle = archive.BaseValueType.Title,
                    TypeBaseTitle = archive.TypeBase.Title,
                    Attachments = archive.ArchiveAttachments.Select(x => new AttachmentModel
                    {
                        Id = x.Attachment.Id,
                        Description = x.Attachment.Description,
                        KeyWords = x.Attachment.KeyWords,
                        Extention = x.Attachment.Extention,
                        IsUsed = x.Attachment.IsUsed,
                        LanguageId = x.Attachment.LanguageId,
                        Title = x.Attachment.Title,
                        TypeBaseId = x.Attachment.TypeBaseId,
                        Url = x.Attachment.Url
                    }).ToList()
                })
                .Where(x => x.BaseValueTypeId == typeId)
                .ToListAsync();
        }
        public async Task<bool> DeleteArchive(int id)
        {
            var archive = await _unitOfWork.Archives.Include(x => x.ArchiveAttachments).FirstOrDefaultAsync(x => x.Id == id);
            archive.IsDeleted = true;
            foreach (var item in archive.ArchiveAttachments)
            {
                if (!(await _unitOfWork.ArchiveAttachments.AnyAsync(x => x.AttachmentId == item.AttachmentId && x.Id != item.Id)))
                {
                    item.IsDeleted = true;
                    _unitOfWork.ArchiveAttachments.Update(item);
                }
            }
            _unitOfWork.Archives.Update(archive);
            await _unitOfWork.SaveAsync(CancellationToken.None);
            return true;
        }

        public async Task<Library.Models.ServiceResult> SearchAttachment(string searchQuery, int pageSize, int pageIndex)
        {
            var attachments = new List<AttachmentModel>();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var searchKeys = searchQuery
                 .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                 .Append(searchQuery)
                 .Distinct()
                 .ToList();


                var parameter = Expression.Parameter(typeof(Attachment), "x");
                var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));

                Expression? predicate = null;

                foreach (var keyword in searchKeys)
                {
                    var likePattern = Expression.Constant($"%{keyword}%", typeof(string));

                    // x.Title
                    var titleProp = Expression.Property(parameter, nameof(Attachment.Title));
                    var titleLike = Expression.Call(
                        typeof(DbFunctionsExtensions),
                        nameof(DbFunctionsExtensions.Like),
                        Type.EmptyTypes,
                        efFunctions,
                        titleProp,
                        likePattern
                    );

                    // x.Description
                    var descProp = Expression.Property(parameter, nameof(Attachment.Description));
                    var descLike = Expression.Call(
                        typeof(DbFunctionsExtensions),
                        nameof(DbFunctionsExtensions.Like),
                        Type.EmptyTypes,
                        efFunctions,
                        descProp,
                        likePattern
                    );

                    // x.KeyWords
                    var keywordsProp = Expression.Property(parameter, nameof(Attachment.KeyWords));
                    var keywordsLike = Expression.Call(
                        typeof(DbFunctionsExtensions),
                        nameof(DbFunctionsExtensions.Like),
                        Type.EmptyTypes,
                        efFunctions,
                        keywordsProp,
                        likePattern
                    );

                    var orGroup = Expression.OrElse(titleLike, Expression.OrElse(descLike, keywordsLike));
                    predicate = predicate == null ? orGroup : Expression.OrElse(predicate, orGroup);
                }

                var lambda = Expression.Lambda<Func<Attachment, bool>>(predicate!, parameter);
                var query = _unitOfWork.Attachments.Where(lambda);


                var rawResults = await query.AsNoTracking().ToListAsync();

                double minRatio = 0.45;
                double maxScore = searchKeys.Count * 1.0;
                double minScore = Math.Ceiling(maxScore * minRatio * 10) / 10.0;


                var entities = rawResults
                .Select(x => new
                {
                    Attachment = x,
                    RelevanceScore = CalculateRelevanceScore(x, searchKeys.ToArray(), searchQuery)
                })
                .Where(x => x.RelevanceScore >= minScore)
                .OrderByDescending(x => x.RelevanceScore)
                .Select(x => new AttachmentModel
                {
                    Id = x.Attachment.Id,
                    KeyWords = x.Attachment.KeyWords,
                    Description = x.Attachment.Description,
                    Extention = x.Attachment.Extention,
                    IsUsed = x.Attachment.IsUsed,
                    LanguageId = x.Attachment.LanguageId,
                    Title = x.Attachment.Title,
                    TypeBaseId = x.Attachment.TypeBaseId,
                    Url = x.Attachment.Url,
                    RelevanceScore = x.RelevanceScore
                });


                return Library.Models.ServiceResult.Success(new Library.Models.PagedList()
                {
                    Data = entities
                     .Skip((pageIndex - 1) * pageSize)
                     .Take(pageSize)
                     .ToList(),
                    TotalCount = rawResults.Count
                });

            }
            else
            {
                var entities = _unitOfWork.Attachments.Select(x => new AttachmentModel
                {
                    Id = x.Id,
                    KeyWords = x.KeyWords,
                    Description = x.Description,
                    Extention = x.Extention,
                    IsUsed = x.IsUsed,
                    LanguageId = x.LanguageId,
                    Title = x.Title,
                    TypeBaseId = x.TypeBaseId,
                    Url = x.Url,
                });

                return Library.Models.ServiceResult.Success(new Library.Models.PagedList()
                {
                    Data = await entities
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(),
                    TotalCount = await entities.CountAsync()
                });


            }
        }

        double CalculateRelevanceScore(
    Attachment attachment,
    string[] searchKeys,
    string searchQuery
)
        {
            // Normalized query length to affect weights
            const int minLen = 1;
            const int maxLen = 50;
            var normalized = Math.Clamp((double)searchQuery.Length - minLen, 0, maxLen - minLen)
                             / (maxLen - minLen);

            double titleWeight = 0.40 + 0.20 * normalized;
            const double descWeight = 0.10;
            double keywordsWeight = 1.0 - (titleWeight + descWeight);

            double score = 0;
            int matchedKeywordCount = 0;

            foreach (var key in searchKeys.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                bool matched = false;

                if (!string.IsNullOrWhiteSpace(attachment.Title) &&
                    attachment.Title.Contains(key, StringComparison.OrdinalIgnoreCase))
                {
                    score += titleWeight;
                    matched = true;
                }

                if (!string.IsNullOrWhiteSpace(attachment.Description) &&
                    attachment.Description.Contains(key, StringComparison.OrdinalIgnoreCase))
                {
                    score += descWeight;
                    matched = true;
                }

                if (!string.IsNullOrWhiteSpace(attachment.KeyWords) &&
                    attachment.KeyWords.Contains(key, StringComparison.OrdinalIgnoreCase))
                {
                    score += keywordsWeight;
                    matched = true;
                }

                if (matched)
                {
                    matchedKeywordCount++;
                }
            }

            double keywordBonus = Math.Pow(matchedKeywordCount, 1.2) * 0.5;
            score += keywordBonus;

            return score;
        }
    }
}
